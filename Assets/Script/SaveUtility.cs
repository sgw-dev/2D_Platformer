using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
FYI
https://docs.unity3d.com/ScriptReference/PlayerPrefs.html
On Windows, PlayerPrefs are stored in the registry under HKCU\Software\[company name]\[product name] key, where company and product names are the names set up in Project Settings.
On Linux, PlayerPrefs can be found in ~/.config/unity3d/[CompanyName]/[ProductName] again using the company and product names specified in the Project Settings.
On Windows Store Apps, Player Prefs can be found in %userprofile%\AppData\Local\Packages\[ProductPackageId]>\LocalState\playerprefs.dat
On Windows Phone 8, Player Prefs can be found in application's local folder, See Also: Directory.localFolder
On Android data is stored (persisted) on the device. The data is saved in SharedPreferences. C#/JavaScript, Android Java and Native code can all access the PlayerPrefs data. The PlayerPrefs data is physically stored in /data/data/pkg-name/shared_prefs/pkg-name.xml.
On WebGL, PlayerPrefs are stored using the browser's IndexedDB API.
On iOS, PlayerPrefs are stored in /Library/Preferences/[bundle identifier].plist.
 */

/*
 * Note there is a difference between the unity editor 
 * and an actual build location in terms of the window's registry.
 */
public class SaveUtility : MonoBehaviour {
	
	static readonly string MAXHP = "playermaxhealth";
	static readonly string CURHP = "playercurrenthealth";
	static readonly string GOLD  = "playergold";
	//location of gameobject and script subject to change
	static readonly string INV   = "inventoryslot_";
	//not sure where this gameobject/script is yet
	static readonly string EQUIP = "playerequipslot_";

	[Tooltip("Make Sure the instance attached to the player is here")]
	public GameObject      player;
	Player                 playerscript;
	Inventory              inventoryscript;

	void Start() {
        playerscript    = player.GetComponent<Player>();
		inventoryscript = player.GetComponentInChildren<Inventory>();
		if(playerscript ==null || inventoryscript == null) {
			Debug.LogError("Cannot save missing components from Player GameObject,\n"+
				"Player    : " + playerscript+"\n"+
				"Inventory : " + inventoryscript+"\n");
		}
	}

    void Update() {
		//Code for the editor to test
		//In deployed build these should not be included by the compiler
		#if UNITY_EDITOR
			if(Input.GetKeyDown(KeyCode.KeypadMinus)) {
				Debug.Log("SAVING");
				SaveGame();
				//Load<Player>(player.GetComponent<Player>());
			}
			if (Input.GetKeyDown(KeyCode.KeypadMultiply)) {
				Debug.Log("LOADING");
				LoadGame();
				//Save<Player>(player.GetComponent<Player>());
			}
		#endif
    }

	public void SaveGame() {
		PlayerPrefs.SetFloat(MAXHP,playerscript.maxHealth);
		PlayerPrefs.SetFloat(CURHP,playerscript.health);
		
		//try is here until item system and inventory is set
		try {
			/*for(int i = 0 ; i < inventoryscript.Slots.Length ; i++) {
				PlayerPrefs.SetInt(INV+i,inventoryscript.Slots[i].GetComponent<Slot>().ID);
			}*/
		} catch(Exception e) {
			Debug.LogError(e.Message);
		}
		//PlayerPrefs.SetFloat(GOLD,-1);
		PlayerPrefs.Save();
	}

	public void LoadGame() {
		if(PlayerPrefs.HasKey(MAXHP)) {
			playerscript.maxHealth = PlayerPrefs.GetFloat(MAXHP);
		}
		if(PlayerPrefs.HasKey(CURHP)) {
			playerscript.health    = PlayerPrefs.GetFloat(CURHP);
		}
		//try is here until item system and inventory is set
		try {
			/*for(int i = 0 ; i < inventoryscript.Slots.Length ; i++) {
				if(PlayerPrefs.HasKey(INV+i)) {
					inventoryscript.Slots[i].GetComponent<Slot>().ID = PlayerPrefs.GetInt(INV+i);
				}
			}*/
		} catch(Exception e) {
			Debug.LogError(e.Message);
		}
		if(PlayerPrefs.HasKey(GOLD)) {
			//code to set gold
		}
	}


/////////////////////////////////////////////////////////////////////////////////
//////Everything below here is just for an example of reflection/////////////////
/////////////////////////////////////////////////////////////////////////////////	
	/**
	 * Save one thing?
	 * No way, save it all!
	 * string,float, int
	 */
	[Obsolete("Save is deprecated, please use GameSave instead.")]
	void Save<T>(T t) {
		SaveFields(t);
	}

