﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    public void talkToPlayer() {
        Debug.Log("Wizard!!");
        Player.main.ToggleFrozen();
        ShopManager.main.ToggleGui();
    }
}
