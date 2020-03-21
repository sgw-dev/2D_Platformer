using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject inventory;
    public GameObject boxPanels;

    private bool inventoryEnabled;
    private int inventorySize;
    public GameObject[] slot;
    Item item;
    public LayerMask hitMask;

    void Update()
    {
        // pulls up the inventory if I is pressed
        if (Input.GetKeyDown(KeyCode.I))
        {
            // sets inventory active or inactive
            if (inventoryEnabled == true)
            {
                //inventory.SetActive(true);
                /*showInventory();
                inventoryEnabled = true;
                Time.timeScale = 0f;*/
            }
            else
            {
                //inventory.SetActive(false);
                /*hideInventory();
                inventoryEnabled = false;
                Time.timeScale = 1f;*/
            }
        }
            
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            pickUp();
        }
    }
    private void hideInventory()
    {
        CanvasGroup cg = inventory.GetComponent<CanvasGroup>();
        cg.interactable = false;
        cg.alpha = 0;
        cg.blocksRaycasts = true;
    }
    private void showInventory()
    {
        CanvasGroup cg = inventory.GetComponent<CanvasGroup>();
        cg.interactable = true;
        cg.alpha = 1;
        cg.blocksRaycasts = false;
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
            /*if (slot[i].GetComponent<Slot>().Item != null)
                slot[i].GetComponent<Slot>().empty = false;*/
        }
    }

    private void pickUp()
    {
        Collider2D myCollider = this.GetComponent<CircleCollider2D>();
        int numColliders = 10;
        Collider2D[] colliders = new Collider2D[numColliders];
        ArrayList names = new ArrayList();
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.layerMask = hitMask;
        // Set you filters here according to https://docs.unity3d.com/ScriptReference/ContactFilter2D.html
        //int colliderCount = myCollider.OverlapCollider(contactFilter, colliders);
        int colliderCount = Physics2D.OverlapCollider(myCollider, contactFilter, colliders);
        for (int i = 0; i < numColliders; i++)
        {
            if (colliders[i] != null)
            {
                if (colliders[i].tag.CompareTo("Item") == 0)
                {

                    if (!names.Contains(colliders[i].name))
                    {
                        GameObject itemAcquired = colliders[i].gameObject;
                        item = itemAcquired.GetComponent<ItemMananger>().getItem();
                        AddItem(item.getId());
                        Destroy(colliders[i].gameObject);
                    }
                }

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // if collide with object labled "Item" adds it to inventory
        if (other.tag == "Item")
        {
            Debug.Log("Got Item");
            GameObject itemAcquired = other.gameObject;
            item = itemAcquired.GetComponent<ItemMananger>().getItem();
            AddItem(item.getId());
            Destroy(other);
        }
    }

    void AddItem(int id)
    {
        // goes through inventory to find a free slot
        for (int i = 0; i < inventorySize; i++)
        {
            // if empty slot is found
            if (slot[i].GetComponent<Slot>().isEmpty())
            {
                // pick up game object and call acquired from Items script, set to true
                //itemObject.GetComponent<Items>().acquired = true;

                slot[i].GetComponent<Slot>().addItem(id);
                /*
                // call item from Slot script and set it to the itemObject that was picked up
                slot[i].GetComponent<Slot>().item = itemObject;
                // call the ReadIn script and pass it the reference number
                slot[i].GetComponent<ReadIn>().getItem(id);

                // moves item object to correct slot and sets object to inactive
                itemObject.transform.parent = slot[i].transform;
                itemObject.SetActive(false);*/
                /*
                slot[i].GetComponent<Slot>().UpdateSlot();
                slot[i].GetComponent<Slot>().empty = false;*/
            }
            return;
        }
    }
}