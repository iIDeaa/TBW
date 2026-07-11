using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("กล่องข้อความทั้งหมด (Panel/Image ที่เป็น BoxText)")]
    public GameObject dialogueBox;

    [Tooltip("Text component สำหรับแสดงข้อความ")]
    public TextMeshProUGUI dialogueText;

    [Header("Dialogues")]
    [Tooltip("ใส่ข้อความทั้งหมดที่ต้องการแสดงตรงนี้ได้เลย")]
    public DialogueLine[] dialogueLines;

    [Header("Settings")]
    [Tooltip("ความเร็วในการพิมพ์ตัวอักษร (ตัวต่อวินาที)")]
    public float typingSpeed = 0.05f;

    [Tooltip("เลือกโหมดการข้ามข้อความ")]
    public AdvanceMode advanceMode = AdvanceMode.ClickToAdvance;

    [Tooltip("หน่วงเวลาก่อนไปข้อความถัดไปอัตโนมัติ (วินาที) - ใช้เมื่อเลือก AutoAdvance)")]
    public float autoAdvanceDelay = 2f;

    public enum AdvanceMode
    {
        ClickToAdvance,  // กดคลิก/Space เพื่อไปต่อ
        AutoAdvance,     // ไปเองอัตโนมัติหลังข้อความพิมพ์จบ
    }

    private int currentIndex = 0;
    private bool isTyping = false;
    private bool isDialogueActive = false;
    private Coroutine typingCoroutine;
    private Coroutine autoAdvanceCoroutine;

    private void OnEnable()
    {
        // เรียกทุกครั้งที่ Timeline เปิด BoxText
        StartDialogue();
    }

    private void Update()
    {
        // กดคลิก/Space เพื่อไปต่อ (เฉพาะโหมด ClickToAdvance)
        if (advanceMode == AdvanceMode.ClickToAdvance)
        {
            if (isDialogueActive && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
            {
                OnDialogueClick();
            }
        }
    }

    public void StartDialogue()
    {
        if (dialogueLines.Length == 0) return;

        currentIndex = 0;
        isDialogueActive = true;

        if (dialogueBox != null)
            dialogueBox.SetActive(true);

        ShowLine(currentIndex);
    }

    /// <summary>
    /// เรียกเมื่อกดที่กล่องข้อความ
    /// </summary>
    public void OnDialogueClick()
    {
        if (isTyping)
        {
            // กำลังพิมพ์อยู่ → แสดงทั้งหมดทันที
            SkipTyping();
        }
        else
        {
            // พิมพ์จบแล้ว → ไปข้อความถัดไป
            NextLine();
        }
    }

    private void ShowLine(int index)
    {
        if (index >= dialogueLines.Length) return;

        // หยุด auto advance เก่าถ้ามี
        if (autoAdvanceCoroutine != null)
            StopCoroutine(autoAdvanceCoroutine);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(dialogueLines[index].dialogueText));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        // ถ้าโหมด AutoAdvance → รอแล้วไปต่อเอง
        if (advanceMode == AdvanceMode.AutoAdvance)
        {
            autoAdvanceCoroutine = StartCoroutine(AutoAdvanceAfterDelay());
        }
    }

    private IEnumerator AutoAdvanceAfterDelay()
    {
        yield return new WaitForSeconds(autoAdvanceDelay);
        NextLine();
    }

    private void SkipTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.text = dialogueLines[currentIndex].dialogueText;
        isTyping = false;

        // ถ้าโหมด AutoAdvance → เริ่มนับ delay ทันที
        if (advanceMode == AdvanceMode.AutoAdvance)
        {
            autoAdvanceCoroutine = StartCoroutine(AutoAdvanceAfterDelay());
        }
    }

    private void NextLine()
    {
        currentIndex++;

        if (currentIndex < dialogueLines.Length)
        {
            ShowLine(currentIndex);
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        isDialogueActive = false;

        if (dialogueBox != null)
            dialogueBox.SetActive(false);

        Debug.Log("[DialogueManager] Dialogue จบแล้ว!");
    }
}

[System.Serializable]
public class DialogueLine
{
    [Tooltip("ข้อความที่พูด")]
    [TextArea(2, 5)]
    public string dialogueText;
}
