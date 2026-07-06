using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CutsceneManager : MonoBehaviour
{
    [Header("Cutscene Settings")]
    [Tooltip("PlayableDirector ของ Cutscene ปัจจุบัน")]
    public PlayableDirector currentDirector;

    [Header("Next Cutscene")]
    [Tooltip("เลือกว่าจะไป Cutscene ถัดไปแบบไหน")]
    public NextCutsceneMode nextMode = NextCutsceneMode.PlayNextTimeline;

    [Tooltip("Timeline ถัดไปที่จะเล่น (ใช้เมื่อ mode = PlayNextTimeline)")]
    public PlayableAsset nextTimeline;

#if UNITY_EDITOR
    [Tooltip("ลาก Scene ที่จะไปต่อมาวางตรงนี้")]
    public SceneAsset nextSceneAsset;
#endif

    [HideInInspector]
    public string nextSceneName;

    [Header("Transition")]
    [Tooltip("หน่วงเวลาก่อนไป Cutscene ถัดไป (วินาที)")]
    public float delayBeforeNext = 0f;

    public enum NextCutsceneMode
    {
        PlayNextTimeline,  // เล่น Timeline ถัดไปใน Scene เดิม
        LoadScene          // โหลด Scene ใหม่
    }

#if UNITY_EDITOR
    /// <summary>
    /// อัปเดตชื่อ Scene อัตโนมัติเมื่อลากไฟล์มาวาง
    /// </summary>
    private void OnValidate()
    {
        if (nextSceneAsset != null)
        {
            nextSceneName = nextSceneAsset.name;
        }
        else
        {
            nextSceneName = "";
        }
    }
#endif

    private void OnEnable()
    {
        if (currentDirector != null)
        {
            // ลงทะเบียน event เมื่อ Timeline เล่นจบ
            currentDirector.stopped += OnCutsceneFinished;
        }
    }

    private void OnDisable()
    {
        if (currentDirector != null)
        {
            currentDirector.stopped -= OnCutsceneFinished;
        }
    }

    /// <summary>
    /// ถูกเรียกอัตโนมัติเมื่อ Cutscene เล่นจบ
    /// </summary>
    private void OnCutsceneFinished(PlayableDirector director)
    {
        if (delayBeforeNext > 0f)
        {
            StartCoroutine(DelayedNextCutscene());
        }
        else
        {
            GoToNextCutscene();
        }
    }

    private IEnumerator DelayedNextCutscene()
    {
        yield return new WaitForSeconds(delayBeforeNext);
        GoToNextCutscene();
    }

    private void GoToNextCutscene()
    {
        switch (nextMode)
        {
            case NextCutsceneMode.PlayNextTimeline:
                PlayNextTimeline();
                break;

            case NextCutsceneMode.LoadScene:
                LoadNextScene();
                break;
        }
    }

    /// <summary>
    /// เปลี่ยน Timeline ของ PlayableDirector แล้วเล่นต่อ
    /// </summary>
    private void PlayNextTimeline()
    {
        if (nextTimeline != null && currentDirector != null)
        {
            currentDirector.playableAsset = nextTimeline;
            currentDirector.Play();
            Debug.Log($"[CutsceneManager] Playing next timeline: {nextTimeline.name}");
        }
        else
        {
            Debug.LogWarning("[CutsceneManager] nextTimeline or currentDirector is null!");
        }
    }

    /// <summary>
    /// โหลด Scene ใหม่
    /// </summary>
    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Debug.Log($"[CutsceneManager] Loading scene: {nextSceneName}");
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("[CutsceneManager] nextSceneName is empty!");
        }
    }
}
