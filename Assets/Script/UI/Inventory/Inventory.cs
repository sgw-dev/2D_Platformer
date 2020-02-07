using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject inventory;
    private bool inventoryEnabled;

    private int inventorySize;
    public GameObject boxPanels;
    public GameObject[] slot;
    Items items;

    void Update()
    {
        // pulls up the inventory if I is pressed
        if (Input.GetKeyDown(KeyCode.I))
            inventoryEnabled = !inventoryEnabled;
        // sets inventory active or inactive
        if (inventoryEnabled == true)
        {
            inventory.SetActive(true);
        }
        else
        {
            inventory.SetActive(false);
        }
    }

    void Start()
    {
        // sets size of inventroy to max 18
        inventorySize = 18;
        slot = new GameObject[inventorySize];
        // populates inventory with box panels
        for (int i = 0; i < inventorySize; i++)
        {
            slot[i] = boxPanels.transform.GetChild(i).gameObject;
            // checks if slot is empty
            if (slot[i].GetComponent<Slot>().item != null)
                slot[i].GetComponent<Slot>().empty = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // if collide with object labled "Item" adds it to inventory
        if (other.tag == "Item")
        {
            GameObject itemAcquired = other.gameObject;
            items = itemAcquired.GetComponent<Items>();
            AddItem(itemAcquired, items.ID, items.type, items.description, items.icon);
        }
    }

    void AddItem(GameObject itemObject, int id, string type, string description, Sprite itemIcon)
    {
        // goes through inventory to find a free slot
        for (int i = 0; i < inventorySize; i++)
        {
            // if empty slot is found
            if (slot[i].GetComponent<Slot>().empty)
            {
                // pick up game object and call acquired from Items script, set to true
                itemObject.GetComponent<Items>().acquired = true;

                // call item from Slot script and set it to the itemObject that was picked up
                slot[i].GetComponent<Slot>().item = itemObject;
                // call the ReadIn script and pass it the reference number
                slot[i].GetComponent<ReadIn>().getItem(id);

                // moves item object to correct slot and sets object to inactive
                itemObject.transform.parent = slot[i].transform;
                itemObject.SetActive(false);

                slot[i].GetComponent<Slot>().UpdateSlot();
                slot[i].GetComponent<Slot>().empty = false;
            }
            return;
        }
    }
}