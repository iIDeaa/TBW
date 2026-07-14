using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    

    [Header("Quest Button")]
    public Button questButton;
    public TMP_Text questButtonText;

    [Header("Detail Panel")]
    public GameObject detailPanel;

    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text rewardText;

    private QuestData currentQuest;
    private bool showingDetail;

    private void Start()
    {
        
        detailPanel.SetActive(false);

        RefreshQuest();
    }

    public void RefreshQuest()
    {
        currentQuest = QuestManager.Instance.GetCurrentQuestData();

        detailPanel.SetActive(false);

        if (currentQuest == null)
        {
            questButtonText.text = "";
            questButton.interactable = false;
            return;
        }

        questButton.interactable = true;
        questButtonText.text = currentQuest.title;
    }

   public void ShowDetail()
    {
        if(currentQuest == null)
            return;

        showingDetail = !showingDetail;

        detailPanel.SetActive(showingDetail);

        if(showingDetail)
        {
            titleText.text = currentQuest.title;
            descriptionText.text = currentQuest.description;
            rewardText.text = "Reward : " + currentQuest.rewardGold + " Gold";
        }
    }
    void OnEnable()
    {
        QuestManager.OnQuestStepChanged += OnQuestChanged;
        QuestManager.OnQuestStarted += OnQuestChangedSimple;
        QuestManager.OnQuestCompleted += OnQuestChangedSimple;
    }

    void OnDisable()
    {
        QuestManager.OnQuestStepChanged -= OnQuestChanged;
        QuestManager.OnQuestStarted -= OnQuestChangedSimple;
        QuestManager.OnQuestCompleted -= OnQuestChangedSimple;
    }

    void OnQuestChanged(int step)
    {
        RefreshQuest();
    }

    void OnQuestChangedSimple(string id)
    {
        RefreshQuest();
    }
}