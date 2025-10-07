using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GameUIPanel : MonoBehaviour
{
    public Image playerHeart;

    public Sprite life0,life1,life2,life3;

    public Text score;

    public Image Buff;

    public Image pauseButtonImage;

    public Sprite pauseButton1, pauseButton2;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.Instance.isGamePaused &&Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.PauseGame();
        }
    }

    public void UpdateHealthDisplay(int currentHealth)
    {
        switch (currentHealth)
        {
            case 3:
                playerHeart.sprite = life3;
                break;
            case 2:
                playerHeart.sprite = life2;
                break;
            case 1:
                playerHeart.sprite = life1;
                break;
            case 0:
                playerHeart.sprite = life0;
                break;
            default:
                playerHeart.sprite = life0; // No hearts left
                break;
        }
    }

    public void ChangePauseButtonImage()
    {
        if (GameManager.Instance.isGamePaused)
        {
            pauseButtonImage.sprite = pauseButton2; // °ëÍ¸Ã÷
        }
        else
        {
            pauseButtonImage.sprite = pauseButton1; // ²»Í¸Ã÷
        }
    }

    public void UpdateScoreDisplay(int currentScore)
    {
        score.text = currentScore.ToString();
    }
}
