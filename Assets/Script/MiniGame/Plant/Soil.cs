using UnityEngine;
using UnityEngine.UI;

public class Soil : MonoBehaviour
{
    public GameObject groundSprite;
    public GameObject holeSprite;

    public Image progressBar;

    public float digTime = 2f;

    float timer;

    bool completed;

    private void Start()
    {
        holeSprite.SetActive(false);
        progressBar.fillAmount = 0;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (completed)
            return;

        if (PlantMinigameManager.Instance.currentStep != PlantStep.DigSoil)
            return;

        if (ToolManager.Instance.CurrentTool != ToolType.Shovel)
            return;

        if (!Input.GetMouseButton(0))
            return;

        timer += Time.deltaTime;

        progressBar.fillAmount = timer / digTime;

        if (timer >= digTime)
        {
            completed = true;

            groundSprite.SetActive(false);
            holeSprite.SetActive(true);

            progressBar.gameObject.SetActive(false);

            PlantMinigameManager.Instance.DigComplete();
        }
    }
}