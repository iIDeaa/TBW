using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCtalk : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public GameObject prompt;

    [Header("Dialogue")]
    public string npcName;

    [Header("Quest")]
    public List<QuestData> quests;
    

    [Header("Player")]
    public PlayerMove playerMovement;

    [Header("Typing")]
    public float typingSpeed = 0.04f;
    [Header("Default Dialogue")]
    public DialogueData defaultDialogue;
    [Header("Portrait")]
    public Image portraitImage;

    private bool playerInRange;
    private bool isTalking;
    private bool isTyping;
    private int currentLine;
    

    private DialogueData.Line[] currentLines;

    void Start()
    {
        dialoguePanel.SetActive(false);

        if (prompt != null)
            prompt.SetActive(false);
    }

    void Update()
    {
        if (!playerInRange)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isTalking)
                StartDialogue();
            else
                NextLine();
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        currentLine = 0;

        dialoguePanel.SetActive(true);
        playerMovement.canMove = false;

        DialogueData dialogueToUse = null;

        QuestData currentQuest = GetAvailableQuest();

        if (currentQuest != null)
        {
            QuestState state = playerMovement.GetQuestState(currentQuest);

            if (state == QuestState.NotStarted)
            {
                dialogueToUse = currentQuest.startDialogue;
                playerMovement.StartQuest(currentQuest);
            }
            else if (state == QuestState.InProgress)
            {
                dialogueToUse = currentQuest.inProgressDialogue;
                playerMovement.CompleteQuest(currentQuest);
            }
            else if (state == QuestState.Completed)
            {
                dialogueToUse = currentQuest.completeDialogue;
                playerMovement.FinishQuest(currentQuest);
            }
            else if (state == QuestState.Finished)
            {
                dialogueToUse = currentQuest.completeDialogue;
            }
        }
        else
        {
            // 🔥 ไม่มีเควส → ใช้บทพูดทั่วไป
            dialogueToUse = defaultDialogue;
        }

        if (dialogueToUse == null)
        {
            Debug.LogWarning("No dialogue assigned!");
            return;
        }

        currentLines = dialogueToUse.lines;

        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentLines[currentLine].text;
            isTyping = false;
            return;
        }

        currentLine++;

        if (currentLine < currentLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";

        nameText.text = currentLines[currentLine].speaker;
        portraitImage.sprite = currentLines[currentLine].portrait;

        foreach (char c in currentLines[currentLine].text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        isTalking = false;
        dialoguePanel.SetActive(false);

        playerMovement.canMove = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (prompt != null)
                prompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (isTalking)
                EndDialogue();

            if (prompt != null)
                prompt.SetActive(false);
        }
    }
    QuestData GetAvailableQuest()
    {
        foreach (QuestData q in quests)
        {
            // 🔥 เช็คว่าเป็นลำดับปัจจุบันไหม
            if (q.questOrder != playerMovement.currentQuestStep)
                continue;

            QuestState state = playerMovement.GetQuestState(q);

            if (state == QuestState.NotStarted)
                return q;

            if (state == QuestState.InProgress)
                return q;

            if (state == QuestState.Completed)
                return q;
        }

        return null;
    }

    // 🔥 หา quest ตัวถัดไปจาก ID
    QuestData FindNextQuest(string id)
    {
        QuestData[] allQuests = Resources.LoadAll<QuestData>("");

        foreach (QuestData q in allQuests)
        {
            if (q.questId == id)
                return q;
        }

        return null;
    }
}