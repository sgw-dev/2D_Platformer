using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject InvMenu;
    public GameObject BoxesPanel;
    public GameObject[] Slots;
    public bool[] isFull;
	public GameObject itemSlotImage;

	private Slot[] slotsScripts;
    private int InvSize = 18;

    // Start is called before the first frame update
    void Start()
    {
        Slots = new GameObject[18];
        isFull = new bool[18];
		slotsScripts = new Slot[18];
        for (int i = 0; i < 18; i++)
        {
            Slots[i] = BoxesPanel.transform.GetChild(i).gameObject;
            isFull[i] = false;
			GameObject tmp = GameObject.Instantiate(itemSlotImage,Slots[i].transform);
			Slots[i].AddComponent<Slot>();

			Slots[i].GetComponent<Slot>().slotIconGO = tmp.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            InvMenu.SetActive(!InvMenu.activeSelf);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Item")
        {
            GameObject itemPickedUp = other.gameObject;
            Items item = itemPickedUp.GetComponent<Items>();

            AddItem(itemPickedUp, item.ID, item.type, item.description, item.icon);
        }
    }
    void AddItem(GameObject itemObject, int itemId, string itemType, string itemDescription, Sprite itemIcon)
    {
        for(int i = 0; i < InvSize; i++)
        {
            if( ! isFull[i] )
            {
                Slots[i].GetComponent<Slot>().item = itemObject;
                Slots[i].GetComponent<Slot>().icon = itemIcon;
                Slots[i].GetComponent<Slot>().type = itemType; 
                Slots[i].GetComponent<Slot>().description = itemDescription;
                Slots[i].GetComponent<Slot>().ID = itemId;

                itemObject.transform.parent = Slots[i].transform;
                itemObject.SetActive(false);

                Slots[i].GetComponent<Slot>().UpdateSlot();
                isFull[i] = true;
                return;
            }
        }
    }
}