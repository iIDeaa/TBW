using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscCon : MonoBehaviour
{
    public GameObject PauseMenu;     // เมนูหลัก (Continue / Setting / Exit)
    public GameObject settingsPanel; // เมนูตั้งค่า

    void Start()
    {
        PauseMenu.SetActive(false);
        settingsPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
    
            if (settingsPanel.activeSelf)
            {
                CloseSettings();
            }
            else if (PauseMenu.activeSelf)
            {
                PauseMenu.SetActive(false);
                Time.timeScale = 1f; 
            }
            else
            {
                PauseMenu.SetActive(true);
                Time.timeScale = 0f; 
            }
        }
    }


    public void OpenSettings()
    {
        PauseMenu.SetActive(false);
        settingsPanel.SetActive(true);
    }

    // ปิดหน้า Setting (กลับไป Pause)
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        PauseMenu.SetActive(true);
    }

    // ปุ่ม Continue
    public void OnContinueClick()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    // ปุ่ม Exit
    public void OnExitClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("(3)StartMenu");
    }
}