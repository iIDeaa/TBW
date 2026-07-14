using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAppear : MonoBehaviour
{
    [Header("UI")]
    public GameObject prompt; 

    [Header("Outline")]
    public Behaviour outline; 

    [Header("Inventory")]
    public InventoryManager inventory;
    public ItemData itemData;


    private bool playerInRange = false;


    void Start()
    {
        
        if (prompt != null)
            prompt.SetActive(false);

        if (outline != null)
            outline.enabled = false;
    }


    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;


            
            if (prompt != null)
                prompt.SetActive(true);


            
            if (outline != null)
                outline.enabled = true;
        }
    }



    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;


            
            if (prompt != null)
                prompt.SetActive(false);


            
            if (outline != null)
                outline.enabled = false;
        }
    }



    void PickUp()
    {
        
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(itemData);
        }
        else
        {
            Debug.LogError("InventoryManager not found!");
            return;
        }


        
        if (prompt != null)
            prompt.SetActive(false);


        
        Destroy(gameObject);
    }
}
