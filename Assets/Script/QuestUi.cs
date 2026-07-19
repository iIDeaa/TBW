using UnityEngine;
using TMPro;

public class QuestNotificationSystem : MonoBehaviour
{
    // ตัวแปรส่วนกลาง (Singleton) ทำให้สคริปต์อื่นสามารถเรียกใช้ได้จากทุกที่
    public static QuestNotificationSystem Instance { get; private set; }

    [Header("UI Objects")]
    [SerializeField] private TextMeshProUGUI questNameText;
    [SerializeField] private Animator panelAnimator;

    private void Awake()
    {
        // 1. ตรวจสอบและเปิดระบบไม่ให้ถูกทำลายเมื่อเปลี่ยนซีน (DontDestroyOnLoad)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ยึด Canvas นี้ไว้ไม่ให้หายไปไหน
        }
        else
        {
            Destroy(gameObject); // หากมีตัวซ้ำในซีนใหม่ ให้ทำลายทิ้งเพื่อไม่ให้ทับกัน
            return;
        }
    }

    // 2. ฟังก์ชันที่เราจะให้สคริปต์เควสอื่นๆ เรียกใช้
    public void TriggerNotification(string questName)
    {
        // เปลี่ยนชื่อเควสที่จะแสดงผล
        questNameText.text = questName;

        // สั่งให้อนิเมชั่นทำงาน (เล่น Animation 'QuestPop')
        panelAnimator.Play("QuestPopup", 0, 0f);
    }
}