using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type { Weapon, Helmet, Chest, Boots, Gun, Accessory, Spell, Potion, Rune}

[Serializable]
public class Item 
{
    [SerializeField]
    private int ID;
    private string description;
    private string itemName;
    private Type type;
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
        string temp = parts[2];
        switch (temp)
        {
            case "Weapon":
                type = Type.Weapon;
                break;
            case "Helmet":
                type = Type.Helmet;
                break;
            case "Chest":
                type = Type.Chest;
                break;
            case "Boots":
                type = Type.Boots;
                break;
            case "Gun":
                type = Type.Gun;
                break;
            case "Accessory":
                type = Type.Accessory;
                break;
            case "Spell":
                type = Type.Spell;
                break;
            case "Potion":
                type = Type.Potion;
                break;
            case "Rune":
                type = Type.Rune;
                break;
        }
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
    public Type getType()
    {
        return type;
    }
}
