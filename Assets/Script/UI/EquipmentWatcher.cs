using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Watching { Head, Chest, Feet, Weapon, Shield}
public class EquipmentWatcher : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Item item;
    private Vector2 location;
    private GameObject overlord;
    public Watching typeOfInv;

    void Start()
    {
        overlord = GameObject.Find("OverLord");
        location = this.GetComponent<RectTransform>().anchoredPosition;
    }
    public void setItem(int id)
    {
        Debug.Log("setItem Called");
        if(item.getId() == 0)
        {
            Debug.Log("Item is null");
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
}
