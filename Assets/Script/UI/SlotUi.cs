using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUi : MonoBehaviour
{
    public Image icon;
    public TMP_Text amountText;
    public TMP_Text itemNameText;


    public void Set(InventorySlot slot)
    {
        if(slot.item == null)
        {
            icon.enabled = false;
            amountText.text = "";
            itemNameText.text = "";
            return;
        }


        icon.enabled = true;

        icon.sprite = slot.item.icon;


        amountText.text = slot.amount.ToString();


        itemNameText.text = slot.item.itemName;
    }
    
}
