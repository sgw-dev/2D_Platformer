using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour
    , IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler // 2
{
    public GameObject       hoverPrefab;//The prefab for the Description Popup

    private Item            item;
    private Vector3       location;//Starting Location
    private bool            display;//If the Description Popup is showing

    private GameObject       inventory;//The location where the Pane will Be made
    private GameObject      pane;//Destription Popup
    private GameObject       overLord;//Refrence to Overlord Object

    private Image            image;//Refrence to the image component of the box
    private bool            dragging;//If the object is being dragged

    private bool            hovering;//If the mouse is on the slot; used to show the item description
    public int childIndex;//The proper index location of the child


    void Start()
    {
        inventory = GameObject.Find("Canvas/Inventory/BoxesPanel");
        overLord = GameObject.Find("OverLord");
        image = this.GetComponent<Image>();
        location = this.transform.position;
        childIndex = transform.GetSiblingIndex();
    }
    public Slot()
    {
        
    }
    public void Update()
    {
        if (Input.GetButtonDown("Fire2") && hovering && !display && item != null)
        {
            pane = Instantiate(hoverPrefab) as GameObject;
            pane.transform.SetParent(inventory.transform);
            pane.GetComponent<RectTransform>().localPosition = this.GetComponent<RectTransform>().localPosition + new Vector3(115, -55, 0);
            pane.GetComponentInChildren<Text>().text = item.getName() + "\n\n" + item.getDescription();
            display = true;
        }
        if (dragging)
        {
            Debug.Log("Dragging");
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //Make the thing you are dragging ontop of everything else
            transform.SetSiblingIndex(transform.childCount - 1);
        }
        /*else
        {

            if (!transform.position.Equals(location.position))
            {
                transform.position = new Vector2(Mathf.Lerp(Input.mousePosition.x, location.position.x, .75f), Mathf.Lerp(Input.mousePosition.y, location.position.y, .75f));
            }
        }
        */
    }
    
    public bool isEmpty()
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
    public void addItem(int id)
    {
        item = overLord.GetComponent<ReadIn>().getItem(id);
        image.sprite = item.getIcon();
    }
    public void removeItem()
    {
        item = null;
        image.sprite = null;
    }
    public void OnPointerClick(PointerEventData eventData) // 3
    {
        /*if (!display && item != null && !dragging)
        {
            pane = Instantiate(hoverPrefab) as GameObject;
            pane.transform.SetParent(inventory.transform);
            pane.GetComponent<RectTransform>().localPosition = this.GetComponent<RectTransform>().localPosition + new Vector3(115, -55, 0);
            pane.GetComponentInChildren<Text>().text = item.getName() + "\n\n" + item.getDescription();
            display = true;
        }
        else
        {
            if(pane != null)
            {
                Destroy(pane);
                pane = null;
            }
            display = false;
        }*/
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Dragging");
        if (!Input.GetButton("Fire2"))
        {
            dragging = true;
        }
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Not Dragging");
        transform.position = location;
        dragging = false;
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
