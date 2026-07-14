using UnityEngine;

public class SaveUI : MonoBehaviour
{
    public void OnClickSave()
    {
        SaveManager.Instance.SaveGame();
    }

    public void OnClickLoad()
    {
        SaveManager.Instance.LoadGame();
    }
}