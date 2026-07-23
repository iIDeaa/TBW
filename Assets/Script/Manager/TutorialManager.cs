using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] pages;

    private int currentPage = 0;

    void Start()
    {
        ShowPage(0);
    }

    void ShowPage(int index)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == index);
        }
    }

    public void NextPage()
    {
        currentPage++;

        if (currentPage < pages.Length)
        {
            ShowPage(currentPage);
        }
        else
        {
            gameObject.SetActive(false); // ปิด Tutorial เมื่อหน้าสุดท้าย
        }
    }

    public void PreviousPage()
    {
        currentPage--;

        if (currentPage < 0)
            currentPage = 0;

        ShowPage(currentPage);
    }
}
