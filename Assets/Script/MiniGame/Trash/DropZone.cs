using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public int targetID;

    public void OnDrop(PointerEventData eventData)
    {
        DragItem item = eventData.pointerDrag.GetComponent<DragItem>();

        if (item == null) return;

        if (item.itemID == targetID)
        {
            
            TrashManager.Instance.AddScore(1);
            Destroy(item.gameObject);
            TrashManager.Instance.ItemFinished();

            Debug.Log("Correct!");
        }
        else
        {
            TrashManager.Instance.MinusScore(1);
            Destroy(item.gameObject);
            TrashManager.Instance.ItemFinished();

            Debug.Log("Wrong!");
        }
    }
}