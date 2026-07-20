using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMain : MonoBehaviour
{
    public GameObject settingSubPage;   
    public GameObject commandSubPage; 

    public GameObject settingsPanel;
    void Start()
    {
        settingsPanel.SetActive(false);
    }
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void ShowSetting()
    {
        settingSubPage.SetActive(true);
        commandSubPage.SetActive(false);
    }

    public void ShowCommand()
    {
        settingSubPage.SetActive(false);
        commandSubPage.SetActive(true);
    }
}
