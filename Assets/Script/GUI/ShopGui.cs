using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopGui : MonoBehaviour
{
    public Transform contentContainer;
    public Text moneyText;
    public GameObject slotPrefab;


    bool showSelf = true;
    public void ToggleShop()
    {
        showSelf = !showSelf;
        transform.GetChild(0).gameObject.SetActive(showSelf);
        OnShopUpdate();
    }

    void Start()
    {
        ShopManager.main.onItemsUpdate += OnShopUpdate;
        ShopManager.main.onToggleGui += ToggleShop;
        ToggleShop();
    }

    void OnShopUpdate()
    {
        ClearGui();
        CreateSlots();
        UpdateCurrentMoney();
    }
    
    void UpdateCurrentMoney()
    {
        moneyText.text = ShopManager.main.money.ToString();
    }


    void ClearGui()
    {
        for(int i = 0; i < contentContainer.childCount; i++)
        {
            Destroy(contentContainer.GetChild(i).gameObject);
        }
    }

    void CreateSlots()
    {
        for(int i = 0; i < ShopManager.main.purchasable_items.Count; i++)
        {
            ShopManager.ShopItem item = ShopManager.main.purchasable_items[i]; //Get object shop's items

            //Create GUI object
            GameObject obj = Instantiate(slotPrefab) as GameObject;

            //Update prefab to match item
            obj.name = "Slot: " + i.ToString();
            UpdateSlotGameObject(item, obj);
            int slotId = i;
            obj.GetComponent<Button>().onClick.AddListener(() => ShopManager.main.PurchaseItem(slotId));

            //Move GameObject into scene
            obj.transform.SetParent(contentContainer);
        }
    }

    //Dynamically update GUI object values based upon ShopItem struct
    void UpdateSlotGameObject(ShopManager.ShopItem item, GameObject obj)
    {
        //Check each child object
        for(int i = 0; i < obj.transform.childCount; i++)
        {
            //Test if name corresponds to a field of the struct
            var field = item.GetType().GetField(obj.transform.GetChild(i).gameObject.name);
            if(field != null)
            {
                //Checks if the value should have a text component
                if(field.FieldType == typeof(int) || field.FieldType == typeof(float) || field.FieldType == typeof(string))
                {
                    string text_to_display = "";
                    if(field.FieldType == typeof(string))
                    {
                        text_to_display = (string)field.GetValue(item);
                    }
                    else
                    {
                        //Convert floats and ints to strings
                        text_to_display = field.GetValue(item).ToString();
                    }

                    //Update text component if it exists
                    Text textObj = obj.transform.GetChild(i).GetComponent<Text>();
                    if(textObj != null)
                    {
                        textObj.text = text_to_display;
                    }
                }
                //Checks if the value should have a SpriteRenderer component
                else if (field.FieldType == typeof(Sprite))
                {
                    //Update SpriteRenderer component
                    SpriteRenderer renderer = obj.transform.GetChild(i).GetComponent<SpriteRenderer>();
                    if(renderer != null)
                    {
                        renderer.sprite = (Sprite)field.GetValue(item);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        

        //Debug Press `K` to add test item to shop
        if (Application.isEditor)
        {
            if(Input.GetKeyDown(KeyCode.K))
            {
                ShopManager.ShopItem item = new ShopManager.ShopItem();
                item.ID = 5;
                item.name = "Test Item";
                item.type = "DevType";
                item.cost = 5;
                ShopManager.main.AddItemToShop(item);
                Debug.Log("Added " + item.name + " to shop");
                
            }
        }
    }
}
