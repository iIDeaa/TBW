using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public List<QuestData> allQuests;

    private Dictionary<string, Quest> activeQuests = new Dictionary<string, Quest>();

    void Awake()
    {
        Instance = this;
    }

    public Quest GetQuest(string questId)
    {
        if (activeQuests.ContainsKey(questId))
            return activeQuests[questId];

        return null;
    }

    public void StartQuest(QuestData data)
    {
        if (activeQuests.ContainsKey(data.questId)) return;

        Quest q = new Quest
        {
            questID = data.questId,
            questName = data.title,
            targetAmount = data.requiredAmount,
            currentAmount = 0,
            state = QuestState.InProgress
        };

        activeQuests.Add(q.questID, q);
        Debug.Log("Start Quest: " + q.questName);
    }

    public void CompleteQuest(string questId)
    {
        if (!activeQuests.ContainsKey(questId)) return;

        activeQuests[questId].state = QuestState.Completed;
        Debug.Log("Quest Completed: " + questId);
    }

    public void FinishQuest(string questId)
    {
        if (!activeQuests.ContainsKey(questId)) return;

        activeQuests[questId].state = QuestState.Finished;
        Debug.Log("Quest Finished: " + questId);
    }

    public void AddProgress(string questId, int amount)
    {
        if (!activeQuests.ContainsKey(questId)) return;

        var q = activeQuests[questId];
        q.currentAmount += amount;

        if (q.currentAmount >= q.targetAmount)
        {
            q.state = QuestState.Completed;
        }
    }

    // 🔥 TEST MODE
    public void ForceComplete(string questId)
    {
        if (!activeQuests.ContainsKey(questId)) return;

        var q = activeQuests[questId];
        q.currentAmount = q.targetAmount;
        q.state = QuestState.Completed;

        Debug.Log("TEST: Force Complete " + questId);
    }
}