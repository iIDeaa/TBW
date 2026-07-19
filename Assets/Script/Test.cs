using UnityEngine;

public class TestQuestTrigger : MonoBehaviour
{
    void Update()
    {
        // ถ้ากดปุ่ม Spacebar บนคีย์บอร์ด
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // เรียกใช้ระบบแจ้งเตือนเควส
            QuestNotificationSystem.Instance.TriggerNotification("TESTTTQUEST");
        }
    }
}