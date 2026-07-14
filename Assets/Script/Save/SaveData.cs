using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int questStep;
    public List<string> finishedQuests;
    public List<string> inProgressQuests;
    public List<string> completedQuests;
    public string currentScene;
    public List<string> itemIDs;
    public List<int> itemAmounts;
    public float playerPosX;
    public float playerPosY;
    public float playerPosZ;
}