using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public bool canMove = true;
    private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private Dictionary<QuestData, QuestState> questStates = new Dictionary<QuestData, QuestState>();
    public List<QuestData> allQuests;
    public int currentQuestStep = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (canMove)
            rb.velocity = moveInput * moveSpeed;
        else
            rb.velocity = Vector2.zero;
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!canMove)
        {
            moveInput = Vector2.zero;
            return;
        }

        moveInput = context.ReadValue<Vector2>();
    }
    

    

    public QuestState GetQuestState(QuestData quest)
    {
        if (questStates.ContainsKey(quest))
            return questStates[quest];

        return QuestState.NotStarted;
    }

    public void StartQuest(QuestData quest)
    {
        if (quest.questOrder != currentQuestStep)
            return;

        if (!questStates.ContainsKey(quest))
        {
            questStates.Add(quest, QuestState.InProgress);

             
        }
    }

    
    public void FinishQuest(QuestData quest)
    {
        if (questStates.ContainsKey(quest))
        {
            questStates[quest] = QuestState.Finished;

            
            currentQuestStep++;
        }
    }

    
    public void CompleteQuest(QuestData quest)
    {
        if (questStates.ContainsKey(quest))
        {
            questStates[quest] = QuestState.Completed;
        }
    }
    public QuestData GetCurrentQuest()
    {
        foreach (QuestData quest in allQuests)
        {
            if (quest.questOrder == currentQuestStep)
            {
                QuestState state = GetQuestState(quest);

                if (state == QuestState.InProgress || state == QuestState.Completed)
                {
                    return quest;
                }
            }
        }

        return null;
    }
}