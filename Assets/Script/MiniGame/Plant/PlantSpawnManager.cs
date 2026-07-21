using UnityEngine;

public class PlantSpawnManager : MonoBehaviour
{
    public GameObject grassPrefab;
    public GameObject rockPrefab;

    public Transform spawnArea;

    public int grassAmount = 8;
    public int rockAmount = 5;

    public float rangeX = 4f;
    public float rangeY = 2f;

    void Start()
    {
        SpawnGrass();

        SpawnRock();
    }

    void SpawnGrass()
    {
        PlantMinigameManager.Instance.grassRemain = grassAmount;

        for (int i = 0; i < grassAmount; i++)
        {
            Instantiate(
                grassPrefab,
                RandomPos(),
                Quaternion.identity,
                spawnArea);
        }
    }

    void SpawnRock()
    {
        PlantMinigameManager.Instance.rockRemain = rockAmount;

        for (int i = 0; i < rockAmount; i++)
        {
            Instantiate(
                rockPrefab,
                RandomPos(),
                Quaternion.identity,
                spawnArea);
        }
    }

    Vector3 RandomPos()
    {
        float x = Random.Range(-rangeX, rangeX);
        float y = Random.Range(-rangeY, rangeY);

        return new Vector3(x, y, 0);
    }
}