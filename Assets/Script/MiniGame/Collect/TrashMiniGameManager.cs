using UnityEngine;

public class TrashMiniGameManager : MonoBehaviour
{
    public static TrashMiniGameManager Instance;

    [Header("Reference")]
    public TrashSpawner trashSpawner;
    public TrashMinigameResultUI resultUI;
    public SliderController sliderController;
    public ComboSys comboSys;
    public PlayerMovements playerMove;

    [Header("Result")]
    public float elapsedTime;
    public int correctCount;
    public int wrongCount;

    private bool isPlaying;

    // 🔥 แยก Target
    private Trash currentTrash;
    public CarbonCore currentCore;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (isPlaying)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    // =============================
    // 🎮 เริ่มเกมหลัก
    // =============================
    public void StartGame()
    {
        elapsedTime = 0;
        correctCount = 0;
        wrongCount = 0;

        isPlaying = true;

        trashSpawner.StartRound();
    }

    // =============================
    // 🎮 เริ่ม Minigame
    // =============================
    public void StartTrashGame(object target)
    {
        Debug.Log("เปิดมินิเกม");

        currentTrash = null;
        currentCore = null;

        // 🔥 แยกประเภท
        if (target is Trash)
        {
            currentTrash = (Trash)target;

            // 🟢 Trash → ใช้ Slider
            sliderController.gameObject.SetActive(true);
            sliderController.StartGame();

            comboSys.gameObject.SetActive(false);
        }
        else if (target is CarbonCore)
        {
            currentCore = (CarbonCore)target;

            // 🔴 Core → ใช้ Combo เป็นหลัก
            comboSys.gameObject.SetActive(true);

            // 👉 เปิด slider ได้ แต่ฆ่าไม่ได้
            sliderController.gameObject.SetActive(true);
            sliderController.StartGame();
        }
        playerMove.canMove = false; // ตอนเริ่ม

    // =============================
    // 🟢 Slider โดน (เรียกจาก SliderController)
    // =============================
    public void OnSliderHit()
    {
        if (currentCore != null)
        {
            currentCore.TakeSliderDamage(5);
        }
        else if (currentTrash != null)
        {
            AddCorrect();
        }
    }

    // =============================
    // 🔴 Combo Success
    // =============================
    public void OnComboSuccess()
    {
        Debug.Log("💥 Combo สำเร็จ!");

        if (currentCore != null)
        {
            currentCore.OnComboSuccess(); // 💥 ฆ่าจริง
        }
        else if (currentTrash != null)
        {
            FinishTrash();
        }
    }

    // =============================
    // ⚠️ Combo Fail
    // =============================
    public void OnComboFail()
    {
        Debug.Log("❌ Combo พลาด!");

        if (currentCore != null)
        {
            currentCore.OnComboFail();
        }
    }

    // =============================
    // 🧹 จบ Trash (เท่านั้น)
    // =============================
    public void FinishTrash()
    {
        // ปิด UI
        comboSys.gameObject.SetActive(false);
        sliderController.gameObject.SetActive(false);

        playerMove.canMove = true;

        // 🟢 Trash เท่านั้น
        if (currentTrash != null)
        {
            currentTrash.Collect();
            TrashCoreManager.Instance.AddTrash();
            currentTrash = null;
        }

        // ❗ Core ห้ามตายตรงนี้
    }

    // =============================
    public void AddCorrect()
    {
        correctCount++;
    }

    public void AddWrong()
    {
        wrongCount++;
    }

    // =============================
    public void EndGame()
    {
        isPlaying = false;

        resultUI.ShowResult(
            elapsedTime,
            correctCount,
            wrongCount
        );
    }

    public void Continue()
    {
        MinigameManager.Instance.EndMinigame(true);
    }
}