using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Weapon, Helmet, Chest, Boots, Gun, Accessory, Spell, Potion, Rune, Shield}

[Serializable]
public class Item 
{
    [SerializeField]
    private int ID;
    private string description;
    private string itemName;
    private ItemType type;
    private string subType;
    private float value;
    private float secValue;
    [SerializeField]
    private Sprite icon;
    private Sprite image;


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
                type = ItemType.Weapon;
                break;
            case "Helmet":
                type = ItemType.Helmet;
                break;
            case "Chest":
                type = ItemType.Chest;
                break;
            case "Boots":
                type = ItemType.Boots;
                break;
            case "Gun":
                type = ItemType.Gun;
                break;
            case "Accessory":
                type = ItemType.Accessory;
                break;
            case "Spell":
                type = ItemType.Spell;
                break;
            case "Potion":
                type = ItemType.Potion;
                break;
            case "Rune":
                type = ItemType.Rune;
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
        icon = GameObject.Find("OverLord").GetComponent<ReadIn>().getIcon(ID);
    }
    public void setImage()
    {
        image = GameObject.Find("OverLord").GetComponent<ReadIn>().getSprite(ID);
    }
    public Sprite getIcon()
    {
        return icon;
    }
    public Sprite getImage()
    {
        return image;
    }
    public ItemType getType()
    {
        return type;
    }
    public int getValue()
    {
        return (int)value;
    }
}
