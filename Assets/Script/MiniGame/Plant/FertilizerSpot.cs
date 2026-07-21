using UnityEngine;

public class FertilizerSpot : MonoBehaviour
{
    bool finished;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (finished)
            return;

        if (PlantMinigameManager.Instance.currentStep != PlantStep.Fertilize)
            return;

        if (ToolManager.Instance.CurrentTool != ToolType.Fertilizer)
            return;

        if (!Input.GetMouseButtonDown(0))
            return;


        finished = true;

        other.gameObject.SetActive(false);

        PlantMinigameManager.Instance.FertilizeComplete();
    }
}