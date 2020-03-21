using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item 
{
    [SerializeField]
    private int ID;
    private string description;
    private string itemName;
    private string type;
    private string subType;
    private float value;
    private float secValue;
    [SerializeField]
    private Sprite icon;


    public Item()
    {

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
    public int getId()
    {
        return ID;
    }
    public string getName()
    {
        return itemName;
    }
    public string getDescription()
    {
        return description;
    }
    public void setIcon()
    {
        icon = GameObject.Find("OverLord").GetComponent<ReadIn>().getSprite(ID);
    }
    public Sprite getIcon()
    {
        return icon;
    }
}
