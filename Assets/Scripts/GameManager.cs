using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameUIPanel gameUIPanel;
    public GameObject pauseMenu;

    public bool isGamePaused = false;

    public bool playerDead = false;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        gameUIPanel = FindAnyObjectByType<GameUIPanel>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
    public void EndGame()
    {

    }

    public void PlayerDie()
    {

    }
}
