using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    public static TrashSpawner Instance;

    public GameObject trashPrefab;
    public int totalTrash = 15;

    private BoxCollider2D area;

    private void Awake()
    {
        Instance = this;
        area = GetComponent<BoxCollider2D>();
    }

    public void StartRound()
    {
        for (int i = 0; i < totalTrash; i++)
        {
            SpawnTrash();
        }
    }

    void SpawnTrash()
    {
        Bounds bounds = area.bounds;

        Vector2 pos = new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );

        Instantiate(trashPrefab, pos, Quaternion.identity);
    }

    public void SpawnCore(GameObject corePrefab)
    {
        Bounds bounds = area.bounds;

        Vector2 pos = new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );

        Instantiate(corePrefab, pos, Quaternion.identity);

        TrashCoreManager.Instance.currentCore++;

    }
}