	/*
	 * Load one thing?
	 */
	[Obsolete("Load is deprecated, please use LoadGame instead.")]
	void Load<T>(T t) {
		LoadFields(t);
	}

	void LoadFields<T>(T t) {
		Type save_type = typeof(T);
		FieldInfo[] fields = save_type.GetFields(
			BindingFlags.Public | 			
			BindingFlags.NonPublic | 
			BindingFlags.Instance);
		for (int i = 0 ; i < fields.Length ; i++ ) {
			if (fields[i].FieldType == typeof(int)) {
				if(PlayerPrefs.HasKey(fields[i].Name)) {
					int l = PlayerPrefs.GetInt(fields[i].Name);
					//Debug.Log(fields[i].Name+" : "+l);
					fields[i].SetValue(t,l);
				}
			} else if(fields[i].FieldType == typeof(float)) {
				if(PlayerPrefs.HasKey(fields[i].Name)) {
					float l = PlayerPrefs.GetFloat(fields[i].Name);
					//Debug.Log(fields[i].Name + " : " + l);
					fields[i].SetValue(t,l);
				}
			} else if(fields[i].FieldType == typeof(double)) {
				//Debug.Log("double");
			} else if(fields[i].FieldType == typeof(char)) {
				//Debug.Log("char");
			} else if(fields[i].FieldType == typeof(bool)) {
				//Debug.Log("bool");
			} else if(fields[i].FieldType == typeof(long)) {
				//Debug.Log("Long");
			} else if(fields[i].FieldType == typeof(string)) {
				if(PlayerPrefs.HasKey(fields[i].Name)) {
					string l = PlayerPrefs.GetString(fields[i].Name);
					fields[i].SetValue(t,l);
				}
			} else if(fields[i].FieldType == typeof(int[])) {
				//Debug.Log("Int array");
			}
			
		}
		#if UNITY_EDITOR
			Debug.Log("LoadFields Complete for " + save_type.Name);
		#endif
	}

	void SaveFields<T>(T c) {
	
		Type save_type = c.GetType();
		//bit mask flags?
		FieldInfo[] fields = save_type.GetFields(
			BindingFlags.Public | //public fields
			BindingFlags.NonPublic |  //private fields
			BindingFlags.Instance); //instance/internal

		for (int i = 0 ; i < fields.Length ; i++ ) {
			if (fields[i].FieldType == typeof(int)) {
				//Debug.Log("int: " + fields[i].Name);
				PlayerPrefs.SetInt(fields[i].Name,(int)fields[i].GetValue(c));
			} else if(fields[i].FieldType == typeof(float)) {
				//Debug.Log("float : " + fields[i].Name);
				PlayerPrefs.SetFloat(fields[i].Name,(float)fields[i].GetValue(c));
			} else if(fields[i].FieldType == typeof(double)) {
				//Debug.Log("double: " + fields[i].Name);
			} else if(fields[i].FieldType == typeof(char)) {
				//Debug.Log("char: " + fields[i].Name);
			} else if(fields[i].FieldType == typeof(bool)) {
				//Debug.Log("bool: " + fields[i].Name);
			} else if(fields[i].FieldType == typeof(long)) {
				//Debug.Log("Long");
			} else if(fields[i].FieldType == typeof(string)) {
				//Debug.Log("String: " + fields[i].Name);
				PlayerPrefs.SetString(fields[i].Name,(string)fields[i].GetValue(c));
			} else if(fields[i].FieldType == typeof(int[])) {
				//Debug.Log("Int array : " + fields[i].Name);
			}
		}
		#if UNITY_EDITOR
			Debug.Log("SaveFields Complete for " + save_type.Name);
		#endif
	}
}