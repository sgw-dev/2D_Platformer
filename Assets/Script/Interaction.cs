using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject[] LOOT_TABLE;
    public GameObject tospawn;

    void Start()
    {
        // maybe some code to populate the array LOOT_TABLE
    }

    //Spawns object and hides interactable
    public void DoInteraction()
    {
        //Loot table and putting in inventory
        SpawnLootAt(tospawn.transform.position + new Vector3(-1, 0, 0), 0);
        gameObject.SetActive(false);
    }

    //returns loot depending on place in table
    private GameObject RandomLoot(int tier)
    {
        return LOOT_TABLE[tier];
    }

    //spawns loot at a given location from the loot table
    public void SpawnLootAt(Vector2 position, int tier)
    {
        GameObject.Instantiate(RandomLoot(tier), position, Quaternion.identity);
    }
}
