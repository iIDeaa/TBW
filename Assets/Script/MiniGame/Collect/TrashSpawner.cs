using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    public static TrashSpawner Instance;


    public GameObject trashPrefab;

    public int totalTrash = 20;
    public int currentCore = 0;

    private int remainTrash;
    private int collectedTrash;


    private BoxCollider2D area;



    private void Awake()
    {
        Instance = this;

        area = GetComponent<BoxCollider2D>();
    }



        public void StartRound()
    {
        remainTrash = totalTrash;

        collectedTrash = 0;


        for(int i = 0; i < totalTrash; i++)
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


        Instantiate(
            trashPrefab,
            pos,
            Quaternion.identity
        );
    }




    public void TrashCollected()
    {
        
        remainTrash--;

        collectedTrash++;

        TrashCoreManager.Instance.TrashCollected();
        TrashMiniGameUI.Instance.UpdateTrash(
            collectedTrash,
            totalTrash
        );


        if(remainTrash <= 0 && currentCore <= 0)
        {
            TrashMiniGameManager.Instance.EndGame();
        }
    }

    public void SpawnCore(GameObject corePrefab)
    {
        Bounds bounds = area.bounds;

        Vector2 pos = new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );

        Instantiate(corePrefab, pos, Quaternion.identity);

        currentCore++;
    }
}