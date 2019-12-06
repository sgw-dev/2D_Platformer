using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour {
	
	public string savefilename = "CharacterInventory";
	
	string _f;


	[Tooltip("ITEMS HERE")]
	public int[] items;
	private int[] stats;

	void Update() {
		if (Input.GetKeyUp(KeyCode.KeypadMultiply)){ 
			SaveData();
		}
		if (Input.GetKeyUp(KeyCode.KeypadMinus)) {
			GetSaveData();
		}
		/*
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
		}*/
	}

	string PlayerStats() {
		return "STATS :\t";
	}

	string PlayerInventory() {
		string itemsininv="";
		Inventory inv = GameObject.Find("Character").transform.Find("Inventory").GetComponent<Inventory>();
		GameObject[] slots = inv.Slots;
		foreach (GameObject g in slots ) {
			GameObject slot = g.GetComponent<Slot>().item;
			if (slot!=null) {
				itemsininv+=slot.GetComponent<Items>().ID+"\t";//probably terrible performance
				//try finding string builder later
				//Debug.Log(slot.GetComponent<Items>().ID);
			}
		}

		return "INVENTORY :\t"+itemsininv;
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

	public void GetSaveData() {
		ReadData();
	}
	
	public void ReadData() {

		System.IO.StreamReader file = new System.IO.StreamReader(_f);
		
		if (file==null) {
			Debug.LogError("NO SAVE FOUND");
		}
		
		string input ;//= file.ReadLine();
		string itemstring="";
		List<int> items = new List<int>();
		
		try{
			while( (input = file.ReadLine()) != null ) {
				if (input.Contains("STATS")) {
					//next line contains stats
				}
				if (input.Contains("INVENTORY")) {
					//items
					itemstring = input.Split(':')[1];
				}
			}
		
			string[] _is = itemstring.Split('\t');

			for (int i = 0 ; i < _is.Length ; i++) {
				try{
					int n = Int32.Parse(_is[i]);
					items.Add(n);
				} catch(Exception e) {
					//just catching bad parses here
				}
			}

		} catch (Exception e) {
			Debug.LogError("BAD SAVE FILE");
			Debug.LogError(e.Message);
		} finally {
			file.Close();//close the file...
		}
		
		this.items = items.ToArray();
	}

}
