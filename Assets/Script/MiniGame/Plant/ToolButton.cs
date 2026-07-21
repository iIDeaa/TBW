using UnityEngine;

public class ToolButton : MonoBehaviour
{
    public ToolType tool;

    public void SelectTool()
    {
        ToolManager.Instance.SelectTool(tool);
    }
}