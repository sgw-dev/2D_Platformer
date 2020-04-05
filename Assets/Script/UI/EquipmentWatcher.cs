using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Watching { Head, Chest, Feet, Weapon, Shield}
public class EquipmentWatcher : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public Item item;
    private Vector2 location;
    private GameObject overlord;
    private GameObject inventory;
    private Sprite emptyIcon;//The image of the empty slot
    public Watching typeOfInv;

    void Start()
    {
        overlord = GameObject.Find("OverLord");
        inventory = GameObject.Find("Character/Inventory");
        location = this.GetComponent<RectTransform>().anchoredPosition;
        //Slot starts out empty so...
        emptyIcon = GetComponent<Image>().sprite;
    }
    public void setItem(int id)
    {
        
        if(item == null || item.getId() == 0)
        {
            
            item = overlord.GetComponent<ReadIn>().getItem(id);
            this.GetComponent<Image>().sprite = item.getIcon();

        }
        else
        {
            inventory.GetComponent<Inventory>().AddItem(item.getId());
            item = overlord.GetComponent<ReadIn>().getItem(id);
            this.GetComponent<Image>().sprite = item.getIcon();
        }
        switch (typeOfInv)
        {
            case Watching.Head:
                overlord.GetComponent<InvEventHandling>().HeadEmpty = false;
                break;
            case Watching.Chest:
                overlord.GetComponent<InvEventHandling>().ChestEmpty = false;
                break;
            case Watching.Feet:
                overlord.GetComponent<InvEventHandling>().FeetEmpty = false;
                break;
            case Watching.Weapon:
                overlord.GetComponent<InvEventHandling>().WeaponEmpty = false;
                break;
            case Watching.Shield:
                overlord.GetComponent<InvEventHandling>().ShieldEmpty = false;
                break;
        }
    }
    public bool empty()
    {
        if(item == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Pointer entered "+ typeOfInv);
        switch (typeOfInv)
        {
            case Watching.Head:
                overlord.GetComponent<InvEventHandling>().Head = true;
                break;
            case Watching.Chest:
                overlord.GetComponent<InvEventHandling>().Chest = true;
                break;
            case Watching.Feet:
                overlord.GetComponent<InvEventHandling>().Feet = true;
                break;
            case Watching.Weapon:
                overlord.GetComponent<InvEventHandling>().Weapon = true;
                break;
            case Watching.Shield:
                overlord.GetComponent<InvEventHandling>().Shield = true;
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        switch (typeOfInv)
        {
            case Watching.Head:
                overlord.GetComponent<InvEventHandling>().Head = false;
                break;
            case Watching.Chest:
                overlord.GetComponent<InvEventHandling>().Chest = false;
                break;
            case Watching.Feet:
                overlord.GetComponent<InvEventHandling>().Feet = false;
                break;
            case Watching.Weapon:
                overlord.GetComponent<InvEventHandling>().Weapon = false;
                break;
            case Watching.Shield:
                overlord.GetComponent<InvEventHandling>().Shield = false;
                break;
        }
    }
    public void clearItem()
    {
        item = null;
        this.GetComponent<Image>().sprite = emptyIcon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        inventory.GetComponent<Inventory>().AddItem(item.getId());
        clearItem();
    }
}
