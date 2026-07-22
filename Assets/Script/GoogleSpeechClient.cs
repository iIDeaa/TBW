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

    async void Start()
    {
        await ConnectWebSocket();
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
    }
    public void ResetSession()
    {

        sessionId++;
        Debug.Log("[Speech] Session reset: " + sessionId);
    }
}
