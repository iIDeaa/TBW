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

            
            fillImage.fillAmount = Mathf.Clamp01(fillImage.fillAmount);

            
            if (fillImage.fillAmount <= 0)
            {
                Debug.Log("Progress หมดแล้ว!");
            }
        }
    }
}