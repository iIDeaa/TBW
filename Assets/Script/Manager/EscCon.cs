using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscCon : MonoBehaviour
{
    public GameObject Menu;
    public GameObject EscPanel;
    public GameObject settingsPanel;
    public GameObject CommandListPage;
    
    private GameObject continueButton;
    private GameObject settingButton;
    private GameObject exitButton;

    void Start()
    {
        EscPanel.SetActive(false);

        continueButton = EscPanel.transform.Find("Continue_but")?.gameObject;
        settingButton = EscPanel.transform.Find("Setting_but")?.gameObject;
        exitButton = EscPanel.transform.Find("Exit_but")?.gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Menu != null && Menu.activeSelf)
                return;

            if (CommandListPage.activeSelf)
            {
                CommandListPage.SetActive(false);
                SetButtonsActive(true);
                return;
            }
            if (IsSettingsOpen())
            {
                CloseSettings();
                return;
            }

            bool isESCActive = EscPanel.activeSelf;

            EscPanel.SetActive(!isESCActive);
            SetButtonsActive(!isESCActive);

            Time.timeScale = isESCActive ? 1f : 0f;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if ((Menu != null && Menu.activeSelf))
                return;

            EscPanel.SetActive(true);

            SetButtonsActive(false);

            if (settingsPanel != null)
                settingsPanel.SetActive(false);

            CommandListPage.SetActive(true);

            Time.timeScale = 0f;
        }
    }
    void OnEnable()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        SetButtonsActive(true);
    }
    

    public bool IsSettingsOpen()
    {
        return settingsPanel != null && settingsPanel.activeSelf;
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
        SetButtonsActive(false);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        EscPanel.SetActive(true);
        SetButtonsActive(true);
    }

    private void SetButtonsActive(bool active)
    {
        if (continueButton != null) continueButton.SetActive(active);
        if (settingButton != null) settingButton.SetActive(active);
        if (exitButton != null) exitButton.SetActive(active);
    }

    public void CloseEsc()
    {
        EscPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnExitClick()
    {
        SceneManager.LoadScene("(3)StartMenu");
    }
}
