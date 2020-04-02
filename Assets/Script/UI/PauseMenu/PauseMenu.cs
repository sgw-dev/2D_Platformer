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
    public bool isInv;

    //The visual pause menu
    public GameObject pauseMenuCanvas;
    public GameObject invMenuCanvas;

    public GameObject deathCanvas;

    //pause function


    void Start()
    {
        pauseMenuCanvas = GameObject.Find("Canvas/PausePanel");
        pauseMenuCanvas.SetActive(false);
        invMenuCanvas = GameObject.Find("Canvas/Inventory");
        deathCanvas = GameObject.Find("Canvas/DeathPanel");
        deathCanvas.SetActive(false);
    }
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
        //if paused show pause menu and stop all game movement
        if (isInv)
        {
            invMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
        //if not paused hide pause menu and resume all game movement
        else if(!isInv)
        {
            invMenuCanvas.SetActive(false);
            Time.timeScale = 1f;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            isInv = !isInv;
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
        Application.Quit();
    }
    public void main()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenu);
    }

    //Reloads current level
    public void LoadCurrentScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(startLevel);
    }
    public void openPause() {
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        isPaused = !isPaused;
    }
    public void closePause() {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPaused = !isPaused;
    }
    public void openInv()
    {
        invMenuCanvas.SetActive(isInv = !isInv);
        Time.timeScale = 0f;
    }
    public void closeInv()
    {
        invMenuCanvas.SetActive(isInv);
        Time.timeScale = 1f;
        isInv = !isInv;
    }
    public void respawn()
    {
        deathCanvas.SetActive(true);
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(startLevel);
    }
}
