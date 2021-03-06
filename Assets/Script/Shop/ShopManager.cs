﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public bool isMain = false;
    public static ShopManager main;
    private List<ShopItem> items_for_sale = new List<ShopItem>();

    public int money = 100; //This can be later replaced my pointing at a different variable
    public Inventory inv;

    public List<ShopItem> purchasable_items
    {
        get
        {
            return items_for_sale;
        }
    }


    private void Awake()
    {
        //Let main ShopManager be called as `ShopManager.main`
        if(isMain)
        {
            main = this;
        }

    }
    //Spencer
    void Start()
    {
        ReadIn read = GameObject.Find("OverLord").GetComponent<ReadIn>();
        List<Item> toSell = new List<Item>();
        toSell.Add(read.getItem(2));
        toSell.Add(read.getItem(5));
        toSell.Add(read.getItem(8));
        toSell.Add(read.getItem(11));
        toSell.Add(read.getItem(14));
        toSell.Add(read.getItem(17));
        toSell.Add(read.getItem(20));
        toSell.Add(read.getItem(23));
        AddItemsToShop(toSell);
        inv = GameObject.FindGameObjectWithTag("Inv").GetComponent<Inventory>();
        StartCoroutine(LateStart());
    }
    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(.2f);
        Debug.Log("Your gold is " + inv.Gold);
        money = inv.Gold;

    }

    public event Action onItemsUpdate;
    public event Action onToggleGui;

    public void ToggleGui()
    {
        if(onToggleGui != null)
        {
            onToggleGui();
        }
    }

    void ItemsUpdated()
    {
        if(onItemsUpdate != null)
        {
            onItemsUpdate();
        }
    }


    private void AddItemToInventory(ShopItem item)
    {
        GameObject.FindGameObjectWithTag("Inv").GetComponent<Inventory>().AddItem(item.ID);
        Debug.LogWarning(item.name + " ID:" + item.ID.ToString() + " is trying to be added to inventory, but functionality has not been implemented!");
    }


    //Converts Items behaviour objects -> ShopItem struct objects to add them
    public void AddItemsToShop(List<Item> items)
    {
        List<ShopItem> shop_items = new List<ShopItem>();
        foreach(Item item in items)
        {
            shop_items.Add(ItemToShopItem(item));
        }
        AddItemsToShop(shop_items);
    }

    //Add list of items to shop
    public void AddItemsToShop(List<ShopItem> items)
    {
        foreach(ShopItem item in items)
        {
            AddItemToShop(item);
        }
    }

    //Add single item to shop
    public void AddItemToShop(ShopItem item)
    {
        items_for_sale.Add(item);
        ItemsUpdated();
    }

    //Given an item object, buy that item
    public bool PurchaseItem(ShopItem item)
    {
        if(items_for_sale.Contains(item)) //If item in shop
        {
            if(item.cost <= money)
            {
                //Remove item from shop and add to inventory
                items_for_sale.Remove(item);
                money -= item.cost;
                inv.Gold -= item.cost;
                Debug.Log("Bought item: " + item.name);
                AddItemToInventory(item);
                ItemsUpdated();
                return true;
            }
        }
        return false;
    }

    //Given a shop slot, buy that item
    public bool PurchaseItem(int slot)
    {
        if(items_for_sale.Count > slot) //If list is large enough for slot to exists
        {
            ShopItem item = items_for_sale[slot]; //Get item from slot
            return PurchaseItem(item);
        }
        return false;
    }

    //Stuct to hold item information
    public struct ShopItem
    {
        public int ID;
        public string name;
        public string type;
        public string subType;
        public float value;
        public float secValue;
        public string description;
        public Sprite icon;
        public int cost;
    }

    //Simple function to export the values from the item behaviour -> struct
    public ShopItem ItemToShopItem(Item item)
    {
        //Create struct object
        ShopItem shop_item = new ShopItem();

        //Set values
        shop_item.ID = item.ID;
        shop_item.name = item.itemName;
        shop_item.type = item.type.ToString();
        shop_item.subType = item.subType;
        shop_item.value = item.value;
        shop_item.secValue = item.secValue;
        shop_item.cost = (int)item.secValue;//Spencer
        shop_item.description = item.description;
        shop_item.icon = item.icon;

        return shop_item;
    }
}


