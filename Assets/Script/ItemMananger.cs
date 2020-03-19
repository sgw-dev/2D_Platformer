using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMananger : MonoBehaviour
{
    public int min = 1;
    public int max = 10;//exclusive
    public int id;
    public Item item;
    private GameObject overLord;
    public ReadIn read;

    // Start is called before the first frame update
    void Start()
    {
        overLord = GameObject.Find("OverLord");

        id = Random.Range(min, max);
        read = overLord.GetComponent<ReadIn>();
        Debug.Log(id);
        item = read.getItem(id);
    }
    public Item getItem()
    {
        return item;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
