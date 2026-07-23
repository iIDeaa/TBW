using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_manage : MonoBehaviour
{

    // void Start()
    // {
    //     if (FadeManager.HasInstance())
    //     {
    //         StartCoroutine(FadeManager.Instance.FadeInRoutine());
    //     }
    // }
    
//   public void OnStartClick()
//     {
//         FadeManager.Instance.FadeToScene("SampleScene");
        
        
//     }
    public void OnStartClick()
    {
        if (QuestManager.Instance != null) QuestManager.Instance.ResetAll(); 
        if (InventoryManager.Instance != null) InventoryManager.Instance.ClearInventory();
        if (ZoneDataManager.Instance != null) ZoneDataManager.Instance.ResetAll();
        SceneManager.LoadScene("(4)CutScene_World");
    }
    public void OnContinueClick()
    {
        if (SaveManager.Instance != null)
        {
            if (SaveManager.Instance.HasSave())
            {
                SaveManager.Instance.LoadGame();
            }
            else
            {
                Debug.Log("No save file");
            }
        }
        else
        {
            Debug.LogWarning("SaveManager.Instance is null!");
        }
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
