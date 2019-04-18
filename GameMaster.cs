using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    //Ali Rashidinejad
    //counts points and sets coinText equal to points
    public int points;

    public Text coinText;

    void Update()
    {
        coinText.text = "Points: " + points;
    }
}
