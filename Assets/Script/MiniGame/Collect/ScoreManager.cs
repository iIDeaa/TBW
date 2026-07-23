using UnityEngine;
using TMPro;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;


    public TMP_Text scoreText;


    private int score;



    private void Awake()
    {
        Instance=this;
    }



    public void AddScore(int amount)
    {
        score += amount;


        if(scoreText)
            scoreText.text =
            "Score : " + score;
    }



    public int GetScore()
    {
        return score;
    }
}