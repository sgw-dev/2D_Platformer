using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
/**
 * 
 * 
 * 
 * Save to file 2d_PlatformerSave
 * this file containts a static classes that deals
 * with the file io
 * 
 * To add data to be changed 
 * 1.) Add the data type to the _2dPlatformerSaveData class
 * 2.) Set the data to be saved in the SaveData() below
 * 3.) Update any scripts that read the _2dPlatformerSaveData from LoadData()
 * 
 * Note: Unity doesnt like serializing MonoBehavior's
 *       So only non MonoBehavior's can be serialized in the _2dPlatformerSaveData class
 *       One solution is to use ScriptableObject's
 *		 If not using that make sure in SaveData() there is a way to map the data to 
 *		 something that is serializable. (Also for readin the data out of _2dPlatformerSaveData)
 */

public class PlayerSave : MonoBehaviour {
	
	public string savefilename = "2d_PlatformerSave";

	public void SaveData() {
		Debug.Log(Application.persistentDataPath);
		//construct the dataobject
		_2dPlatformerSaveData sd = new _2dPlatformerSaveData();
		
		//change where the data comes from
		sd.its = GetPlayerInventory();
		sd.stats = GetPlayerStats();//new PlayerStats(1000,2000,3000
		Saver.CreateSave(savefilename,sd);
	}

	public _2dPlatformerSaveData LoadSave() {
		return Saver.Load(savefilename);
	}

	PlayerStats GetPlayerStats() {

		//TODO : Add GameObject.Find("Character").getstats
		//and return that instead of this hardcoded value
		return new PlayerStats(1000,2000,3000); 
	}

	List<string> GetPlayerInventory() {
		//may want to find a better way to serialize items
		//replace the iterable in the for loop with the inventory, or
		//whatever returns the Players owned Items
		List<string> itemlist = new List<string>();
		foreach(Items i in GameObject.Find("OverLord").GetComponents<Items>()) {//<=here
			itemlist.Add(mapItem(i));
			Debug.Log("SAVE : "+mapItem(i));
		}
		return itemlist;
	}


	#if UNITY_EDITOR
	//remove update once done testing
	void Update() {
		if (Input.GetKeyUp(KeyCode.KeypadMultiply)){ 
			Debug.Log("*");
			SaveData();
		}
		if (Input.GetKeyUp(KeyCode.KeypadMinus)) {
			Debug.Log("-");
			_2dPlatformerSaveData DATA = LoadSave();
			foreach(string i in DATA.its) {
				Debug.Log(i);
			}
			Debug.Log(DATA.stats.ToString());
			Debug.Log("Load Finished");
		}
	}
	#endif
	public string mapItem(Items i) {
	   return i.ID +"|"+
        i.itemName +"|"+
        i.type +"|"+
        i.subType +"|"+
        i.value +"|"+
        i.secValue +"|"+
        i.description;
	}
}



/// <summary>
/// Class to serialize save data
/// for future use, just add the script or data
/// </summary>

[System.Serializable]
public class _2dPlatformerSaveData { 

	public List<string> its;
	public PlayerStats stats;
	
	public _2dPlatformerSaveData() {
		its = new List<string>();
		stats = null;
	}

}

/// <summary>
/// Class to perform IO
/// </summary>
public static class Saver { 
	public static void CreateSave(string savefilename,_2dPlatformerSaveData d) {//Items[] items,Player playerstat) {
		BinaryFormatter formatter = new BinaryFormatter();
		string file = Application.persistentDataPath + "\\"+ savefilename;
		FileStream stream = new FileStream (file,FileMode.Create);
		//put the data
		formatter.Serialize(stream,d);
		stream.Close();
	}

	public static _2dPlatformerSaveData Load(string savefilename) {
		string file = Application.persistentDataPath + "\\"+ savefilename;
		if(File.Exists(file)) {
			BinaryFormatter formatter =  new BinaryFormatter();
			FileStream stream  = new FileStream(file,FileMode.Open);
			_2dPlatformerSaveData readdata = formatter.Deserialize(stream) as _2dPlatformerSaveData;
			stream.Close();
			return readdata;	
		} else {
			Debug.Log("No save file found");
			return null;
		}
	}
}