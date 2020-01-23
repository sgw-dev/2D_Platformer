using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Simple Trigger to load scene
 * two strings, the scene to load (must be spelled correctly)
 * and the object name that can cause the trigger (this will most likely always
 * be "Character")
 * 
 * Also includes a boolean trigger_on_key_press to change the use of the object
 * 
 * 
 */

[RequireComponent(typeof(Collider2D))]
public class SceneTrigger : MonoBehaviour {

	[Tooltip("The scene to load when cause enters the trigger")]
	public string scene_to_load;
	[Tooltip("What object can trigger this load")]
	public string cause;
	[Tooltip("Check if the scene should be loaded on a key press")]
	public bool trigger_on_key_press;
	
	bool causeintrigger;

	public void OnTriggerEnter2D(Collider2D c) {
		if (c.name.Equals(cause)&&!trigger_on_key_press) {
			SceneManager.LoadScene(scene_to_load);
		}
	}

	public void OnTriggerStay2D(Collider2D c) {
		if (c.name.Equals(cause)&&trigger_on_key_press) {
			causeintrigger=true;
		}
	}
	public void OnTriggerExit2D(Collider2D c) {
		if (c.name.Equals(cause)&&trigger_on_key_press) {
			causeintrigger=false;
		}
	}
	void Update() {
		if (trigger_on_key_press &&
			causeintrigger && 
			Input.GetKeyDown(KeyCode.E) ) 
		{
			SceneManager.LoadScene(scene_to_load);
		}
	}

	void Start() {
		causeintrigger = false;
	}

}
