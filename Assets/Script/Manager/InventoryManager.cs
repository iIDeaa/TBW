using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
     public InventoryUI inventoryUI;

    public List<InventorySlot> slots = new List<InventorySlot>();

    public int maxSlots = 20;

    public ItemData scrap;


    void Awake()
    {
        for (int i = 0; i < maxSlots; i++)
        {
            slots.Add(new InventorySlot());
        }
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Scrap value = " + scrap);

            AddItem(scrap);
        }
    }


    public void AddItem(ItemData item)
    {
    if(item == null)
    {
        Debug.LogError("ไม่มี ItemData ส่งเข้ามา");
        return;
    }

    Debug.Log("Add : " + item.itemName);


    foreach (InventorySlot slot in slots)
    {
        if(slot.item == item && item.stackable)
        {
            slot.amount++;
            inventoryUI.RefreshUI();
            return;
        }
    }


    foreach (InventorySlot slot in slots)
    {
        if(slot.item == null)
        {
            slot.item = item;
            slot.amount = 1;

            inventoryUI.RefreshUI();
            return;
        }
    }
    
    }


        
}
