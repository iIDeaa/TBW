using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCon : MonoBehaviour
{
    public GameObject EscPanel;
    public GameObject menuCanvas;
    public GameObject settingsPanel;
    public QuestUI questUI;
    // Start is called before the first frame update
    void Start()
    {
        menuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(EscPanel.activeSelf){
                return;
            }
            menuCanvas.SetActive(!menuCanvas.activeSelf);
            questUI.RefreshQuest();
        }

    }
}
