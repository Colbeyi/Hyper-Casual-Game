using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelController : MonoBehaviour
{
    public static LevelController Current;
    public bool gameActive = false;
    public GameObject startMenu, gameMenu, gameOverMenu, finishMenu;
    public Text scoreText, finishScoreText, currentLevelText, nextLevelText;
    void Start()
    {
        Current = this;
        int currentLevel = PlayerPrefs.GetInt("currentLevel");
        if (SceneManager.GetActiveScene().name != "Level" + currentLevel)
        {
            SceneManager.LoadScene("Level" + currentLevel);

        }
        else
        {
            currentLevelText.text = (currentLevel + 1).ToString();
            nextLevelText.text = (currentLevel + 2).ToString();
        }

    }
    public void StartLevel()
    {
        PlayerMovement.Current.ChangeSpeed(PlayerMovement.Current.runSpeed);
        startMenu.SetActive(false);
        gameMenu.SetActive(true);


        gameActive = true;
    }

    
    void Update()
    {
        
    }
}
