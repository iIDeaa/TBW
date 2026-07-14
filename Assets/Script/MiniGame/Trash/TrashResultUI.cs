using TMPro;
using UnityEngine;

public class TrashResultUI : MonoBehaviour
{
    public GameObject panel;
    void Awake()
    {
        panel.SetActive(false);
    }

    public TMP_Text timeText;
    public TMP_Text correctText;
    public TMP_Text wrongText;
    public TMP_Text rewardText;
    
    public void ShowResult(float time, int correct, int wrong, int reward)
    {
        panel.SetActive(true);

        timeText.text = $"Time  : {time:F1} sec";
        correctText.text = $"Correct  : {correct}";
        wrongText.text = $"Wrong  : {wrong}";
        rewardText.text = $"Reward  : Scrap  {reward}x";
    }
    public void ContinueGame()
    {
        panel.SetActive(false);
        MinigameManager.Instance.EndMinigame(true);
    }
}