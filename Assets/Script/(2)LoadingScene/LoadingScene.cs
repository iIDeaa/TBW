using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LoadingScene : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image loadingBarFill;        // ตัว bar ที่เลื่อน
    [SerializeField] private TextMeshProUGUI loadingText; // ข้อความ Loading
    [SerializeField] private TextMeshProUGUI titleText;   // ข้อความ Title (optional)

    [Header("Settings")]
    [SerializeField] private string sceneToLoad = "(3)StartMenu"; // ชื่อ Scene ที่จะโหลด
    [SerializeField] private float minimumLoadTime = 3f;       // เวลาขั้นต่ำ (วินาที) ปรับได้
    [SerializeField] private bool useFakeLoading = false;      // false = โหลด Scene จริง

    [Header("Animation")]
    [SerializeField] private float pulseSpeed = 2f;     // ความเร็วกะพริบข้อความ
    [SerializeField] private bool animateTitle = true;   // เปิด/ปิด animation title

    void Start()
    {
        if (useFakeLoading)
        {
            StartCoroutine(FakeLoadingRoutine());
        }
        else
        {
            StartCoroutine(LoadSceneAsync());
        }
    }

    void Update()
    {
        // Animation กะพริบ title text
        if (animateTitle && titleText != null)
        {
            float alpha = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            alpha = Mathf.Lerp(0.3f, 1f, alpha);
            Color c = titleText.color;
            c.a = alpha;
            titleText.color = c;
        }
    }

    /// <summary>
    /// โหลดจำลอง — ใช้เมื่อยังไม่มี Scene จริงที่จะโหลด
    /// </summary>
    IEnumerator FakeLoadingRoutine()
    {
        float elapsed = 0f;

        while (elapsed < minimumLoadTime)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / minimumLoadTime);
            progress = EaseInOutCubic(progress);

            UpdateUI(progress);
            yield return null;
        }

        UpdateUI(1f);
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Loading Complete! (Fake loading)");
    }

    /// <summary>
    /// โหลด Scene จริงแบบ Async พร้อมเวลาขั้นต่ำ
    /// </summary>
    IEnumerator LoadSceneAsync()
    {
        float elapsed = 0f;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            elapsed += Time.deltaTime;

            // คำนวณ progress จริง (Unity ไปถึง 0.9 แล้วรอ)
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // คำนวณ progress ตามเวลาขั้นต่ำ
            float timeProgress = Mathf.Clamp01(elapsed / minimumLoadTime);

            // ใช้ค่าที่น้อยกว่า → bar จะไม่เร็วกว่าเวลาที่กำหนด
            float displayProgress = Mathf.Min(realProgress, timeProgress);

            UpdateUI(displayProgress);

            // โหลดจริงเสร็จแล้ว + ครบเวลาขั้นต่ำแล้ว
            if (operation.progress >= 0.9f && elapsed >= minimumLoadTime)
            {
                UpdateUI(1f);
                yield return new WaitForSeconds(0.5f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    /// <summary>
    /// อัพเดท UI — bar และ text
    /// </summary>
    void UpdateUI(float progress)
    {
        if (loadingBarFill != null)
        {
            loadingBarFill.fillAmount = progress;
        }

        if (loadingText != null)
        {
            loadingText.text = "Loading...";
        }
    }

    /// <summary>
    /// Easing function — ทำให้การโหลดดูนุ่มนวล
    /// </summary>
    float EaseInOutCubic(float t)
    {
        return t < 0.5f
            ? 4f * t * t * t
            : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
    }
}