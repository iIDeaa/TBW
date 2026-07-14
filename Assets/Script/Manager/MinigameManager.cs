using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance;

    public Action<bool> OnMinigameEnd;
    private string returnScene;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void StartMinigame(string sceneName)
    {
        Debug.Log("FadeManager: " + FadeManager.Instance);

        if (FadeManager.Instance == null)
        {
            return;
        }

        returnScene = SceneManager.GetActiveScene().name;
        FadeManager.Instance.FadeToScene(sceneName);
    }

    public void EndMinigame(bool success)
    {
        StartCoroutine(ReturnRoutine(success));
    }

    IEnumerator ReturnRoutine(bool success)
    {
        bool done = false;

        FadeManager.Instance.Fade(() =>
        {
            SceneManager.LoadScene(returnScene);
            done = true;
        });

        // รอจนกว่าจะโหลดเสร็จ
        yield return new WaitUntil(() => done);

        OnMinigameEnd?.Invoke(success);
    }
}