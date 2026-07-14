using UnityEngine;
using UnityEngine.UI;

public class UnlockByQuest : MonoBehaviour
{
    public string requiredQuestID;

    public Button button;
    public GameObject lockIcon;

    void Start()
    {
        UpdateState();
    }

    private void OnEnable()
    {
        QuestManager.OnQuestCompleted += OnQuestUpdated;
    }

    private void OnDisable()
    {
        QuestManager.OnQuestCompleted -= OnQuestUpdated;
    }

    void OnQuestUpdated(string questId)
    {
        UpdateState();
    }

    public void UpdateState()
    {
        bool unlocked = IsUnlocked();

        if (button != null)
            button.interactable = unlocked;

        if (lockIcon != null)
            lockIcon.SetActive(!unlocked);
    }

    bool IsUnlocked()
    {
        if (string.IsNullOrEmpty(requiredQuestID))
            return true;

        Quest quest = QuestManager.Instance.GetQuest(requiredQuestID);

        if (quest == null)
            return false;

        return quest.state == QuestState.Completed || quest.state == QuestState.Finished;
    }
    public bool IsUnlockedPublic()
    {
        return IsUnlocked();
    }
}