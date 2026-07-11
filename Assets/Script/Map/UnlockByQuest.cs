using UnityEngine;
using UnityEngine.UI;

public class UnlockByQuest : MonoBehaviour
{
    [Header("Leave Empty = Unlock from Start")]
    public string requiredQuestID;

    [Header("UI")]
    public Button button;
    public GameObject lockIcon;

    void Start()
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
        // ถ้าไม่กำหนด Quest = ปลดล็อกตั้งแต่แรก
        if (string.IsNullOrEmpty(requiredQuestID))
            return true;

        Quest quest = QuestManager.Instance.GetQuest(requiredQuestID);

        if (quest == null)
            return false;

        return quest.state == QuestState.Finished;
    }
}