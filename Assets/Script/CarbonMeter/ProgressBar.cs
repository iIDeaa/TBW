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
        fillImage.fillAmount = ZoneDataManager.Instance.GetCurrentProgress();
    }

    public void Decrease(float amount)
    {
        ZoneDataManager.Instance.DecreaseProgress(amount);
        RefreshBar();

        if (fillImage.fillAmount <= 0f)
        {
            Debug.Log("Zone Complete");
        }
    }
}