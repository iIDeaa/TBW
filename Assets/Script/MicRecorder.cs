using UnityEngine;
using System;

public class MicRecorder : MonoBehaviour
{
    public string deviceName;
    public AudioClip audioClip;

    public int sampleRate = 16000;
    public int maxRecordTime = 10;

    public GoogleSpeechClient speechClient;

    private bool isRecording = false;
    private int lastSamplePosition = 0;

    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            deviceName = Microphone.devices[0];
            Debug.Log("Mic Found: " + deviceName);
        }
        else
        {
            Debug.LogError("No microphone detected!");
        }
    }

    public void StartRecording()
    {
        if (isRecording) return;

        lastSamplePosition = 0;
        audioClip = Microphone.Start(deviceName, true, maxRecordTime, sampleRate);
        isRecording = true;

        Debug.Log("Recording started...");
    }

    public void StopRecording()
    {
        if (!isRecording) return;

        Microphone.End(deviceName);
        isRecording = false;
        lastSamplePosition = 0;

        Debug.Log("Recording stopped.");
    }

    void Update()
    {
        if (isRecording && audioClip != null)
        {
            int micPosition = Microphone.GetPosition(deviceName);

            if (micPosition <= 0 || micPosition == lastSamplePosition)
                return;

            int sampleCount;
            if (micPosition > lastSamplePosition)
            {
                sampleCount = micPosition - lastSamplePosition;
            }
            else
            {
                // Mic wrapped around (loop recording)
                sampleCount = (audioClip.samples - lastSamplePosition) + micPosition;
            }

            if (sampleCount > 0)
            {
                float[] samples = new float[sampleCount];
                audioClip.GetData(samples, lastSamplePosition);
                lastSamplePosition = micPosition;

                byte[] bytes = ConvertToPCM16(samples);
                speechClient.SendAudio(bytes);
            }
        }
    }

    byte[] ConvertToPCM16(float[] samples)
    {
        byte[] bytes = new byte[samples.Length * 2];
        int rescaleFactor = 32767;

        for (int i = 0; i < samples.Length; i++)
        {
            short value = (short)(samples[i] * rescaleFactor);
            byte[] byteArr = BitConverter.GetBytes(value);

            bytes[i * 2] = byteArr[0];
            bytes[i * 2 + 1] = byteArr[1];
        }

        return bytes;
    }
}