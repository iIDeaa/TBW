using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    SaveData pendingData;
    bool isLoading = false;
    public bool IsLoaded { get; private set; } = false;
    


    string path;
    void Start()
    {
        
        
        // if (HasSave())
        // {
        //     LoadGame();
        // }
        // InvokeRepeating(nameof(AutoSave), 30f, 30f);
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            path = Application.persistentDataPath + "/save.json";
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 💾 SAVE
    public void SaveGame()
    {
        if (isLoading || !Application.isPlaying) return;
        SaveData data = new SaveData();
        data.itemIDs = new List<string>();
        data.itemAmounts = new List<int>();
        if (isLoading || !Application.isPlaying) return;

        if (InventoryManager.Instance == null || QuestManager.Instance == null)
        {
            Debug.LogWarning("Manager not ready, skip save");
            return;
        }
        foreach (var slot in InventoryManager.Instance.slots)
        {
            if (slot.item != null)
            {
                data.itemIDs.Add(slot.item.itemId);
                data.itemAmounts.Add(slot.amount);
            }
        }

        data.questStep = QuestManager.Instance.currentQuestStep;
        data.finishedQuests = QuestManager.Instance.GetFinishedQuestIDs();
        data.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        data.zoneProgress = ZoneDataManager.Instance.GetAllProgress();

        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            Vector3 pos = player.transform.position;

            data.playerPosX = pos.x;
            data.playerPosY = pos.y;
            data.playerPosZ = pos.z;
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log(File.Exists(path));
        Debug.Log("Game Saved");
    }

    // 📂 LOAD
    public void LoadGame()
    {
        if (!File.Exists(path)) return;

        isLoading = true;

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        pendingData = data;

        FadeManager.Instance.FadeToScene(data.currentScene);
    }
    
    // void AutoSave()
    // {
    //     SaveGame();
    //     Debug.Log("Auto Save (Timer)");
    // }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (isLoading && pendingData != null)
        {
            ApplyDataAfterSceneLoad(pendingData);
            pendingData = null;
            isLoading = false;
            IsLoaded = true;
            return;
        }

        // StartCoroutine(DelayedSave());
    }
    // IEnumerator DelayedSave()
    // {
    //     yield return new WaitForSeconds(0.2f); // รอ manager spawn

    //     SaveGame();
    // }
    void ApplyDataAfterSceneLoad(SaveData data)
    {
        QuestManager.Instance.currentQuestStep = data.questStep;
        QuestManager.Instance.LoadFinishedQuests(data.finishedQuests);
        ZoneDataManager.Instance.LoadProgress(data.zoneProgress);
        

        InventoryManager.Instance.isLoading = true;

        InventoryManager.Instance.ClearInventory();

        for (int i = 0; i < data.itemIDs.Count; i++)
        {
            string id = data.itemIDs[i];
            int amount = data.itemAmounts[i];

            ItemData itemData = ItemDatabase.Instance.GetItemByID(id);

            if (itemData != null)
            {
                InventoryManager.Instance.AddItem(itemData, amount);
            }
        }

        // 🔥 [เพิ่ม] โหลดเสร็จ
        InventoryManager.Instance.isLoading = false;

        // 🔥 [เพิ่ม] รีเฟรช UI ทีเดียว
        InventoryManager.Instance.ForceRefreshUI();
        ProgressBar[] bars = FindObjectsOfType<ProgressBar>();

        foreach (var bar in bars)
        {
            bar.RefreshBar();
        }
        
        StartCoroutine(SetPlayerPosition(data));
        
    }
    
    IEnumerator SetPlayerPosition(SaveData data)
    {
        yield return new WaitForSeconds(0.2f); // รอ player spawn

        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            player.transform.position = new Vector3(
                data.playerPosX,
                data.playerPosY,
                data.playerPosZ
            );

            Debug.Log("Player position loaded");
        }
    }
    public bool HasSave()
    {
        return File.Exists(path);
    }
    
    void OnApplicationQuit()
    {
        // SaveGame();
        Debug.Log("Auto Save (Quit)");
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}