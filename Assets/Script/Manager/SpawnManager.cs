using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    public string ReturnScene { get; private set; }
    public Vector3 ReturnPosition { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveSpawn(string sceneName, Vector3 position)
    {
        ReturnScene = sceneName;
        ReturnPosition = position;
    }
}