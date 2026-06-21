using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSetting : MonoBehaviour
{
    public GameObject settingsPanel;
    // Start is called before the first frame update
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
}
