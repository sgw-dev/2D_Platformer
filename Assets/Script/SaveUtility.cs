using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  How to use this class
 *  Just call the public Save or Load with the 
 *  script you want to save, or load, values for.
 *  
 *  CAREFUL :
 *  THIS WILL SAVE EVERY FLOAT STRING INT IN A CLASS PROVIDED.
 *  AS WELL AS LOAD EVERY FLOAT STRING AND INT TOT THE CLASS PROVIDED.
 *  
 *  As an example, the kepad minus and mulitply will respectively Load and Save 
 *  the int float and string values in the class provided, Player by the GameObject
 *  set in the scene.
 *  
 *  The class provided to the methods does not need to extend Monobehavior.
 *  
 */

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

	[Tooltip("Make Sure the instance attached to the player is here")]
	public GameObject player;

	void Start() {
        
    }

    void Update() {
		#if UNITY_EDITOR
			if(Input.GetKeyDown(KeyCode.KeypadMinus)) {
				Load<Player>(player.GetComponent<Player>());
			}
			if (Input.GetKeyDown(KeyCode.KeypadMultiply)) {
				Save<Player>(player.GetComponent<Player>());
			}
		#endif
    }

	/**
	 * Save one thing?
	 * No way, save it all!
	 * string,float, int
	 */
	public void Save<T>(T t) {
		SaveFields(t);
	}

	/*
	 * Load one thing?
	 */
	public void Load<T>(T t) {
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
//garbage below


	/*
	void SaveFields<T>(T c) {
		Assembly info = typeof(T).Assembly;
		MemberInfo[] memberInfo;
		Type save_type = c.GetType();
		memberInfo = save_type.GetMembers();
		for(int i = 0 ; i < memberInfo.Length ; i++ ) {
			//Debug.Log(string.Format("'{0}' is a {1}", memberInfo[i].Name, memberInfo[i].MemberType));
		}

		FieldInfo[] fields = save_type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
		for (int i = 0 ; i < fields.Length ; i++ ) {
			Debug.Log(string.Format(
				"Name            : {0}\n" +
	            "Declaring Type  : {1}\n" +
				"IsPublic        : {2}\n" +
				"MemberType      : {3}\n" +
				"FieldType       : {4}\n" +
				"IsFamily        : {5}\n" +
				"VALUE           : {6}\n"
									,fields[i].Name
									,fields[i].DeclaringType
									,fields[i].IsPublic
									,fields[i].MemberType
									,fields[i].FieldType
									,fields[i].IsFamily
									,fields[i].GetValue(c)
				)
			);
		}

		Debug.Log(info);
	}*/

				/*Debug.Log(string.Format(
				"Name            : {0}\n" +
	            "Declaring Type  : {1}\n" +
				"IsPublic        : {2}\n" +
				"MemberType      : {3}\n" +
				"FieldType       : {4}\n" +
				"IsFamily        : {5}\n" +
				"VALUE           : {6}\n"
									,fields[i].Name
									,fields[i].DeclaringType
									,fields[i].IsPublic
									,fields[i].MemberType
									,fields[i].FieldType
									,fields[i].IsFamily
									,fields[i].GetValue(c)
				)
			);*/