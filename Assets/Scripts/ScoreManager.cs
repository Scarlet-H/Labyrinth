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

    private void Awake() //setting an instance of manager before the start of the game
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
        scoreText.text = score.ToString();
        highscoreText.text = "Highscore: "+highscore.ToString();
    }

    public void AddPoint()
    {
        score++;
        scoreText.text = score.ToString();
        if (highscore < score)
        {
            highscore = score;
            highscoreText.text = "Highscore: " + highscore.ToString();
            PlayerPrefs.SetInt("highscore", score);

        }

    }
}
