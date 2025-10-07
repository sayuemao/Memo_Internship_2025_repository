using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Level1_unlocked", 1);       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
