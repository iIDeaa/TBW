using UnityEngine;

public class TrashMiniGameManager : MonoBehaviour
{
    public static TrashMiniGameManager Instance;

    [Header("Reference")]
    public TrashSpawner trashSpawner;
    public ComboSys comboSys;
    public SliderController sliderController;
    public PlayerMovements playerMove;
    public CarbonCore currentCore;

    private Trash currentTrash;

    public bool usedCombo = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (trashSpawner != null)
            trashSpawner.StartRound();
    }

    // =============================
    // 🎮 เริ่ม Minigame
    // =============================
    public void StartTrashGame(Trash trash)
    {
        Debug.Log("เริ่ม Minigame");

        currentTrash = trash;

        usedCombo = false;

        // เปิด Combo
        comboSys.gameObject.SetActive(true);
        comboSys.onComboSuccess = OnComboSuccess;

        // เปิด Slider
        sliderController.StartGame();

        // ล็อกการเดิน
        playerMove.canMove = false;
    }

    public void OnComboSuccess()
    {
        Debug.Log("💥 COMBO FINISH (Instant)");

        // ปิดทุกอย่างก่อน
        comboSys.gameObject.SetActive(false);
        sliderController.gameObject.SetActive(false);

        // เก็บขยะทันที
        if (currentTrash != null)
        {
            currentTrash.Collect();
            currentTrash = null;
        }

        if (currentCore != null)
        {
            currentCore.OnComboSuccess();

            EndGame();
        }

        // ปลดล็อกการเดิน
        playerMove.canMove = true;

        // รี UI
        TrashMiniGameUI.Instance.UpdateHit(0);

        //  รี state
        usedCombo = false;
        
    }

    //Combo พลาด
    public void OnComboFail()
    {
        if (currentCore != null)
        {
            currentCore.OnComboFail();
        }
    }

    // 🧹 จบเกม
    // =============================
    public void FinishTrash()
    {
        Debug.Log("💥 เก็บขยะสำเร็จ");

        if (currentTrash != null)
        {
            currentTrash.Collect();
            currentTrash = null;
        }

        sliderController.gameObject.SetActive(false);
        comboSys.gameObject.SetActive(false);

        playerMove.canMove = true;

        // รี UI
        TrashMiniGameUI.Instance.UpdateHit(0);
    }

    //End
    public void EndGame()
    {
        Debug.Log("End Game");

        // ถ้าคุณมี UI result ก็ใส่ตรงนี้
    // เช่น resultUI.Show();
    }

    public void Continue()
    {
        Debug.Log("Continue");

        // ปิด UI / กลับไปเกมหลัก
        playerMove.canMove = true;
    }

    public void StartCoreGame(CarbonCore core)
    {
        currentCore = core;

        comboSys.gameObject.SetActive(true);
        comboSys.onComboSuccess = OnComboSuccess;
        comboSys.onComboFail = OnComboFail; // 🔥 สำคัญ

        sliderController.StartGame();

        playerMove.canMove = false;
    }

    public void AddCorrect() { }
    public void AddWrong() { }
    public void OnSliderHit() { }
}