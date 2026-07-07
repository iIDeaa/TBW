using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSetting : MonoBehaviour
{
    Audio_Manager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio_Manager>();
    }
    
    public GameObject settingsPanel;
    // Start is called before the first frame update
    void Start()
    {
        settingsPanel.SetActive(false);
    }
    public void OpenSettings()
    {
        audioManager.PlaySFX(audioManager.Setting_but);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        audioManager.PlaySFX(audioManager.Setting_but);
        settingsPanel.SetActive(false);
    }
}
