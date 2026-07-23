using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    [Header("UI")]
    public Image fadeImage;

    [Header("Setting")]
    public float fadeDuration = 20f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (fadeImage != null)
            fadeImage.color = new Color(0, 0, 0, 1);
    }

    
    //  CORE FADE

    public IEnumerator FadeOutRoutine(float duration = -1f)
    {
        if (fadeImage == null) yield break;

        float d = (duration > 0) ? duration : fadeDuration;
        float t = 0;

        while (t < d)
        {
            t += Time.deltaTime;
            float alpha = t / d;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 1);
    }

    public IEnumerator FadeInRoutine(float duration = -1f)
    {
        if (fadeImage == null) yield break;

        float d = (duration > 0) ? duration : fadeDuration;
        float t = d;

        while (t > 0)
        {
            t -= Time.deltaTime;
            float alpha = t / d;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 0);
    }

    public void Fade(Action onBlack, float duration = -1f)
    {
        StartCoroutine(FadeActionRoutine(onBlack, duration));
    }

    IEnumerator FadeActionRoutine(Action onBlack, float duration)
    {
        yield return FadeOutRoutine(duration);

        onBlack?.Invoke(); // 🔥 ทำตอนจอดำ

        yield return FadeInRoutine(duration);
    }

    // 🎬 FADE + SCENE

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeSceneRoutine(sceneName));
    }

    IEnumerator FadeSceneRoutine(string sceneName)
    {
        // 🔥 1. Fade Out (จอดำ)
        yield return FadeOutRoutine();

        // 🔥 2. โหลดฉาก
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        while (!op.isDone)
        {
            yield return null;
        }

        // 🔥 3. Fade In (กลับมาเห็น)
        yield return FadeInRoutine();
    }

    public static bool HasInstance()
    {
        return Instance != null;
    }
}