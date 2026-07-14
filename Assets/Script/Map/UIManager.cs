using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Main Panel")]
    [SerializeField] private GameObject zoneSelectPanel;

    [Header("Zone Panels")]
    [SerializeField] private List<GameObject> zonePanels = new List<GameObject>();

    private void Start()
    {
        ShowZoneSelect();
    }

    public void ShowZoneSelect()
    {
        zoneSelectPanel.SetActive(true);

        foreach (GameObject panel in zonePanels)
        {
            panel.SetActive(false);
        }
    }

    public void OpenZone(int index)
    {
        if (index < 0 || index >= zonePanels.Count)
        {
            Debug.LogWarning($"Zone Index {index} doesn't exist.");
            return;
        }

        // เช็ค Unlock ก่อน
        UnlockByQuest unlock = zonePanels[index].GetComponent<UnlockByQuest>();

        if (unlock != null && !unlock.IsUnlockedPublic())
        {
            Debug.Log("Zone ยังไม่ปลดล็อก");
            return;
        }

        zoneSelectPanel.SetActive(false);

        foreach (GameObject panel in zonePanels)
        {
            panel.SetActive(false);
        }

        zonePanels[index].SetActive(true);
    }
}