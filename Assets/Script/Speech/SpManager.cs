using UnityEngine;
using TMPro;

public class SpManager : MonoBehaviour
{
    public TMP_Text targetTextUI;
    public TMP_Text recognizedTextUI;
    public TMP_Text resultTextUI;

    public MicRecorder mic;
    public TextComparer comparer;
    public GoogleSpeechClient googleSpeech;

    string targetText = "ควย";
    string currentText = "";

    void Start()
    {
        targetTextUI.text = "Target: " + targetText;

        googleSpeech.OnTextReceived += OnSpeechText;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartListening();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopListening();
        }
    }

    void StartListening()
    {
        // รีเซ็ตข้อมูลคำพูดและ Session เดิมก่อนทุกครั้ง
        ResetSpeech();

        recognizedTextUI.text = "Listening...";
        resultTextUI.text = "";
        currentText = "";

        mic.StartRecording();
    }

    void StopListening()
    {
        mic.StopRecording();
        Evaluate();
    }

    void OnSpeechText(string text)
    {
        // ป้องกันผลลัพธ์ที่ตอบกลับมาดีเลย์จากฝั่งเซิร์ฟเวอร์มาทับตอนที่กดปล่อยปุ่มหยุดพูดไปแล้ว
        if (mic != null && !mic.IsRecording()) return;
        if (string.IsNullOrEmpty(text)) return;

        // ล้างช่องว่างที่อาจจะเกิดจากการแปลภาษา
        text = text.Trim();

        recognizedTextUI.text = "You said: " + text;
        currentText = text;
    }

    void Evaluate()
    {
        bool isCorrect = comparer.IsMatch(currentText);

        if (isCorrect)
            resultTextUI.text = "Correct";
        else
            resultTextUI.text = "Incorrect - Try Again";
    }

    // ⭐ FIX จริงอยู่ตรงนี้
    public void ResetSpeech()
    {
        Debug.Log("[Speech] Reset");

        currentText = "";

        recognizedTextUI.text = "";
        resultTextUI.text = "";

        // รีเซ็ต stream ฝั่ง server
        googleSpeech.ResetSession();
    }
}