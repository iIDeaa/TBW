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
        if (FadeManager.Instance == null)
            return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            SpawnManager.Instance.SaveSpawn(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                player.transform.position
            );
        }

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
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(SpawnManager.Instance.ReturnScene);

            done = true;
        });

        yield return new WaitUntil(() => done);

        OnMinigameEnd?.Invoke(success);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            player.transform.position = SpawnManager.Instance.ReturnPosition;
        }
    }
}