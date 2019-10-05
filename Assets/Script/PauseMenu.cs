using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    //level select has not been designed yet so scene is only blank screen.
    public string levelSelect;

    //Start Menu
    public string mainMenu;

    //Current Level
    public string startLevel;

    //Determines if game is paused
    public bool isPaused;

    //The visual pause menu
    public GameObject pauseMenuCanvas;

    //pause function
    void Update()
    {
        //if paused show pause menu and stop all game movement
        if (isPaused)
        {
            pauseMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
        //if not paused hide pause menu and resume all game movement
        else
        {
            pauseMenuCanvas.SetActive(false);
            Time.timeScale = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }
    }

    //resumes game
    public void Resume()
    {
        isPaused = false;
    }

    //Takes you to level select screen
    public void LevelSelect()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelSelect);
    }

    //Takes you to main menu
    public void Quit()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenu);
    }

    //Reloads current level
    public void LoadCurrentScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(startLevel);
    }
}
