using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Dictionary<string, QuestState> questStates = new();

    public QuestState GetQuestState(QuestData quest)
    {
        if (!questStates.ContainsKey(quest.questId))
            return QuestState.NotStarted;

        return questStates[quest.questId];
    }

    public void AcceptQuest(QuestData quest)
    {
        questStates[quest.questId] = QuestState.InProgress;
    }

    public void CompleteQuest(QuestData quest)
    {
        questStates[quest.questId] = QuestState.Completed;
    }

    public void FinishQuest(QuestData quest)
    {
        questStates[quest.questId] = QuestState.Finished;
    }
}