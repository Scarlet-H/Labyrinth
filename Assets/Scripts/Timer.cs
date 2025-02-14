using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance; //because i have to access this script from other scripts
    [SerializeField] TextMeshProUGUI timerText;
    public float remainingTime;
    int minutes;
    int seconds;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (PlayerPrefs.GetInt("timedPlay")==0) //if endless -> infinite time
        {
            timerText.gameObject.SetActive(false);
            remainingTime = float.PositiveInfinity;
        }
        else
        {
            timerText.gameObject.SetActive(true);
            remainingTime = 30;
        }

    }
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else
        {
            remainingTime = 0;
            //timerText.color = Color.red;
            GameManager.Instance.GameOver();
        }
        minutes = Mathf.FloorToInt(remainingTime/60);
        seconds = Mathf.FloorToInt(remainingTime%60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void AddTime() //add time on TimeBonus collection
    {
        remainingTime += 20;
    }
}
