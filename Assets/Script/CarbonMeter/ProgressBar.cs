using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    private void Start()
    {
        RefreshBar();
    }

    public void RefreshBar()
    {
        if (fillImage != null && ZoneDataManager.Instance != null)
        {
            fillImage.fillAmount = ZoneDataManager.Instance.GetCurrentProgress();
        }
    }

    public void Decrease(float amount)
    {
        if (ZoneDataManager.Instance != null)
        {
            ZoneDataManager.Instance.DecreaseProgress(amount);
        }
        RefreshBar();

        if (fillImage != null && fillImage.fillAmount <= 0f)
        {
            Debug.Log("Zone Complete");
        }
    }
}