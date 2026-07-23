using UnityEngine;


public class TrashMiniGameManager : MonoBehaviour
{
    public static TrashMiniGameManager Instance;


    [Header("Reference")]
    public TrashSpawner trashSpawner;

    public TrashMinigameResultUI resultUI;

    public SliderController sliderController;



    [Header("Result")]
    public float elapsedTime;

    public int correctCount;

    public int wrongCount;



    private bool isPlaying;


    private Trash currentTrash;



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




    public void StartTrashGame(Trash trash)
{
    Debug.Log("เปิด Slider");

    currentTrash = trash;

    sliderController.StartGame();
}




    public void FinishTrash()
    {
        if(currentTrash != null)
        {
            currentTrash.Collect();

            currentTrash = null;
        }
    }




    public void AddCorrect()
    {
        correctCount++;
    }



    public void AddWrong()
    {
        wrongCount++;
    }




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