const speech = require('@google-cloud/speech');
const WebSocket = require('ws');

const client = new speech.SpeechClient({
  keyFilename: 'speechtext-501014-fdd0a6140314.json',
});

const wss = new WebSocket.Server({ port: 8080 });

wss.on('connection', function connection(ws) {
  console.log("Client connected");

  let recognizeStream;

  const startStream = () => {
  recognizeStream = client
    .streamingRecognize({
      config: {
        encoding: 'LINEAR16',
        sampleRateHertz: 16000,
        languageCode: 'th-TH',
      },
      interimResults: true,
    })
    .on('data', data => {
      const text = data.results[0]?.alternatives[0]?.transcript;
      if (text) {
        console.log("Speech:", text);
        ws.send(text);
      }
    })
    .on('error', err => {
      console.error("Speech error:", err);
      recognizeStream = null; // 🔥 สำคัญ
    });
};

  startStream();

  ws.on('message', function incoming(message) {
  try {
    // ถ้า C# ส่งคำสั่งให้รีเซ็ตสตรีมกลับมา
    if (message.toString() === "RESET_STREAM") {
      console.log("Forced stream reset requested by client");
      if (recognizeStream) {
        recognizeStream.destroy();
        recognizeStream = null;
      }
      return;
    }

    if (!recognizeStream || recognizeStream.destroyed) {
      console.log("Restarting stream...");
      startStream();
    }

    recognizeStream.write(message);
  } catch (err) {
    console.error("Write error:", err);
  }
});

  ws.on('close', () => {
    console.log("Client disconnected");
    if (recognizeStream) recognizeStream.destroy();
  });
});

console.log("Server running on ws://localhost:8080");