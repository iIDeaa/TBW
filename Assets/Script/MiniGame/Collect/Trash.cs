using UnityEngine;

public class Trash : MonoBehaviour
{
    private bool collected;

    public void Interact()
    {
        if (collected)
            return;

        TrashMiniGameManager.Instance.StartTrashGame(this);
    }

    public void Collect()
    {
        if (collected)
            return;

        collected = true;

        TrashCoreManager.Instance.TrashCollected();

        Destroy(gameObject);
    }
}