using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    [Header("Player")]
    public PlayerMove player;

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
        currentQuest = player.GetCurrentQuest();

        detailPanel.SetActive(false);

        if(currentQuest == null)
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
        Debug.Log("Detail");
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
}