using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Game/Quest")]
public class QuestData : ScriptableObject
{
    public int questOrder;
    public string questId;
    public string title;
    public string description;

    public int requiredAmount;
    public string requiredItem;

    public int rewardGold;

    public DialogueData startDialogue;
    public DialogueData inProgressDialogue;
    public DialogueData completeDialogue;
    public string nextQuestID;
}
