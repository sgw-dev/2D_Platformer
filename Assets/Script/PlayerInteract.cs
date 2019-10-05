using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public GameObject currentInteractableObj = null;

    //Interation script when button is pressed in range of interactable
    void Update()
    {
        if (Input.GetButtonDown("Interact") && currentInteractableObj)
        {
            // Call Loot Table Script;
            currentInteractableObj.SendMessage("DoInteraction");
        }
    }

    //Saves object as current interactable object
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("InteractableObj"))
        {
            Debug.Log(other.name);
            currentInteractableObj = other.gameObject;
        }
    }

    //When out of range of interactable object, interactable set to null
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("InteractableObj"))
        {
            if (other.gameObject == currentInteractableObj)
            {
                currentInteractableObj = null;
            }
        }
    }
}
