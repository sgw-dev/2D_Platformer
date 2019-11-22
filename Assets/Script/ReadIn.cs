using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadIn : MonoBehaviour
{

    List<Items> items = new List<Items>();
    // Start is called before the first frame update
    void Start()
    {
        string filepath = System.IO.Path.GetFullPath("Assets/Loot_Spreadsheet.tsv");
        System.IO.StreamReader file = new System.IO.StreamReader(filepath);
        string line = file.ReadLine();
        Items item = this.gameObject.AddComponent<Items>();
        items.Add(item);
        while ((line = file.ReadLine()) != null)
        {
            item = this.gameObject.AddComponent<Items>();
            item.newItems(line);
            items.Add(item);
        }

        file.Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Items getItem(int i) {
        return items[i];
    }
}
