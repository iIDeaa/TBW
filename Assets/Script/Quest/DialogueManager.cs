using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static event System.Action OnDialogueFinished;
    public static DialogueManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Show(DialogueData dialogue)
{
    

    Debug.Log("Show Dialogue"); 

    foreach (var line in dialogue.lines)
    {
        Debug.Log(line.speaker + ": " + line.text);
    }

    OnDialogueFinished?.Invoke();
}
}