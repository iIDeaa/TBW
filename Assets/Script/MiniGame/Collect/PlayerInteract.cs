using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;

    private Trash currentTrash;


    private void Update()
    {
        if(currentTrash != null && Input.GetKeyDown(interactKey))
        {
            Debug.Log("กด E");
            currentTrash.Interact();
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
{
    Trash trash = other.GetComponent<Trash>();

    if(trash != null)
    {
        Debug.Log("เจอขยะ");
        currentTrash = trash;
    }
}



    private void OnTriggerExit2D(Collider2D other)
    {
        Trash trash = other.GetComponent<Trash>();

        if(trash != null && trash == currentTrash)
        {
            currentTrash = null;
        }
    }
}