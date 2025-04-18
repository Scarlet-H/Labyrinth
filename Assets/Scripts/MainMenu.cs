using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    int highscore = 0;
    public Text highscoreText;
    public int mode = 0; //theme (0 - light theme, 1 - night theme)
    public Camera mainCamera;

    public void Play()
    {
        PlayerPrefs.SetInt("timedPlay",0); //endless play
        SceneManager.LoadSceneAsync(2);
    }
    public void TimedPlay()
    {
        PlayerPrefs.SetInt("timedPlay", 1); //timed play
        SceneManager.LoadSceneAsync(2);
    }
    public void Options()
    {
        SceneManager.LoadSceneAsync(1);
    }
    private void Start()
    {   
        mode = PlayerPrefs.GetInt("mode", 0);
        if (mode == 0)
        {
            mainCamera.backgroundColor = new Color(0.8470588235294118f, 0.7764705882352941f, 1f); //lilac background
        }
        else
        {
            mainCamera.backgroundColor = Color.black;
        }
        highscore = PlayerPrefs.GetInt("highscore", 0);
        highscoreText.text = "Highscore: " + highscore.ToString();
    }
}
