using TMPro;
using UnityEngine;

public class PlantResultUI : MonoBehaviour
{
    public GameObject panel;

    public TMP_Text timeText;

    private void Awake()
    {
        panel.SetActive(false);
    }

    public void ShowResult(float time)
    {
        panel.SetActive(true);

        timeText.text = $"Time : {time:F1} sec";
    }

    public void ContinueGame()
    {
        panel.SetActive(false);

        MinigameManager.Instance.EndMinigame(true);
    }
}