using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLoader : MonoBehaviour
{
    void Start()
    {
        FadeManager.Instance.FadeToScene("StartMenu");
    }
}