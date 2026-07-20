using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MainSetting : MonoBehaviour
{
    public GameObject mainSettingWindow; 

    public GameObject settingSubPage;   
    public GameObject commandSubPage;  

    void Start()
    {
        mainSettingWindow.SetActive(false);
    }

    public void OpenSettings()
    {
        mainSettingWindow.SetActive(true);
        ShowSetting(); 
    }

    public void CloseSettings()
    {
        mainSettingWindow.SetActive(false);
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

