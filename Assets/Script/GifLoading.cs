using UnityEngine;
using UnityEngine.UI;

public class GifPlayer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Sprite[] frames;         // ลากเฟรมทั้งหมดมาวาง
    [SerializeField] private float framesPerSecond = 12f; // ความเร็ว animation
    [SerializeField] private bool loop = true;         // วนซ้ำ

    private Image image;
    private int currentFrame = 0;
    private float timer = 0f;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (frames.Length == 0) return;

        timer += Time.deltaTime;
        float interval = 1f / framesPerSecond;

        if (timer >= interval)
        {
            timer -= interval;
            currentFrame++;

            if (currentFrame >= frames.Length)
            {
                if (loop)
                    currentFrame = 0;
                else
                    currentFrame = frames.Length - 1;
            }

            image.sprite = frames[currentFrame];
        }
    }
}