using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public bool isMain = false;
    public static ShopManager main;
    private List<ShopItem> items_for_sale = new List<ShopItem>();

    public List<ShopItem> purchasable_items
    {
        get
        {
            return items_for_sale;
        }
    }


    private void Start()
    {
        //Let main ShopManager be called as `ShopManager.main`
        if(isMain)
        {
            main = this;
        }
    }


    private void AddItemToInventory(ShopItem item)
    {
        Debug.LogWarning(item.name + " ID:" + item.ID.ToString() + " is trying to be added to inventory, but functionality has not been implemented!");
    }


    //Converts Items behaviour objects -> ShopItem struct objects to add them
    public void AddItemsToShop(List<Items> items)
    {
        List<ShopItem> shop_items = new List<ShopItem>();
        foreach(Items item in items)
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
    }

    //Given an item object, buy that item
    public bool PurchaseItem(ShopItem item)
    {
        if(items_for_sale.Contains(item)) //If item in shop
        {
            //Remove item from shop and add to inventory
            items_for_sale.Remove(item);
            AddItemToInventory(item);
            return true;
        }
        return false;
    }

    //Given a shop slot, buy that item
    public bool PurchaseItem(int slot)
    {
        if(items_for_sale.Count > slot) //If list is large enough for slot to exists
        {
            ShopItem item = items_for_sale[slot]; //Get item from slot
            items_for_sale.Remove(item); //Remove item from shop
            AddItemToInventory(item); //Add item to inventory
            return true;
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
    }

    //Simple function to export the values from the item behaviour -> struct
    public ShopItem ItemToShopItem(Items item)
    {
        //Create struct object
        ShopItem shop_item = new ShopItem();

        //Set values
        shop_item.ID = item.ID;
        shop_item.name = item.name;
        shop_item.type = item.type;
        shop_item.subType = item.subType;
        shop_item.value = item.value;
        shop_item.secValue = item.secValue;
        shop_item.description = item.description;
        shop_item.icon = item.icon;

        return shop_item;
    }
}


