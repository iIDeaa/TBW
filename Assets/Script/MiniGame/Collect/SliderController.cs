using UnityEngine;

public class SliderController : MonoBehaviour
{
    public RectTransform pointer;
    public RectTransform greenZone;
    public RectTransform bar;

    public float speed = 600;

    private float direction = 1;
    private int success;

    private float left;
    private float right;

    private bool playing;

    private void Start()
    {
        left = -bar.rect.width / 2;
        right = bar.rect.width / 2;

        gameObject.SetActive(false);
    }

    public void StartGame()
    {
        gameObject.SetActive(true);

        success = 0;
        playing = true;

        pointer.anchoredPosition = new Vector2(left, 0);

        RandomGreenZone();

        TrashMiniGameUI.Instance.UpdateHit(0);
    }

    private void Update()
    {
        if (!playing) return;

        Move();

        var manager = TrashMiniGameManager.Instance;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Check();
        }
    }

    void Move()
    {
        Vector2 pos = pointer.anchoredPosition;

        pos.x += direction * speed * Time.deltaTime;

        if (pos.x >= right)
        {
            pos.x = right;
            direction = -1;
        }

        if (pos.x <= left)
        {
            pos.x = left;
            direction = 1;
        }

        pointer.anchoredPosition = pos;
    }

    void Check()
    {
        float x = pointer.anchoredPosition.x;

        float min = greenZone.anchoredPosition.x - greenZone.rect.width / 2;
        float max = greenZone.anchoredPosition.x + greenZone.rect.width / 2;

        if (x >= min && x <= max)
        {
            Debug.Log("✔ HIT");

            // 🔥 ถ้าเป็น Core
            if (TrashMiniGameManager.Instance.currentCore != null)
            {
                TrashMiniGameManager.Instance.currentCore.TakeSliderDamage();

                // ❗ Core ห้ามสะสม success
                success = 0;
                TrashMiniGameUI.Instance.UpdateHit(0);

                RandomGreenZone();
                return;
            }
            success++;

            Debug.Log("✔ HIT " + success);

            TrashMiniGameUI.Instance.UpdateHit(success);

            if (success >= 3)
            {
                playing = false;
                gameObject.SetActive(false);

                TrashMiniGameManager.Instance.FinishTrash();
                return;
            }
        }
        else
        {
            Debug.Log("❌ MISS");

            success = 0;
            TrashMiniGameUI.Instance.UpdateHit(0);
        }

        RandomGreenZone();
    }

    void RandomGreenZone()
    {
        float x = Random.Range(
            left + greenZone.rect.width / 2,
            right - greenZone.rect.width / 2
        );

        greenZone.anchoredPosition = new Vector2(x, 0);
    }
    public bool IsInGreenZone()
    {
        float x = pointer.anchoredPosition.x;

        float min = greenZone.anchoredPosition.x - greenZone.rect.width / 2;
        float max = greenZone.anchoredPosition.x + greenZone.rect.width / 2;

        return (x >= min && x <= max);
    }
}