using UnityEngine;

public class TrashCoreManager : MonoBehaviour
{
    public static TrashCoreManager Instance;

    [Header("Trash")]
    public int remainTrash;
    public int collectedTrash;
    public int totalTrash;

    [Header("Core")]
    public int currentCore;

    [Header("Spawn")]
    public float coreChance = 0f;

    public GameObject weakCorePrefab;
    public GameObject strongCorePrefab;

    void Awake()
    {
        Instance = this;
    }

    // =========================
    // 🧹 เรียกตอนเก็บขยะ
    // =========================
    public void TrashCollected()
    {
        remainTrash--;
        collectedTrash++;

        TrashMiniGameUI.Instance.UpdateTrash(collectedTrash, totalTrash);

        AddTrashChance();

        CheckEndGame();
    }

    // =========================
    // 🔥 เพิ่มโอกาส Core
    // =========================
    void AddTrashChance()
    {
        if (collectedTrash == 2) coreChance += 10f;
        else if (collectedTrash == 5) coreChance += 20f;
        else if (collectedTrash == 8) coreChance += 20f;
        else if (collectedTrash == 12) coreChance += 20f;
        else if (collectedTrash >= 15) coreChance = 100f;

        TrySpawnCore();
    }

    void TrySpawnCore()
    {
        float rand = Random.Range(0f, 100f);

        if (rand <= coreChance)
        {
            int type = Random.Range(0, 100);

            if (type < 50)
                TrashSpawner.Instance.SpawnCore(weakCorePrefab);
            else
                TrashSpawner.Instance.SpawnCore(strongCorePrefab);

            currentCore++; // 🔥 เพิ่ม Core

            coreChance = 0;
            collectedTrash = 0;
        }
    }

    // =========================
    // 💥 Core ตาย
    // =========================
    public void RemoveCore()
    {
        currentCore--;

        CheckEndGame();
    }

    // =========================
    // 🎯 เช็คจบเกม
    // =========================
    public void CheckEndGame()
    {
        if (remainTrash <= 0 && currentCore <= 0)
        {
            Debug.Log("🎉 เกมจบแล้ว!");

            TrashMiniGameManager.Instance.EndGame();
        }
    }
}