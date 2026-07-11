using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public InventoryManager inventory;

    public SlotUi slotPrefab;

    public Transform slotParent;


    private List<SlotUi> uiSlots = new List<SlotUi>();


    void Start()
    {
        Debug.Log("UI Start");


        for (int i = 0; i < inventory.maxSlots; i++)
        {
            SlotUi slot = Instantiate(slotPrefab, slotParent);

            uiSlots.Add(slot);
        }


        RefreshUI();
    }



    public void RefreshUI()
    {
        Debug.Log("Refresh UI");


        int count = Mathf.Min(
            inventory.slots.Count,
            uiSlots.Count
        );


        for (int i = 0; i < count; i++)
        {
            uiSlots[i].Set(inventory.slots[i]);
        }
    }
}
