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
        StartCoroutine(LateStart(.1f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //Your Function You Want to Call
        //Debug.Log(id);
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
