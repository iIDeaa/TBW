using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;

    private Trash currentTrash;
    private CarbonCore currentCore; // 🔥 เพิ่ม

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (currentCore != null)
            {
                Debug.Log("กด E ที่ขยะ");
                currentCore.Interact();
            }
            else if (currentTrash != null)
            {
                Debug.Log("กด E ที่ Core");
                currentTrash.Interact(); 
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        Trash trash = other.GetComponent<Trash>();
        if (trash != null)
        {
            Debug.Log("เจอขยะ");
            currentTrash = trash;
            return;
        }

        CarbonCore core = other.GetComponent<CarbonCore>();
        if (core != null)
        {
            Debug.Log("เจอ Core");
            currentCore = core;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Trash trash = other.GetComponent<Trash>();
        if (trash != null && trash == currentTrash)
        {
            currentTrash = null;
        }

        CarbonCore core = other.GetComponent<CarbonCore>();
        if (core != null && core == currentCore)
        {
            currentCore = null;
        }
    }
}
