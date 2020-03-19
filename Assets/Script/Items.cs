using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public int ID;
    public string itemName;
    public string type;
    public string subType;
    public float value;
    public float secValue;
    public string description;
    public Sprite icon;
    public bool acquired;
    public bool equipped;

    public void Update()
    {
        if (equipped)
        {
            // some weapon actions
        }
    }
    public void newItems(string s)
    {
        string[] parts = s.Split('\t');
        ID = int.Parse(parts[0]);
        itemName = parts[1];
        type = parts[2];
        subType = parts[3];
        value = float.Parse(parts[4]);
        secValue = float.Parse(parts[5]);
        description = parts[6];
    }
    public void newItems(int id, string t, string des)
    {
        ID = id;
        type = t;
        description = des;

    }

    public void UseItem()
    {
        if (type == "Weapon")
        {
            equipped = true;
        }
    }
    public Sprite getIcon()
    {
        return icon;
    }

}
