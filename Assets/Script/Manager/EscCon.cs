using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscCon : MonoBehaviour
{
    public GameObject settingsPanel;

    private GameObject continueButton;
    private GameObject settingButton;
    private GameObject exitButton;

    void Awake()
    {
        continueButton = transform.Find("Continue_but")?.gameObject;
        settingButton = transform.Find("Setting_but")?.gameObject;
        exitButton = transform.Find("Exit_but")?.gameObject;
    }

    void OnEnable()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        SetButtonsActive(true);
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
        SetButtonsActive(true);
    }

    private void SetButtonsActive(bool active)
    {
        if (continueButton != null) continueButton.SetActive(active);
        if (settingButton != null) settingButton.SetActive(active);
        if (exitButton != null) exitButton.SetActive(active);
    }

    public void OnExitClick()
    {
        SceneManager.LoadScene("(3)StartMenu");
    }
}
