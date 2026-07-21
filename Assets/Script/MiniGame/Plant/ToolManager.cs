using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance;

    public ToolType CurrentTool;

    [Header("Tools")]
    public GameObject scissors;
    public GameObject hand;
    public GameObject shovel;
    public GameObject fertilizer;
    public GameObject wateringCan;
    public GameObject tree;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        HideAll();
    }

    void HideAll()
    {
        scissors.SetActive(false);
        hand.SetActive(false);
        shovel.SetActive(false);
        tree.SetActive(false);
        fertilizer.SetActive(false);
        wateringCan.SetActive(false);
    }
    public void HideCurrentTool()
    {
        HideAll();
        CurrentTool = ToolType.None;
    }
    public void SelectTool(ToolType tool)
    {
        CurrentTool = tool;

        HideAll();

        switch (tool)
        {
            case ToolType.Scissors:
                scissors.SetActive(true);
                break;

            case ToolType.Hand:
                hand.SetActive(true);
                break;

            case ToolType.Shovel:
                shovel.SetActive(true);
                break;

            case ToolType.Fertilizer:
                fertilizer.SetActive(true);
                break;

            case ToolType.WateringCan:
                wateringCan.SetActive(true);
                break;
            case ToolType.Tree:
                tree.SetActive(true);
                break;
        }
    }
}