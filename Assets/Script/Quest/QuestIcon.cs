using UnityEngine;


public class QuestIcon : MonoBehaviour
{
    [Header("Icon")]
    [SerializeField] private GameObject icon;


    [Header("Quest List")]
    [SerializeField] private QuestData[] quests;



    private void Start()
    {
        UpdateIcon();


        QuestManager.OnQuestStarted += OnQuestChanged;
        QuestManager.OnQuestCompleted += OnQuestChanged;
        QuestManager.OnQuestStepChanged += OnStepChanged;
    }



    private void OnDestroy()
    {
        QuestManager.OnQuestStarted -= OnQuestChanged;
        QuestManager.OnQuestCompleted -= OnQuestChanged;
        QuestManager.OnQuestStepChanged -= OnStepChanged;
    }



    private void OnQuestChanged(string id)
    {
        UpdateIcon();
    }


    private void OnStepChanged(int step)
    {
        UpdateIcon();
    }



    private void UpdateIcon()
    {
        bool show = false;


        foreach (QuestData questData in quests)
        {
            if(QuestManager.Instance.IsQuestActive(questData.questId))
            {
                show = true;
                break;
            }
        }


        icon.SetActive(show);
    }
}