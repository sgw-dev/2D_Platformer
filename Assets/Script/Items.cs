using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public int ID;
    public string name;
    public string type;
    public string subType;
    public float value;
    public float secValue;
    public string description;
    public Sprite icon;

    public void newItems() {

    }
    public void newItems(string s) {
        string[] parts = s.Split('\t');
        ID = int.Parse(parts[0]);
        name = parts[1];
        type = parts[2];
        subType = parts[3];
        value = float.Parse(parts[4]);
        secValue = float.Parse(parts[5]);
        description = parts[6];
    }
    public void newItems(int id, string t, string des) {
        ID = id;
        type = t;
        description = des;

    }
    

}
