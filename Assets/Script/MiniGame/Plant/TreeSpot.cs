using UnityEngine;

public class TreeSpot : MonoBehaviour
{
    public GameObject treePrefab;

    bool planted;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (planted)
            return;

        if (PlantMinigameManager.Instance.currentStep != PlantStep.PlantTree)
            return;

        if (ToolManager.Instance.CurrentTool != ToolType.Tree)
            return;

        if (!Input.GetMouseButtonDown(0))
            return;

        planted = true;

        Instantiate(
            treePrefab,
            transform.position,
            Quaternion.identity);

        PlantMinigameManager.Instance.TreePlaced();
    }
}