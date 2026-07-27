using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCon : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject EscCanvas;
    public GameObject settingsPanel;
    public QuestUI questUI;
    // Start is called before the first frame update
    void Start()
    {
        menuCanvas.SetActive(false);
        EscCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);
            questUI.RefreshQuest();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel.activeSelf)
            {
                EscCon escCon = EscCanvas.GetComponent<EscCon>();
                if (escCon != null)
                {
                    escCon.CloseSettings();
                }
                else
                {
                    settingsPanel.SetActive(false);
                }
            }
            else
            {
                EscCanvas.SetActive(!EscCanvas.activeSelf);
            }
        }
    }
    public void CloseEsc()
    {
        EscCanvas.SetActive(false);
    }
}
