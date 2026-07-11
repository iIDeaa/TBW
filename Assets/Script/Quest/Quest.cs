[System.Serializable]
public class Quest
{
    public string questID;
    public string questName;

    public int targetAmount;
    public int currentAmount;

    public string nextQuestID; // chain ต่อไป

    public string startNPC; // ใครให้
    public string endNPC;   // ใครรับส่ง

    public QuestState state;
}