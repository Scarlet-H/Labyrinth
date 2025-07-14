using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public Text scoreText;
    public Text highscoreText;
    int score = 0;
    int highscore = 0;

    private void Awake() 
    {
        instance = this;
    }
    void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
        scoreText.text = score.ToString();
        highscoreText.text = "Highscore: "+highscore.ToString();
    }

    public void AddPoint(int amount)
    {
        score+=amount;
        scoreText.text = score.ToString();
        if (highscore < score)
        {
            highscore = score;
            highscoreText.text = "Highscore: " + highscore.ToString();
            PlayerPrefs.SetInt("highscore", score);
        }
    }
}
