using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Show(DialogueData dialogue)
    {
        Debug.Log("Show Dialogue"); // 👈 เพิ่ม
        foreach (var line in dialogue.lines)
        {
            Debug.Log(line.speaker + ": " + line.text);
        }
    }
}