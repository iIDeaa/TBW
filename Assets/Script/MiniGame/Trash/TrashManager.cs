using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrashManager : MonoBehaviour
{
    public static TrashManager Instance;

    public int score = 0;

    public TextMeshProUGUI scoreText;
    public List<TrashData> allTrashs = new List<TrashData>();
    public int totalItems = 15;
    public int itemPerRound = 6;
    private List<TrashData> gameTrashs = new List<TrashData>();
    public Transform spawnParent;
    private int remainRoundItem = 0;

    private int currentIndex = 0;
    public ItemData rewardItem;
    [Header("Result")]
    public float elapsedTime;
    public int correctCount;
    public int wrongCount;
    public int rewardAmount;

    private bool isPlaying = false;
    public TrashResultUI resultUI;
    
    

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateScoreUI();

        PrepareItems();

        SpawnNextRound();

        
        isPlaying = false;
    }
    void Update()
    {
        if (isPlaying)
        {
            elapsedTime += Time.deltaTime;
        }
    }
    void PrepareItems()
    {
        gameTrashs.Clear();

        for (int i = 0; i < totalItems; i++)
        {
            int randomIndex = Random.Range(0, allTrashs.Count);
            gameTrashs.Add(allTrashs[randomIndex]);
        }
    }
    public void SpawnNextRound()
    {
        int count = Mathf.Min(itemPerRound, gameTrashs.Count - currentIndex);
        remainRoundItem = count;

        for (int i = 0; i < count; i++)
        {
            Instantiate(
                gameTrashs[currentIndex].prefab,
                spawnParent);

            currentIndex++;
        }
    }

    public void ItemFinished()
    {
        remainRoundItem--;

        if (remainRoundItem <= 0)
        {
            if (currentIndex >= gameTrashs.Count)
            {
                EndGame();
            }
            else
            {
                SpawnNextRound();
            }
        }
    }

    void GiveReward()
    {
        rewardAmount = score / 4;

        for (int i = 0; i < rewardAmount; i++)
        {
            InventoryManager.Instance.AddItem(rewardItem);
        }

        Debug.Log("Get " + rewardAmount + " ");
    }
    

    public void AddScore(int amount)
    {
        score += amount;
        correctCount++;
        UpdateScoreUI();;
    }
    public void MinusScore(int amount)
    {
        score -= amount;
        wrongCount++;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "" + score;
    }

    public void Continue()
    {
        MinigameManager.Instance.EndMinigame(true);
    }
    public void StartGame()
    {
        elapsedTime = 0f;
        isPlaying = true;
    }
    void EndGame()
    {
        isPlaying = false;

        GiveReward();
        ZoneDataManager.Instance.DecreaseProgress(0.1f);

        resultUI.ShowResult(
            elapsedTime,
            correctCount,
            wrongCount,
            rewardAmount
        );
    }
}
