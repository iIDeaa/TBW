using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SettingMenu : MonoBehaviour
{

    public GameObject settingSubPage;   
    public GameObject commandSubPage;  


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

