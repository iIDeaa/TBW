using TMPro;
using UnityEngine;


public class TrashMinigameResultUI : MonoBehaviour
{
    public GameObject panel;


    public TMP_Text timeText;

    public TMP_Text correctText;

    public TMP_Text wrongText;



    private void Awake()
    {
        panel.SetActive(false);
    }



    public void ShowResult(
        float time,
        int correct,
        int wrong
    )
    {
        panel.SetActive(true);


        timeText.text =
            $"Time : {time:F1} sec";


        correctText.text =
            $"Correct : {correct}";


        wrongText.text =
            $"Wrong : {wrong}";
    }



    public void ContinueGame()
    {
        panel.SetActive(false);


        TrashMiniGameManager.Instance.Continue();
    }
}