using System.Collections.Generic;

[System.Serializable]
public class Quest
{
    public string questID;
    public string questName;

    public Dictionary<ItemData, int> progress = new Dictionary<ItemData, int>();

    public string nextQuestID; // chain ต่อไป

    public string startNPC; // ใครให้
    public string endNPC;   // ใครรับส่ง

    public QuestState state;
}