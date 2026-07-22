using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_manage : MonoBehaviour
{

    void Start()
    {
        if (FadeManager.HasInstance())
        {
            StartCoroutine(FadeManager.Instance.FadeInRoutine());
        }
    }
    
  public void OnStartClick()
    {
        FadeManager.Instance.FadeToScene("SampleScene");
        
        
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
