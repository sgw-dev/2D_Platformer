using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour
    , IPointerClickHandler // 2
{
    private Item item;
    private Transform location;
    public GameObject hoverPrefab;
    private GameObject inventory;
    private bool display;
    private GameObject pane;

    public Slot()
    {

    }
    public void Awake()
    {
        inventory = GameObject.Find("Canvas/Inventory/BoxesPanel");
    }
    public void OnPointerClick(PointerEventData eventData) // 3
    {
        if (!display)
        {
            pane = Instantiate(hoverPrefab) as GameObject;
            pane.transform.SetParent(inventory.transform);
            pane.GetComponent<RectTransform>().localPosition = this.GetComponent<RectTransform>().localPosition + new Vector3(115, -55, 0);
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
}
