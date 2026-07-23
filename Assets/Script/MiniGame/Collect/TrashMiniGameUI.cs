using TMPro;
using UnityEngine;

public class TrashMiniGameUI : MonoBehaviour
{
    public static TrashMiniGameUI Instance;


    public TMP_Text hitText;      // 0/3
    public TMP_Text trashText;    // 0/20



    private void Awake()
    {
        Instance = this;
    }



    public void UpdateHit(int current)
    {
        hitText.text = current + "/3";
    }



    public void UpdateTrash(int current, int total)
    {
        trashText.text = current + "/" + total;
    }
}