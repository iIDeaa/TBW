using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMain : MonoBehaviour
{
    public GameObject settingspanel; 
    public GameObject settingSubPage;   
    public GameObject commandSubPage; 
  
    void Start()
    {
        settingspanel.SetActive(false);
        settingSubPage.SetActive(false);
    }
    public void OpenSettings()
    {
        settingspanel.SetActive(true);
        settingSubPage.SetActive(true);
    }

    public void CloseSettings()
    {
        settingspanel.SetActive(false);
        settingSubPage.SetActive(false);
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
