using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadInGUI : MonoBehaviour
{
    // Loads in GUI Scene
    void Start()
    {
        SceneManager.LoadScene("GUI", LoadSceneMode.Additive);
    }

}
