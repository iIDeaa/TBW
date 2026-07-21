using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class NextLevel : MonoBehaviour
{
#if UNITY_EDITOR
    [Tooltip("ลาก Scene ที่จะไปมาวางตรงนี้")]
    public SceneAsset nextSceneAsset;
#endif

    [HideInInspector]
    public string nextLevelName;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (nextSceneAsset != null)
        {
            nextLevelName = nextSceneAsset.name;
        }
        else
        {
            nextLevelName = "";
        }
    }
#endif

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.gameObject.name == "Player")
        {
            SceneManager.LoadScene(nextLevelName);
        }
    }
}
