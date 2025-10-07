using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameUIPanel gameUIPanel;

    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject pauseMenu;

    public int enemycount = 0;

    public bool isGamePaused = false;

    public bool isPlayerWin = false;

    public bool playerDead = false;

    public string backSceneName = "MainMenu";

    public string nextLevelName;

    public int currentScore = 0;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        gameUIPanel = FindAnyObjectByType<GameUIPanel>();
        enemycount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPlayerWin && enemycount<=0)
        {
            isPlayerWin = true;
            PlayerWin();
        }
    }

    public void UpdateHealthDisplay(int currentHealth)
    {
        gameUIPanel.UpdateHealthDisplay(currentHealth);
    }

    public void PauseGame()
    {
        if (!isGamePaused)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            PlayerController.Instance.stopInput = true;
            isGamePaused = true;
            gameUIPanel.ChangePauseButtonImage();
        }
    }

    public void ResumeGame()
    {
        if (isGamePaused)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            PlayerController.Instance.stopInput = false;
            isGamePaused = false;
            gameUIPanel.ChangePauseButtonImage();
        }
    }

    public void RestartGame()
    {
        if (isGamePaused)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void NextLevel()
    {
        if (isGamePaused)
        {
            SceneManager.LoadScene(nextLevelName);
        }
    }
    public void EndGame()
    {
        SceneManager.LoadScene(backSceneName);
    }

    public void PlayerDie()
    {
        isPlayerWin = false;
        isGamePaused = true;
        losePanel.SetActive(true);
        PlayerController.Instance.stopInput = true;
    }

    public void PlayerWin()
    {
        isPlayerWin = true;
        isGamePaused = true;
        winPanel.SetActive(true);
        PlayerController.Instance.stopInput = true;
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_win", 1);
        PlayerPrefs.SetInt(nextLevelName + "_unlocked", 1);
        if(PlayerPrefs.HasKey("totalscore"))
        {
            PlayerPrefs.SetInt("totalscore", PlayerPrefs.GetInt("totalscore") + currentScore);
        }
        else
        {
            PlayerPrefs.SetInt("totalscore", currentScore);
        }
        gameUIPanel.UpdateEndScoreDisplay();
    }

    public void AddScore(int scoreValue)
    {
        currentScore += scoreValue;
        gameUIPanel.UpdateScoreDisplay(currentScore);
    }

    public void UpdateBuff(Sprite updateSprite)//¸üÐÂUI
    {
        gameUIPanel.UpdateBuffDisplay(updateSprite);
    }
}
