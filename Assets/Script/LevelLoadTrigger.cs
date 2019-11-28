using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/**
 * Greg
 * Generic trigger Object to load the next scene
 * there is a flag if we want the player to hit e to load the level
 * 
 * When adding to each level be sure to put the name of the scene in LOAD_LEVEL
 * if it is an interactive load check f_IsInteractLoad
 * 
 */
public class LevelLoadTrigger : MonoBehaviour {

	public string LOAD_LEVEL;

	[Tooltip("Mark if we want the character to press e to initiate a scene load")]
	public bool f_IsInteractLoad;
	bool inTrigger;//true when in the trigger, false once out
	
	bool sceneLoadTriggered;

	[Tooltip("Check if wanting to load stuff into the next scene")]
	public bool CallFunctionOnLoad;
	[Tooltip("Place an object to load stuff")]//nothing implmented yet
	public GameObject loadObject;

	void Start() {

		if(LOAD_LEVEL==null||LOAD_LEVEL.Equals("")) {
			Debug.LogError("Indicate the Scene to load please.");
		}
		if (CallFunctionOnLoad && loadObject==null) {
			Debug.LogError("Set the loadObject");
		}
		inTrigger=false;//lets not set the chrarcter in the trigger on scene load...
		sceneLoadTriggered = false;

	}

	//just start the coroutine if the flag is set
	void Update() {
		if (Input.GetKeyDown(KeyCode.E) && inTrigger) {
			sceneLoadTriggered=true;
		}

		if(sceneLoadTriggered) {
			Debug.LogFormat("{0} {1}",
				"Preparing to load level : ",
				LOAD_LEVEL);
			sceneLoadTriggered=false;
			StartCoroutine(LoadAndTransferAsyncScene());
		}

	}
	//Coroutine to load
	IEnumerator LoadAndTransferAsyncScene() {
		Scene currentScene = SceneManager.GetActiveScene();// Set the current Scene to be able to unload it later
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(LOAD_LEVEL, LoadSceneMode.Additive);// The Application loads the Scene in the background at the same time as the current Scene.
		while (!asyncLoad.isDone) {// Wait until the last operation fully loads to return anything
			yield return null;
		}

		//scene is loaded at this point, last called before everything in last scene is Destroyed
		if (CallFunctionOnLoad) {
			TEST_FUNCTION();
		}
		// Unload the previous Scene
		SceneManager.UnloadSceneAsync(currentScene);
	}

	void TEST_FUNCTION() {
		//loadObject.GetComponent<SOMESCRIPT_INTERFACE>().LOAD();
	}

	////////////////Collider stuff//////////////////////
	void OnTriggerEnter2D(Collider2D col) {
		if (f_IsInteractLoad) {//mark that the character is in the trigger and can load
			inTrigger=true;
		} else {
			if(col.gameObject.name.Equals("Character")) {
				sceneLoadTriggered=true;
			}
		}
	}

	void OnTriggerExit2D(Collider2D col) {

		if (f_IsInteractLoad) {
			inTrigger=false;
		}
	}

}
/**
		//move objects in the list to the new scene
		for (int i = 0 ; i < move_GameObjects.Length ; i++ ) {
			SceneManager.MoveGameObjectToScene(
				move_GameObjects[i], 
				SceneManager.GetSceneByName(LOAD_LEVEL));// Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
		}
 */
