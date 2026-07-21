using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_manage : MonoBehaviour
{
<<<<<<< HEAD:Assets/Script/Manager/Scene_manage.cs
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
        
        
=======
    Audio_Manager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio_Manager>();
    }

    public void OnStartClick()
    {
        SceneManager.LoadScene("(3)World");
>>>>>>> Cxk21_07:Assets/Script/Scene_manage.cs
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
