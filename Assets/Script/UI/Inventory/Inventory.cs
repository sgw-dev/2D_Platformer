using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject inventory;
    public GameObject boxPanels;
    public GameObject characterBackground;
    public Text goldText;

    private bool inventoryEnabled;
    private int inventorySize;
    public GameObject[] slot;
    public GameObject[] equips;
    Item item;
    public LayerMask hitMask;
    public int gold;

    void Start()
    {
        // sets size of inventroy to max 18
        inventorySize = 18;
        slot = new GameObject[inventorySize];
        // populates inventory with box panels
        for (int i = 0; i < inventorySize; i++)
        {
            slot[i] = boxPanels.transform.GetChild(i).gameObject;
        }
        equips = new GameObject[5];
        for(int i=0; i<equips.Length; i++)
        {
            equips[i] = characterBackground.transform.GetChild(i+1).gameObject;
        }
    }
    void Update()
    {

    }
    public Item getEquiped(int index)
    {
        return equips[index].GetComponent<EquipmentWatcher>().item;
    }
    public GameObject[] getSlots()
    {
        return slot;
    }
    public void clearInv()
    {
        foreach(GameObject s in slot){
            s.GetComponent<Slot>().removeItem();
        }
        foreach(GameObject e in equips)
        {
            e.GetComponent<EquipmentWatcher>().clearItem();
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

    private void OnCollisionEnter(Collision other)
    {
        // if collide with object labled "Item" adds it to inventory
        if (other.gameObject.tag == "Item")
        {
            Debug.Log("Got Item");
            GameObject itemAcquired = other.gameObject;
            item = itemAcquired.GetComponent<ItemMananger>().getItem();
            AddItem(item.getId());
            Destroy(other.gameObject);
        }
    }

    public void AddItem(int id)
    {
        Debug.Log("Adding " + id + " item");
        // goes through inventory to find a free slot
        for (int i = 0; i < inventorySize; i++)
        {
            // if empty slot is found
            if (slot[i].GetComponent<Slot>().isEmpty())
            {
                slot[i].GetComponent<Slot>().addItem(id);
                return;
            }
            else
            {
                
            }
            
        }
    }
    public void AddGold(int amount)
    {
        Debug.Log("Adding " + amount + " gold");
        gold += amount;
        goldText.text = gold.ToString();
    }
    public int Gold
    {
        get
        {
            return gold;
        }
        set
        {
            gold = value;
            goldText.text = gold.ToString();
        }
    }
    public void GoldTransact(int amount)
    {
        if (gold + amount < 0)
        {
            Debug.LogWarning("You cannot afford this");
            return;
        }
        gold += amount;
        //goldpanel.text = gold + "";
    }
}