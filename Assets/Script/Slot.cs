using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour
    , IPointerClickHandler , IBeginDragHandler , IEndDragHandler // 2
{
    public GameObject       hoverPrefab;//The prefab for the Description Popup

    private Item            item;
    private Transform       location;//Starting Location
    private bool            display;//If the Description Popup is showing

    public GameObject       inventory;//The location where the Pane will Be made
    private GameObject      pane;//Destription Popup
    public GameObject       overLord;//Refrence to Overlord Object

    public Image            image;//Refrence to the image component of the box
    private bool            dragging;


    void Start()
    {
        inventory = GameObject.Find("Canvas/Inventory/BoxesPanel");
        overLord = GameObject.Find("OverLord");
        image = this.GetComponent<Image>();
        location = this.transform;
    }
    public Slot()
    {
        
    }
    public void Update()
    {
        if(dragging)
        {
            Debug.Log("Dragging");
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        else
        {
            
            if (!transform.position.Equals(location.position))
            {
                transform.position = new Vector2(Mathf.Lerp(Input.mousePosition.x, location.position.x, .75f), Mathf.Lerp(Input.mousePosition.y, location.position.y, .75f));
            }
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
        if (!display && item != null)
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
        }
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //dragging = true;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = true;
    }
}
