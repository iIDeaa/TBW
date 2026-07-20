using UnityEngine;
using UnityEngine.SceneManagement; 

public class StartMenuManager : MonoBehaviour
{
    [Header("Scene Settings")]
    public string gameSceneName = "SampleScene"; 

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenSetting()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(true);
        }
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}