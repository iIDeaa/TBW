using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public InventoryManager inventory;

    public SlotUi slotPrefab;

    public Transform slotParent;


    private List<SlotUi> uiSlots = new List<SlotUi>();

    void Awake()
    {
        if (inventory == null)
            inventory = InventoryManager.Instance;
            for (int i = 0; i < inventory.maxSlots; i++)
        {
            SlotUi slot = Instantiate(slotPrefab, slotParent);
            uiSlots.Add(slot);
        }

        RefreshUI();
    }
    


    public void RefreshUI()
    {
        if (inventory == null || inventory.slots == null)
        {
            Debug.LogWarning("Inventory not ready");
            return;
        }
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
    void OnEnable()
    {
        InventoryManager.OnItemAdded += OnItemChanged;
        InventoryManager.OnItemRemoved += RefreshUI;

        RefreshUI();
    }

    void OnDisable()
    {
        InventoryManager.OnItemAdded -= OnItemChanged;
        InventoryManager.OnItemRemoved -= RefreshUI;
    }

    void OnItemChanged(ItemData item, int amount)
    {
        RefreshUI();
    }
}
