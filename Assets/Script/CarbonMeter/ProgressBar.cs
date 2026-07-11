using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image fillImage;

    public float decreaseAmount = 0.1f; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            fillImage.fillAmount -= decreaseAmount;

            // ไม่ให้ค่าติดลบ
            fillImage.fillAmount = Mathf.Clamp01(fillImage.fillAmount);

            // ถ้าหลอดหมด
            if (fillImage.fillAmount <= 0)
            {
                Debug.Log("Progress หมดแล้ว!");
            }
        }
    }
}