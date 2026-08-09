using UnityEngine;

public class ComboSys : MonoBehaviour
{
    private KeyCode[] combo = new KeyCode[]
    {
        KeyCode.UpArrow,
        KeyCode.UpArrow,
        KeyCode.LeftArrow,
        KeyCode.DownArrow
    };

    public System.Action onComboFail;

    private int currentIndex = 0;
    private bool comboComplete = false;

    public System.Action onComboSuccess;

    void OnEnable()
    {
        ResetCombo();
    }

    void Update()
    {
        if (comboComplete)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // 🔥 เช็ค green zone
                if (TrashMiniGameManager.Instance.sliderController.IsInGreenZone())
                {
                    Debug.Log("💥 PERFECT COMBO!");

                    onComboSuccess?.Invoke();
                }
                else
                {
                    Debug.Log("❌ Combo MISS (ไม่อยู่ใน green zone)");
                }

                ResetCombo();
            }
        }

        if (Input.GetKeyDown(combo[currentIndex]))
        {
            currentIndex++;
            Debug.Log("✔ " + currentIndex);

            if (currentIndex >= combo.Length)
            {
                comboComplete = true;
                Debug.Log("🔥 กด SPACE เพื่อใช้ Combo");
            }
        }
        else if (
            Input.GetKeyDown(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.RightArrow)
        )
        {
            Debug.Log("❌ ผิด รีเซ็ต");
            onComboFail?.Invoke();
            ResetCombo();
        }
    }

    void ResetCombo()
    {
        currentIndex = 0;
        comboComplete = false;
    }
}