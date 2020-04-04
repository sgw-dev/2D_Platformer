using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    static readonly string MAXHP = "playermaxhealth";
    static readonly string CURHP = "playercurrenthealth";
    static readonly string INV = "inventoryslot_";
    private GameObject popup;

    void Start()
    {
        popup = GameObject.Find("Canvas/Warning");
        popup.SetActive(false);
    }
    public void resume()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }
    public void quit()
    {
        Application.Quit();
    }
    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    public void newGame()
    {
        reset();
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }
    public void reset()
    {
        PlayerPrefs.DeleteKey(MAXHP);
        PlayerPrefs.DeleteKey(CURHP);
        for (int i = 0; i < 18; i++)
        {
            PlayerPrefs.DeleteKey(INV + i);
        }
    }
    public void showPrompt()
    {
        popup.SetActive(true);
    }
    public void back()
    {
        popup.SetActive(false);
    }
}
