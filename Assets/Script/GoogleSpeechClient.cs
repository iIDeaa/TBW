using UnityEngine;
using NativeWebSocket;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class GoogleSpeechClient : MonoBehaviour
{
    WebSocket websocket;
    private Process serverProcess;

    private int sessionId = 0;
    public Action<string> OnTextReceived;

    private bool isConnecting = false;
    private float reconnectDelay = 2f;
    private float reconnectTimer = 0f;
    private bool shouldReconnect = true;

    async void Start()
    {
        LaunchSpeechServer();
        await ConnectWebSocket();
    }

    void LaunchSpeechServer()
    {
        try
        {
            // ตรวจสอบว่ามี process รันอยู่แล้วหรือไม่ (เพื่อไม่ให้เปิดซ้ำซ้อน)
            Process[] processes = Process.GetProcessesByName("node");
            foreach (var p in processes)
            {
                // หากรันจากตำแหน่งหรือชื่อที่เกี่ยวข้อง อาจจะไม่ต้องเปิดเพิ่ม แต่ในที่นี้จะเปิดขึ้นมาใหม่เลย
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            // รัน node.exe โดยตรงจากไฟล์ที่อยู่ใน StreamingAssets
            startInfo.FileName = "node";
            string serverJsPath = System.IO.Path.Combine(Application.streamingAssetsPath, "speech-server", "server.js");
            startInfo.Arguments = $"\"{serverJsPath}\"";
            startInfo.WorkingDirectory = System.IO.Path.Combine(Application.streamingAssetsPath, "speech-server");
            startInfo.CreateNoWindow = true; 
            startInfo.UseShellExecute = false; // ต้องเป็น false เพื่อไม่ให้แสดงหน้าต่างขึ้นมา
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;

            serverProcess = Process.Start(startInfo);
            
            // อ่าน Log ใน Unity Console เผื่อต้องการตรวจสอบการทำงาน
            serverProcess.OutputDataReceived += (sender, args) => {
                if (!string.IsNullOrEmpty(args.Data)) UnityEngine.Debug.Log("[Speech Server Out]: " + args.Data);
            };
            serverProcess.ErrorDataReceived += (sender, args) => {
                if (!string.IsNullOrEmpty(args.Data)) UnityEngine.Debug.LogError("[Speech Server Err]: " + args.Data);
            };
            serverProcess.BeginOutputReadLine();
            serverProcess.BeginErrorReadLine();

            UnityEngine.Debug.Log("[Speech] Started speech-server in background automatically.");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[Speech] Failed to start server: " + ex.Message);
        }
    }

    async System.Threading.Tasks.Task ConnectWebSocket()
    {
        if (isConnecting) return;
        isConnecting = true;

        try
        {
            Debug.Log("Trying to connect to ws://localhost:8080...");

            websocket = new WebSocket("ws://localhost:8080");

            websocket.OnOpen += () =>
            {
                Debug.Log("Connected to server");
                reconnectTimer = 0f;
            };

            websocket.OnError += (e) =>
            {
                Debug.LogError("WebSocket Error: " + e);
            };

            websocket.OnClose += (e) =>
            {
                Debug.Log("Connection closed: " + e);
            };

            websocket.OnMessage += (bytes) =>
            {
                int currentSession = sessionId;

                string text = System.Text.Encoding.UTF8.GetString(bytes);

                if (currentSession != sessionId) return;

                Debug.Log("Received: " + text);

                OnTextReceived?.Invoke(text);
            };

            await websocket.Connect();
        }
        catch (Exception ex)
        {
            Debug.LogError("WebSocket connection failed: " + ex.Message);
        }
        finally
        {
            isConnecting = false;
        }
    }

    public async void SendAudio(byte[] data)
    {
        if (websocket != null && websocket.State == WebSocketState.Open)
        {
            await websocket.Send(data);
        }
    }

    void Update()
    {
        if (websocket != null)
        {
            websocket.DispatchMessageQueue();

            // Auto-reconnect if disconnected
            if (shouldReconnect && websocket.State == WebSocketState.Closed && !isConnecting)
            {
                reconnectTimer += Time.deltaTime;
                if (reconnectTimer >= reconnectDelay)
                {
                    reconnectTimer = 0f;
                    Debug.Log("Attempting to reconnect...");
                    _ = ConnectWebSocket();
                }
            }
        }
    }

    async void OnDestroy()
    {
        shouldReconnect = false;
        if (websocket != null && websocket.State == WebSocketState.Open)
        {
            await websocket.Close();
        }

        // ปิด Speech Server เมื่อปิดเกมหรือหยุดเล่น
        if (serverProcess != null && !serverProcess.HasExited)
        {
            try
            {
                serverProcess.Kill();
                UnityEngine.Debug.Log("[Speech] Stopped speech-server.");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("[Speech] Error stopping server: " + ex.Message);
            }
        }
    }
    public async void ResetSession()
    {
        sessionId++;
        Debug.Log("[Speech] Session reset: " + sessionId);

        // ส่งข้อความไปสั่งให้เซิร์ฟเวอร์เคลียร์และเปิดสตรีมใหม่
        if (websocket != null && websocket.State == WebSocketState.Open)
        {
            await websocket.SendText("RESET_STREAM");
        }
    }
}
