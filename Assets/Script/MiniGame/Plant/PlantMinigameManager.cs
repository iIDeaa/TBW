using TMPro;
using UnityEngine;

public class PlantMinigameManager : MonoBehaviour
{
    public static PlantMinigameManager Instance;
    public PlantResultUI resultUI;

    [Header("Game")]
    public PlantStep currentStep;

    [Header("Object Count")]
    public int grassRemain;
    public int rockRemain;

    [Header("Timer")]
    public float elapsedTime;
    bool isPlaying;

    [Header("UI")]
    public TMP_Text timerText;
    public TMP_Text stepText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentStep = PlantStep.CutGrass;

        UpdateStepUI();

        elapsedTime = 0;

        isPlaying = true;
    }

    private void Update()
    {
        if (!isPlaying)
            return;

        elapsedTime += Time.deltaTime;

        timerText.text = elapsedTime.ToString("F1") + " s";
    }

    public void GrassCut()
    {
        grassRemain--;

        if (grassRemain <= 0)
        {
            currentStep = PlantStep.RemoveRock;
            UpdateStepUI();
        }
    }

    public void RockRemoved()
    {
        rockRemain--;

        if (rockRemain <= 0)
        {
            currentStep = PlantStep.DigSoil;
            UpdateStepUI();
        }
    }

    public void DigComplete()
    {
        currentStep = PlantStep.PlantTree;
        UpdateStepUI();
    }

    public void TreePlaced()
    {
        currentStep = PlantStep.Fertilize;
        UpdateStepUI();
    }

    public void FertilizeComplete()
    {
        currentStep = PlantStep.Water;
        UpdateStepUI();
    }

    public void WaterComplete()
    {
        currentStep = PlantStep.Finish;

        UpdateStepUI();

        EndGame();
    }

    void UpdateStepUI()
    {
        switch (currentStep)
        {
            case PlantStep.CutGrass:
                stepText.text = "Cut the grass";
                break;

            case PlantStep.RemoveRock:
                stepText.text = "Remove the rocks";
                break;

            case PlantStep.DigSoil:
                stepText.text = "Dig the soil";
                break;

            case PlantStep.PlantTree:
                stepText.text = "Plant the tree";
                break;

            case PlantStep.Fertilize:
                stepText.text = "Add fertilizer";
                break;

            case PlantStep.Water:
                stepText.text = "Water the tree";
                break;

            case PlantStep.Finish:
                stepText.text = "Completed";
                break;
        }
    }

    void EndGame()
    {
        isPlaying = false;

        resultUI.ShowResult(elapsedTime);
    }
}