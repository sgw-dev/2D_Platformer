using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Save to file CharacterInventory
 * 
 */
public class PlayerSave : MonoBehaviour {

	public string savefilename = "CharacterSave";
	string _f;

	public int[] items;
	private int[] stats;

	void Start() {
		_f = System.IO.Path.GetFullPath(savefilename);
		//_f = System.IO.Path.GetFullPath("Assets/"+savefilename);
	}
	public void SaveData() {
		System.IO.StreamWriter file = null;
		try {
			file = new System.IO.StreamWriter(_f);
			file.Write(PlayerStats()+"\n"+PlayerInventory());
			file.Flush();
		} catch(Exception e) {
			Debug.LogError(e.Message);
		} finally {
			if(file!=null) {
				file.Close();
			}
		}

	}

	public void LoadSave() {
		System.IO.StreamReader file = null;
		String items;
		String stats;
		try {
			file = new System.IO.StreamReader(_f);
			stats = file.ReadLine();
			items = file.ReadLine();
		} catch(Exception e) {
			Debug.LogError(e.Message);
		} finally {
			if(file!=null) {
				file.Close();
			}
		}

	}

	string PlayerStats() {
		//place real code here
		return "1 2 3 4";
	}

	string PlayerInventory() {
		//place real code here
		return "5 6 7 8";
	}

	//remove update once done testing
	void Update() {
		if (Input.GetKeyUp(KeyCode.KeypadMultiply)){ 
			Debug.Log("*");
			SaveData();
		}
		if (Input.GetKeyUp(KeyCode.KeypadMinus)) {
			Debug.Log("-");
			LoadSave();
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

}
