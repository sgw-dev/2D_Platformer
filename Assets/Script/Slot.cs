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

    public Slot()
    {

    }
    public void OnPointerClick(PointerEventData eventData) // 3
    {
        print("I was clicked");
        
    }
}
