using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour {
	
	private string savefilename = "CharacterInventory";
	
	string _f;

	void Update() {
		if (Input.GetKeyUp(KeyCode.KeypadMinus)) {
			SaveData();
		}
		if (Input.GetKeyUp(KeyCode.KeypadEnter)) {
			Debug.Log("ENTER");
			GameObject overlord = GameObject.Find("OverLord");
			Items[] items = overlord.GetComponents<Items>();
			foreach (Items i in items) {
				Debug.Log(JsonUtility.ToJson(i));
			}
		}
		if (Input.GetKeyUp(KeyCode.KeypadPlus)) {
			GameObject overlord = GameObject.Find("OverLord");
			Items[] items = overlord.GetComponents<Items>();
			foreach (Items i in items) {
				int count= 100;
				Debug.Log(i.ID+":"+count );
			}
		}
	}

	string PlayerStats() {
		return "STATS";
	}
	string PlayerInventory() {
		string itemsininv="";
		Inventory inv = GameObject.Find("Character").transform.Find("Inventory").GetComponent<Inventory>();
		GameObject[] slots = inv.slot;
		foreach (GameObject g in slots ) {
			GameObject slot = g.GetComponent<Slot>().item;
			if (slot!=null) {
				itemsininv+="\t"+slot.GetComponent<Items>().ID;//probably terrible performance
				//try finding string builder later
				//Debug.Log(slot.GetComponent<Items>().ID);
			}
		}

		return "INVENTORY\n"+itemsininv;
	}

	void Start() {
		_f = System.IO.Path.GetFullPath("Assets/"+savefilename);
		Debug.Log(_f);
	}

	public void SaveData() {
		System.IO.StreamWriter file = new System.IO.StreamWriter(_f);
		file.Write(PlayerStats()+"\n"+PlayerInventory());
		file.Close();
	}

	//NOT DONE
	public void ReadData() {
		//System.IO.StreamReader file = new System.IO.StreamReader(_f);
	}

}
