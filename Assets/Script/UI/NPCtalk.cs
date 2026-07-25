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
    

    QuestManager qm => QuestManager.Instance;

    [Header("Typing")]
    public float typingSpeed = 0.04f;
    [Header("Default Dialogue")]
    public DialogueData defaultDialogue;
    [Header("Portrait")]
    public Image portraitImage;
    [Header("TellImage")]
    public Image tellImage;

    private bool playerInRange;
    private bool isTalking;
    private bool isTyping;
    private int currentLine;
    private QuestData pendingQuest;
    private bool isTurnInPhase = false;
    

    private DialogueData.Line[] currentLines;

    
    IEnumerator Start()
    {
        // รอให้ Load เสร็จก่อน
        yield return new WaitUntil(() =>
        SaveManager.Instance == null ||
        SaveManager.Instance.IsLoaded ||
        !SaveManager.Instance.HasSave()
    );

        dialoguePanel.SetActive(false);

        if (prompt != null)
            prompt.SetActive(false);

        MinigameManager.Instance.OnMinigameEnd += OnMinigameFinished;
    }

    void Update()
    {
        if (!playerInRange)
            return;
        if (SaveManager.Instance != null &&
        !SaveManager.Instance.IsLoaded &&
        SaveManager.Instance.HasSave())
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
        FindObjectOfType<PlayerMovements>().canMove = false;

        DialogueData dialogueToUse = null;
        
        QuestData currentQuest = GetAvailableQuest();

        

        if (currentQuest != null)
        {
            
            Quest quest = qm.GetQuest(currentQuest.questId);
            Debug.Log(">>> BEFORE TALK state = " + quest.state);

            //  ยังไม่เริ่ม
        if (quest.state == QuestState.NotStarted)
        {
            dialogueToUse = currentQuest.startDialogue;
            pendingQuest = currentQuest;

            

            

            if (currentQuest.questType == QuestType.Minigame)
            {
                StartCoroutine(StartMinigameAfterDialogue(currentQuest));
            }
        }

            //  กำลังทำ
            else if (quest.state == QuestState.InProgress)
            {
                if (currentQuest.questType == QuestType.Collect)
                {
                    var inv = InventoryManager.Instance;

                    bool hasAll = true;

                    foreach (var req in currentQuest.requirements)
                    {
                        if (!inv.HasItem(req.item, req.amount))
                        {
                            hasAll = false;
                            break;
                        }
                    }

                    if (hasAll)
                    {
                        // 🔥 มีของครบ → ไป dialogue ส่งของเลย
                        dialogueToUse = currentQuest.completeDialogue;

                        // mark ว่าจะส่งของหลังคุยจบ
                        if (pendingQuest == null)
                        pendingQuest = currentQuest;
                        isTurnInPhase = true;
                        
                    }
                    else
                    {
                        dialogueToUse = currentQuest.inProgressDialogue;
                    }
                }
                else
                {
                    dialogueToUse = currentQuest.inProgressDialogue;
                }
            
                if (currentQuest.questType == QuestType.Minigame)
                {
                    StartCoroutine(StartMinigameAfterDialogue(currentQuest));
                    IEnumerator StartMinigameAfterDialogue(QuestData quest)
                    {
                        Debug.Log("Waiting dialogue end");

                        yield return new WaitUntil(() => !isTalking);

                        Debug.Log("Start Minigame : " + quest.minigameId);

                        MinigameManager.Instance.StartMinigame(quest.minigameId);
                    }
                }
            }
            //  ทำเสร็จ
          else if (quest.state == QuestState.Completed)
            {
                dialogueToUse = currentQuest.completeDialogue;
                pendingQuest = currentQuest; // ไป finish ตอน EndDialogue
            }
            //  ทำจบแล้ว
            else if (quest.state == QuestState.Finished)
            {
                dialogueToUse = currentQuest.completeDialogue;
            }
        }
        else
        {
            dialogueToUse = defaultDialogue;
        }

        if (dialogueToUse == null)
        {
            Debug.LogWarning("Fallback to default dialogue");
            dialogueToUse = defaultDialogue;
        }

        
       

        currentLines = dialogueToUse.lines;
        StartCoroutine(TypeLine());
    }
    IEnumerator StartMinigameAfterDialogue(QuestData quest)
    {
        yield return new WaitUntil(() => !isTalking);
        MinigameManager.Instance.StartMinigame(quest.minigameId);
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
        if (tellImage != null)
        {
            Sprite image = currentLines[currentLine].tellimage;

            if (image != null)
            {
                tellImage.sprite = image;
                tellImage.gameObject.SetActive(true);
            }
            else
            {
                tellImage.sprite = null;
                tellImage.gameObject.SetActive(false);
            }
        }

        foreach (char c in currentLines[currentLine].text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        if (pendingQuest != null)
        {
            Quest quest = qm.GetQuest(pendingQuest.questId);
            if (quest.state == QuestState.NotStarted)
            {
                qm.StartQuest(pendingQuest);
            }

            if (quest != null)
            {
                // 🔥 FIX: Talk Quest
                if (quest.state == QuestState.InProgress &&
                    pendingQuest.questType == QuestType.Talk)
                {
                    Debug.Log("Talk Complete");

                    qm.CompleteQuest(pendingQuest.questId);
                    qm.FinishQuest(pendingQuest.questId);
                }

                // Collect
                else if (quest.state == QuestState.InProgress && isTurnInPhase)
                {
                    HandleQuest(pendingQuest);
                }

                // Completed → Finish
                else if (quest.state == QuestState.Completed)
                {
                    qm.FinishQuest(pendingQuest.questId);
                }
            }
        }
        isTalking = false;
        dialoguePanel.SetActive(false);

        FindObjectOfType<PlayerMovements>().canMove = true;

        
       

        pendingQuest = null;
        isTurnInPhase = false;
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
        Debug.Log("=== Checking Available Quest ===");

        foreach (QuestData q in quests)
        {
            Debug.Log($"Checking Quest {q.questId} | Order {q.questOrder}");

            if (q.questOrder != qm.currentQuestStep)
            {
                Debug.Log(" Order not match");
                continue;
            }

            Quest quest = qm.GetQuest(q.questId);

            if (quest == null)
            {
                Debug.Log(" Quest missing (should not happen)");
                continue;
            }

            Debug.Log("State: " + quest.state);

            // 🔥 แก้ตรงนี้
            if (quest.state == QuestState.NotStarted)
            {
                Debug.Log(" HIT NOT STARTED");
                return q;
            }

            if (quest.state == QuestState.InProgress)
            {
                Debug.Log(" In Progress FOUND");
                return q;
            }

            if (quest.state == QuestState.Completed)
            {
                Debug.Log(" Completed FOUND");
                return q;
            }
        }

        Debug.Log(" No quest found");
        return null;
    }
    
    void HandleQuest(QuestData quest)
    {
        Debug.Log("HandleQuest called: " + quest.title);
        //  Talk จบทันที
        if (quest.questType == QuestType.Talk)
        {
            QuestManager.Instance.FinishQuest(quest.questId);
        }

        // Collect เช็คหลาย item
        else if (quest.questType == QuestType.Collect)
        {
            var inv = InventoryManager.Instance;

            // เช็คว่าครบทุก item ไหม
            bool hasAll = true;

            foreach (var req in quest.requirements)
            {
                if (!inv.HasItem(req.item, req.amount))
                {
                    hasAll = false;
                    break;
                }
            }

            if (!hasAll)
            {
                Debug.Log("Item not enough");
                return;
            }

            // ถ้าครบ ลบทุก item
            foreach (var req in quest.requirements)
            {
                inv.RemoveItem(req.item, req.amount);
            }

            //จบเควส
            QuestManager.Instance.FinishQuest(quest.questId);

            Debug.Log("Turn in success");
        }
    }
    void OnMinigameFinished(bool success)
    {
        if (!success) return;

        QuestData q = GetAvailableQuest();
        if (q == null) return;

        Quest quest = qm.GetQuest(q.questId);

        if (quest != null && quest.state == QuestState.InProgress)
        {
            qm.CompleteQuest(q.questId);
        }
    }


    // หา quest ตัวถัดไปจาก ID
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



    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        
    }
}