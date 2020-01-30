using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIElementManager : MonoBehaviour
{
    public Scrollbar hp;
    public Button stats;
    public Scrollbar xp;
    public Text stattwindow;

    void Start()
    {
        if (hp == null)
        {
            Debug.LogError("Attach an hp bar");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void loadLevelSelect() {
        SceneManager.LoadScene("LevelSelect", LoadSceneMode.Single);
    }
}  
