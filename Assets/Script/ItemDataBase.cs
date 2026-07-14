using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;

    public List<ItemData> allItems;

    void Awake()
    {
        Instance = this;
    }

    public ItemData GetItemByID(string id)
    {
        return allItems.Find(x => x.itemId == id);
    }
}