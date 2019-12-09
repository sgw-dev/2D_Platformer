using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    public void talkToPlayer() {
        Debug.Log("Wizard!!");
        ShopManager.main.ToggleGui();
    }
}
