using UnityEngine;

public class TrashCoreManager : MonoBehaviour
{
    public static TrashCoreManager Instance;

    public int collectedTrash = 0;
    public float coreChance = 0f;

    public GameObject weakCorePrefab;
    public GameObject strongCorePrefab;

    void Awake()
    {
        Instance = this;
    }

    public void AddTrash()
    {
        collectedTrash++;

        // 🔥 ระบบสะสมโอกาส
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

            coreChance = 0;
            collectedTrash = 0;
        }
    }
}