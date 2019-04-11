using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    //First Level
    public string startLevel;

    //Level Select Screen
    public string levelSelect;

    //Loads first level
    public void NewGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(startLevel);
    }

    //Loads Level Select Screen
    public void LevelSelect()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelSelect);
    }

    //Quits application if we have a working build
    public void QuitGame()
    {
        Application.Quit();
    }
}
