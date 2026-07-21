using UnityEngine;
using UnityEngine.UI;

public class WaterSpot : MonoBehaviour
{
    public Image progressBar;

    public GameObject wateringCan;

    public float waterTime = 2f;

    float timer;

    bool finished;

    private void Start()
    {
        progressBar.fillAmount = 0;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (finished)
            return;

        if (PlantMinigameManager.Instance.currentStep != PlantStep.Water)
            return;

        if (ToolManager.Instance.CurrentTool != ToolType.WateringCan)
            return;

        if (!Input.GetMouseButton(0))
            return;


        timer += Time.deltaTime;

        progressBar.fillAmount = timer / waterTime;


        if (timer >= waterTime)
        {
            finished = true;

            progressBar.gameObject.SetActive(false);


            // ทำให้บัวรดน้ำหาย
            if (wateringCan != null)
            {
                ToolManager.Instance.HideCurrentTool();
            }


            PlantMinigameManager.Instance.WaterComplete();
        }
    }
}