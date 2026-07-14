using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    
    public List<InventorySlot> slots = new List<InventorySlot>();
    public int maxSlots = 20;
    

    // 🔥 Event
    public static event Action<ItemData, int> OnItemAdded;
    
    public static event System.Action OnItemRemoved;
    public bool isLoading = false;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 🔥 สำคัญ
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (slots.Count == 0)
        {
            for (int i = 0; i < maxSlots; i++)
            {
                slots.Add(new InventorySlot());
            }
        }
        
    }
    public void AddItem(ItemData item, int amount)
    {
        if (item == null)
        {
            Debug.LogError("ไม่มี ItemData ส่งเข้ามา");
            return;
        }

        Debug.Log("Add : " + item.itemName + " x" + amount);

        // 🔥 stack
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == item && item.stackable)
            {
                slot.amount += amount;
                if (!isLoading)
                OnItemAdded?.Invoke(item, amount);
                return;
            }
        }

        // 🔥 new slot
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == null)
            {
                slot.item = item;
                slot.amount = amount;
                if (!isLoading)
                OnItemAdded?.Invoke(item, amount);
                return;
            }
        }
    }
    public void AddItem(ItemData item)
    {
        AddItem(item, 1);
    }


    public bool RemoveItem(ItemData item, int amount)
    {
        int remaining = amount;

        foreach (var slot in slots)
        {
            if (slot.item == item)
            {
                if (slot.amount >= remaining)
                {
                    slot.amount -= remaining;
                    

                    if (slot.amount == 0)
                        slot.item = null;

                    OnItemRemoved?.Invoke();
                    return true;
                }
                else
                {
                    remaining -= slot.amount;
                    slot.item = null;
                    slot.amount = 0;

                    OnItemRemoved?.Invoke();
                }
            }
        }

        return false; // ของไม่พอ
    }
    public void ClearInventory()
    {
        foreach (var slot in slots)
        {
            slot.item = null;
            slot.amount = 0;
        }

        if (!isLoading)
        OnItemRemoved?.Invoke();

        Debug.Log("Inventory Cleared");
    }


    public bool HasItem(ItemData item, int amount)
    {
        int count = 0;

        foreach (var slot in slots)
        {
            if (slot.item == item)
            {
                count += slot.amount;
            }
        }

        return count >= amount;
    }
    public void ForceRefreshUI()
    {
        OnItemRemoved?.Invoke();
    }
}
