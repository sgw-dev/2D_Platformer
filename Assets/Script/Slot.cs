using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject       hoverPrefab;//The prefab for the Description Popup

    private Item            item;
    private Vector3       location;//Starting Location
    private bool            display;//If the Description Popup is showing

    private GameObject       paneLocation;//The location where the Pane will Be made
    private GameObject      pane;//Destription Popup
    private GameObject       overLord;//Refrence to Overlord Object
    private InvEventHandling invEvent;

    private GameObject[] equipSlots;//Refrence to Equipment Slots
    private Sprite blank;//The visual of an empty box

    private Image            image;//Refrence to the image component of the box
    private bool            dragging;//If the object is being dragged

    private bool            hovering;//If the mouse is on the slot; used to show the item description
    private int             childIndex;//The proper index location of the child


    void Start()
    {
        paneLocation = GameObject.Find("Canvas/Inventory/BoxesPanel");
        overLord = GameObject.Find("OverLord");
        invEvent = overLord.GetComponent<InvEventHandling>();
        image = this.GetComponent<Image>();
        location = this.transform.position;
        childIndex = transform.GetSiblingIndex();
        //The box starts as blank so just grab the starting sprite
        blank = image.sprite;
        //Grab the references to all the equipment slots
        equipSlots = new GameObject[] { GameObject.Find("Equip (1)"), GameObject.Find("Equip (2)"), GameObject.Find("Equip (3)"), GameObject.Find("Equip (4)"), GameObject.Find("Equip (5)") };
    }
    public Slot()
    {
        
    }
    public void Update()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            dragging = false;
            image.raycastTarget = true;
            transform.position = location;
        }
        if (Input.GetButtonDown("Fire2") && hovering && !display && item != null)
        {
            pane = Instantiate(hoverPrefab) as GameObject;
            pane.transform.SetParent(paneLocation.transform);
            pane.GetComponent<RectTransform>().localPosition = this.GetComponent<RectTransform>().localPosition + new Vector3(115, -55, 0);
            pane.GetComponentInChildren<Text>().text = item.getName() + "\n\n" + item.getDescription();
            display = true;
        }
        if (dragging && item != null)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //Make the thing you are dragging ontop of everything else
            transform.SetSiblingIndex(transform.childCount - 1);
        }
        
        
    }
    
    public bool isEmpty()
    {
        if(item == null)
        {
            return true;
        }
        else
        {
            if(item.getId() == 0)
            {
                return true;
            }
            return false;
        }
    }
    public void addItem(int id)
    {
        item = overLord.GetComponent<ReadIn>().getItem(id);
        image.sprite = item.getIcon();
    }
    public void removeItem()
    {
        item = null;
        image.sprite = blank;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Input.GetButton("Fire2"))
        {
            dragging = true;
            image.raycastTarget = false;
        }
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Check if it is over any of the equipment Slots
        if (invEvent.Head)
        {
            if(item.getType() == ItemType.Helmet)
            {
                equipSlots[0].GetComponent<EquipmentWatcher>().setItem(item.getId());
                removeItem();
            }
            else
            {
                transform.position = location;
                dragging = false;
                image.raycastTarget = true;
            }
            
        }else if (invEvent.Chest)
        {
            if (item.getType() == ItemType.Chest)
            {
                equipSlots[1].GetComponent<EquipmentWatcher>().setItem(item.getId());
                removeItem();
            }
            else
            {
                transform.position = location;
                dragging = false;
                image.raycastTarget = true;
            }
        }
        else if (invEvent.Feet)
        {
            if (item.getType() == ItemType.Boots)
            {
                equipSlots[2].GetComponent<EquipmentWatcher>().setItem(item.getId());
                removeItem();
            }
            else
            {
                transform.position = location;
                dragging = false;
                image.raycastTarget = true;
            }
        }
        else if (invEvent.Shield)
        {
            if (item.getType() == ItemType.Shield)
            {
                equipSlots[3].GetComponent<EquipmentWatcher>().setItem(item.getId());
                removeItem();
            }
            else
            {
                transform.position = location;
                dragging = false;
                image.raycastTarget = true;
            }
        }
        else if (invEvent.Weapon)
        {
            if (item.getType() == ItemType.Weapon)
            {
                equipSlots[4].GetComponent<EquipmentWatcher>().setItem(item.getId());
                removeItem();
            }
            else
            {
                transform.position = location;
                dragging = false;
                image.raycastTarget = true;
            }
        }
        else
        {
            //It was not ontop of anything so put it back
            transform.position = location;
            dragging = false;
            image.raycastTarget = true;
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
        if (pane != null)
        {
            Destroy(pane);
            pane = null;
        }
        display = false;
        //Set the slot to its proper spot in the list of children
        transform.SetSiblingIndex(childIndex);
    }
}
