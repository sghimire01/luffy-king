using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseDeathMenu : MonoBehaviour
{
    public GameObject PauseUI;
    private bool paused = false;
    public GameObject DeathUI;
    public GameObject WinUI;
    public GameObject StartMenuUI;
    public GameObject SavedScoreUI;
    public Text ScoreNum;

    // Start is called before the first frame update
    void Start()
    {
        PauseUI.SetActive(false);
        DeathUI.SetActive(false);
        WinUI.SetActive(false);
        SavedScoreUI.SetActive(false);
        StartMenuUI.SetActive(true);
    }

    void Awake()
    {
        PauseUI.SetActive(false);
        DeathUI.SetActive(false);
        WinUI.SetActive(false);
        SavedScoreUI.SetActive(false);
        StartMenuUI.SetActive(true);
    }

    public void MainMenu()
    {
        Application.LoadLevel(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartButton()
    {
        Application.LoadLevel(1);
    }

    public void SavedScore()
    {
        StartMenuUI.SetActive(false);
        SavedScoreUI.SetActive(true);
        ScoreNum.text = "" + PlayerPrefs.GetInt("highscore");
    }

    public void Back()
    {
        StartMenuUI.SetActive(true);
        SavedScoreUI.SetActive(false);
    }
}
