using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;

public class ReadyManager : MonoBehaviour
{
    public GameObject readyPanel;
    public TMP_Text countdownText;

    public UnityEvent onCountdownFinished;

    public void ShowReady()
    {
        readyPanel.SetActive(true);
    }

    public void OnReadyButton()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        readyPanel.SetActive(false);
        countdownText.gameObject.SetActive(true);

        countdownText.text = "3";
        yield return new WaitForSecondsRealtime(1);


        countdownText.text = "2";
        yield return new WaitForSecondsRealtime(1);


        countdownText.text = "1";
        yield return new WaitForSecondsRealtime(1);


        countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(1);


        countdownText.gameObject.SetActive(false);

        onCountdownFinished?.Invoke();
    }


}