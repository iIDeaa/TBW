using UnityEngine;

public class Trash : MonoBehaviour
{
    public bool isCollected = false;

    public void Interact()
    {
        if (isCollected)
            return;

        TrashMiniGameManager.Instance.StartTrashGame(this);
    }

    public void Collect()
    {
        if (isCollected)
            return;

        isCollected = true;

        Destroy(gameObject);

        TrashCoreManager.Instance.AddTrashChance();
        TrashCoreManager.Instance.CheckEndGameSafe();
    }

}