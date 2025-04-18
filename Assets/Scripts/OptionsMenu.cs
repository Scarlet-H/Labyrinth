using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Toggle modeToggle;
    public int mode = 0; //0=light; 1=night
    public Camera mainCamera;
    void Start()
    {
        mode = PlayerPrefs.GetInt("mode", 0);
        if (mode == 0)
        {
            mainCamera.backgroundColor = new Color(0.8470588235294118f, 0.7764705882352941f, 1f);
        }
        else
        {
            mainCamera.backgroundColor = Color.black;
        }
        if (mode == 1)
        {
            modeToggle.isOn = true;
        }
        else { modeToggle.isOn = false;}
    }
    public void ToggleMode() //change theme
    {
        mode = modeToggle.isOn? 1: 0;
        PlayerPrefs.SetInt("mode", mode);
        if(mode == 0)
        {
            mainCamera.backgroundColor = new Color(0.8470588235294118f, 0.7764705882352941f, 1f);
        }
        else
        {
            mainCamera.backgroundColor = Color.black;
        }
    }
    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
