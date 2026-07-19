using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    
    public static QuestManager Instance;

    public List<QuestData> allQuests;

    private Dictionary<string, Quest> activeQuests = new Dictionary<string, Quest>();
    void InitializeQuests()
    {
        activeQuests.Clear();

        foreach (var data in allQuests)
        {
            Quest q = new Quest
            {
                questID = data.questId,
                questName = data.title,
                progress = new Dictionary<ItemData, int>(),
                state = QuestState.NotStarted
            };

            // เตรียม progress ไว้
            if (data.requirements != null)
            {
                foreach (var req in data.requirements)
                {
                    q.progress.Add(req.item, 0);
                }
            }

            activeQuests.Add(q.questID, q);
        }

        Debug.Log("Initialized Quests: " + activeQuests.Count);
    }

    public int currentQuestStep = 1;

    // 🔥 Event
    public static event Action<string> OnQuestStarted;
    public static event Action<string> OnQuestCompleted;
    public static event Action<int> OnQuestStepChanged;
    

    void Awake()
    {
        if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
        return;
    }
    Debug.Log("QuestManager Awake: " + GetInstanceID());

    Instance = this;
    DontDestroyOnLoad(gameObject);

    InitializeQuests();
    }
    

    public Quest GetQuest(string questId)
    {
        if (activeQuests.ContainsKey(questId))
            return activeQuests[questId];

        return null;
    }

    public void StartQuest(QuestData data)
    {
        
        Debug.Log(">>> StartQuest CALLED for " + data.questId);
        if (data.questOrder != currentQuestStep) return;

        Quest q = GetQuest(data.questId);

        if (q == null) return;

        if (q.state != QuestState.NotStarted) return;

        q.state = QuestState.InProgress;

        Debug.Log("Start Quest: " + q.questName);
        OnQuestStarted?.Invoke(q.questID);
    }

    public void CompleteQuest(string questId)
    {
        if (!activeQuests.ContainsKey(questId)) return;

        activeQuests[questId].state = QuestState.Completed;

        Debug.Log("Quest Completed: " + questId);
        OnQuestCompleted?.Invoke(questId); //  ยิง event
    }

    public void FinishQuest(string questId)
    {
        if (!activeQuests.ContainsKey(questId)) return;

        activeQuests[questId].state = QuestState.Finished;

        currentQuestStep++; //  เลื่อน quest

        Debug.Log("Quest Finished: " + questId);
        OnQuestStepChanged?.Invoke(currentQuestStep); //  ยิง event
    }

    public void AddProgress(string questId, ItemData item, int amount)
    {
        if (!activeQuests.ContainsKey(questId)) return;

        var q = activeQuests[questId];
        if (!q.progress.ContainsKey(item)) return;

            q.progress[item] += amount;
    }

    //  ใช้หา quest ปัจจุบัน
    public QuestData GetCurrentQuestData()
    {
        foreach (var q in allQuests)
        {
            if (q.questOrder == currentQuestStep)
                return q;
        }
        return null;
    }
    
    void OnQuestChanged(int step)
    {
        // อัปเดต logic หรือ UI ได้
    }
    void OnItemAdded(ItemData item, int amount)
    {
        QuestData current = GetCurrentQuestData();

        if (current == null) return;

        if (current.questType != QuestType.Collect)
            return;

        if (current.requirements == null || current.requirements.Count == 0)
        return;

        foreach (var req in current.requirements)
        {
            if (req.item == item)
            {
                AddProgress(current.questId, item, amount);
            }
        }
    }
    public List<string> GetFinishedQuestIDs()
    {
        List<string> list = new List<string>();

        foreach (var q in activeQuests)
        {
            if (q.Value.state == QuestState.Finished)
                list.Add(q.Key);
        }

        return list;
    }
    public void LoadFinishedQuests(List<string> finishedIDs)
    {
        foreach (var id in finishedIDs)
        {
            if (activeQuests.ContainsKey(id))
            {
                activeQuests[id].state = QuestState.Finished;
            }
        }


        OnQuestStepChanged?.Invoke(currentQuestStep);
    }
    public void StartCurrentQuest()
    {
        QuestData current = GetCurrentQuestData();
        if (current != null)
        {
            StartQuest(current);
        }
    }
    public bool IsQuestActive(string questID)
    {
        Quest quest = GetQuest(questID);

        if (quest == null)
            return false;


        return quest.state == QuestState.InProgress;
    }
    
    
    void OnEnable()
    {
        InventoryManager.OnItemAdded += OnItemAdded;
        QuestManager.OnQuestStepChanged += OnQuestChanged;
    }

    void OnDisable()
    {
        InventoryManager.OnItemAdded -= OnItemAdded;
        QuestManager.OnQuestStepChanged -= OnQuestChanged;
    }
}