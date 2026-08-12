using UnityEngine;
using System.Collections;

public class TrashCoreManager : MonoBehaviour
{
    public static TrashCoreManager Instance;


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

    public void AddTrashChance()
    {
        coreChance += 20f;

        if (coreChance >= 100f)
        {
            SpawnCore();
            coreChance = 0;
        }
    }

    void SpawnCore()
    {
        int type = Random.Range(0, 100);

        GameObject prefab = (type < 50) ? weakCorePrefab : strongCorePrefab;

        TrashSpawner.Instance.SpawnCore(prefab);

        currentCore++;
    }

    public void CheckEndGameSafe()
    {
        StartCoroutine(CheckEndGameDelay());
    }

    IEnumerator CheckEndGameDelay()
    {
        yield return null; 

        CheckEndGame();
    }


    public void RemoveCore()
    {
        currentCore--;
    }

    public void CheckEndGame()
    {
        int trashCount = FindObjectsOfType<Trash>().Length;
        int coreCount = FindObjectsOfType<CarbonCore>().Length;

        Debug.Log("=== CHECK END GAME ===");
        Debug.Log("REAL Trash: " + trashCount);
        Debug.Log("REAL Core: " + coreCount);

        if (trashCount == 0 && coreCount == 0)
        {
            Debug.Log("🎉 END GAME");
            TrashMiniGameManager.Instance.EndGame();
        }
    }
}