using UnityEngine;
using NativeWebSocket;
using System;

public class GoogleSpeechClient : MonoBehaviour
{
    WebSocket websocket;

    private int sessionId = 0;
    public Action<string> OnTextReceived;

    private bool isConnecting = false;
    private float reconnectDelay = 2f;
    private float reconnectTimer = 0f;
    private bool shouldReconnect = true;

    private System.Diagnostics.Process serverProcess;

    void Awake()
    {
        StartServer();
    }

    async void Start()
    {
        await ConnectWebSocket();
    }

    void StartServer()
    {
        try
        {
            string serverPath = System.IO.Path.Combine(Application.streamingAssetsPath, "speech-server");
            string batPath = System.IO.Path.Combine(serverPath, "run-server.bat");

            if (!System.IO.File.Exists(batPath))
            {
                Debug.LogError("[SpeechServer] Cannot find run-server.bat at: " + batPath);
                return;
            }

            Debug.Log("[SpeechServer] Starting server from: " + batPath);

            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/c run-server.bat",
                WorkingDirectory = serverPath,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            serverProcess = System.Diagnostics.Process.Start(startInfo);
            Debug.Log("[SpeechServer] Server process started in background.");
        }
        catch (Exception ex)
        {
            Debug.LogError("[SpeechServer] Failed to start server: " + ex.Message);
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
        StopServer();
    }

    void OnApplicationQuit()
    {
        StopServer();
    }

    void StopServer()
    {
        if (serverProcess != null && !serverProcess.HasExited)
        {
            try
            {
                serverProcess.Kill();
                serverProcess.Dispose();
                Debug.Log("[SpeechServer] Server process stopped.");
            }
            catch (Exception ex)
            {
                Debug.LogError("[SpeechServer] Failed to stop server process: " + ex.Message);
            }
        }
    }
    public void ResetSession()
    {

        sessionId++;
        Debug.Log("[Speech] Session reset: " + sessionId);
    }
}
