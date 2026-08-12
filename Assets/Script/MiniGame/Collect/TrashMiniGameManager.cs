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
    public TrashMinigameResultUI resultUI;

    [Header("Result")]
    public float elapsedTime;
    public int correctCount;
    public int wrongCount;

    private Trash currentTrash;

    private bool isPlaying;

    public bool usedCombo = false;

    private void Awake()
    {
        Instance = this;
    }

        private void Update()
    {
        if(isPlaying)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    public void StartGame()
    {
        elapsedTime = 0;

        correctCount = 0;

        wrongCount = 0;


        isPlaying = true;


        trashSpawner.StartRound();
    }
    private void Start()
    {
        StartGame();
    }


    public void StartTrashGame(Trash trash)
    {
        Debug.Log("เริ่ม Minigame");

        currentTrash = trash;

        usedCombo = false;

        comboSys.gameObject.SetActive(false);

        sliderController.StartGame();

        playerMove.canMove = false;
    }

    public void OnComboSuccess()
    {
        Debug.Log("💥 COMBO FINISH (Instant)");

        comboSys.gameObject.SetActive(false);
        sliderController.gameObject.SetActive(false);

        if (currentTrash != null)
        {
            currentTrash.Collect();
            currentTrash = null;
        }

        if (currentCore != null)
        {
            currentCore.OnComboSuccess();

            TrashCoreManager.Instance.CheckEndGameSafe();
        }

        playerMove.canMove = true;

        TrashMiniGameUI.Instance.UpdateHit(0);

        usedCombo = false;
    }

    public void OnComboFail()
    {
        if (currentCore != null)
        {
            currentCore.OnComboFail();
        }
    }

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

        TrashMiniGameUI.Instance.UpdateHit(0);
    }

    //End
    public void EndGame()
    {
        isPlaying = false;
        Debug.Log("End Game");
        Debug.Log($"TIME: {elapsedTime} | CORRECT: {correctCount} | WRONG: {wrongCount}");

        resultUI.ShowResult(
            elapsedTime,
            correctCount,
            wrongCount
        );
    }

    public void Continue()
    {
        Debug.Log("Continue");

        playerMove.canMove = true;
    }

    public void StartCoreGame(CarbonCore core)
    {
        currentCore = core;

        comboSys.gameObject.SetActive(true);
        comboSys.onComboSuccess = OnComboSuccess;
        comboSys.onComboFail = OnComboFail; 

        sliderController.StartGame();

        playerMove.canMove = false;
    }

    public void AddCorrect() {
        correctCount++;
    }
    public void AddWrong() {
        wrongCount++;
    }
    public void OnSliderHit() { }
}