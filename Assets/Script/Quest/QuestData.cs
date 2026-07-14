using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum QuestType
{
    Talk,       // คุยแล้วจบ
    Collect,    // ส่งของ
    Minigame    // เล่นเกม
}

[CreateAssetMenu(fileName = "NewQuest", menuName = "Game/Quest")]

public class QuestData : ScriptableObject
{
    public int questOrder;
    public string questId;
    public string title;
    public string description;

    public QuestType questType;   
    public List<Requirement> requirements;
    [Header("Minigame")]
    public string minigameId;

    // ใช้เฉพาะ Collect
    [System.Serializable]
    public class Requirement
    {
        public ItemData item;
        public int amount;
    }

    public int rewardGold;

    public DialogueData startDialogue;
    public DialogueData inProgressDialogue;
    public DialogueData completeDialogue;

    public string nextQuestID;
}