using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelShow : MonoBehaviour
{
    public string levelName;

    public string nextLevelName;

    //public Sprite buttonNumberImage;

    private Image lockButtonNumberImage;

    private Button levelButton;
    // Start is called before the first frame update
    void Start()
    {
        levelButton = GetComponentInChildren<Button>();
        lockButtonNumberImage = transform.GetChild(0).GetComponent<Image>();
        if (levelButton!=null)
        {
            levelButton.onClick.AddListener(OnLevelButtonClicked);
        }
        if (PlayerPrefs.HasKey(levelName + "_unlocked") && PlayerPrefs.GetInt(levelName + "_unlocked") == 1)
        {
            levelButton.enabled = true;
            lockButtonNumberImage.enabled = false;
        }
        else
        {
            levelButton.enabled = false;
            lockButtonNumberImage.enabled = true;
        }

        if(PlayerPrefs.HasKey(levelName + "_win") && PlayerPrefs.GetInt(levelName + "_win") == 1)
        {
            if (PlayerPrefs.HasKey(nextLevelName + "_unlocked"))
            {
                if (PlayerPrefs.GetInt(nextLevelName + "_unlocked") == 0)
                {
                    PlayerPrefs.SetInt(nextLevelName + "_unlocked", 1);
                }
            }
            else
            {
                PlayerPrefs.SetInt(nextLevelName + "_unlocked", 1);
            }
        }
    }

    private void OnDestroy()
    {
        if(levelButton!=null)
        {
            levelButton.onClick.RemoveAllListeners();
        }
    }

    private void OnLevelButtonClicked()
    {
        // 仅在关卡已解锁时加载
        if (levelButton != null && levelButton.IsActive() && !string.IsNullOrEmpty(levelName))
        {
            SceneManager.LoadScene(levelName);
        }
    }
}
