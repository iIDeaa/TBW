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
        if (commandSubPage != null) commandSubPage.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleCommandList();
        }
    }

    public void ToggleCommandList()
    {
        if (settingspanel == null || commandSubPage == null) return;

        // ถ้าเปิดหน้า CommandList อยู่แล้ว ให้ปิด
        if (settingspanel.activeSelf && commandSubPage.activeSelf)
        {
            CloseSettings();
        }
        else // ถ้ายังไม่เปิด หรือเปิดหน้าอื่นอยู่ ให้สลับมาหน้า CommandList
        {
            settingspanel.SetActive(true);
            if (settingSubPage != null) settingSubPage.SetActive(false);
            commandSubPage.SetActive(true);
        }
    }

    public void OpenSettings()
    {
        settingspanel.SetActive(true);
        settingSubPage.SetActive(true);
        if (commandSubPage != null) commandSubPage.SetActive(false);
    }

    public void CloseSettings()
    {
        settingspanel.SetActive(false);
        settingSubPage.SetActive(false);
        if (commandSubPage != null) commandSubPage.SetActive(false);
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
