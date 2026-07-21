using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtons : MonoBehaviour
{
    public void Save()
    {
        SaveManager.Instance.SaveGame();
    }

    public void Load()
    {
        SaveManager.Instance.LoadGame();
    }
}
