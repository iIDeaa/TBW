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

    private int currentIndex = 0;
    private bool comboComplete = false;

    public System.Action onComboSuccess;

    void Update()
    {
        if (comboComplete)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("💥 COMBO FINISH!");

                onComboSuccess?.Invoke();

                ResetCombo();
            }
            return;
        }

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(combo[currentIndex]))
            {
                currentIndex++;
                Debug.Log("ถูก " + currentIndex);

                if (currentIndex >= combo.Length)
                {
                    comboComplete = true;
                    Debug.Log("พร้อมกด Spacebar!");
                }
            }
            else
            {
                // ❌ กดผิด รีเซ็ต
                Debug.Log("ผิด รีเซ็ต");
                ResetCombo();
            }
        }
    }

    void ResetCombo()
    {
        currentIndex = 0;
        comboComplete = false;
    }
}