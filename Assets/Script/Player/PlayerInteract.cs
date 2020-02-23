using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public GameObject currentInteractableObj = null;
    public Collider2D interactCollider;
    public LayerMask interactMask;

    //Interation script when button is pressed in range of interactable
    void Update()
    {
        if (Input.GetButtonDown("Interact") && currentInteractableObj)
        {
            // Call Loot Table Script;
            currentInteractableObj.SendMessage("DoInteraction");
        }
        if (Input.GetButtonDown("Interact")) {
            Collider2D myCollider = interactCollider;
            int numColliders = 10;
            Collider2D[] colliders = new Collider2D[numColliders];
            ArrayList names = new ArrayList();
            ContactFilter2D contactFilter = new ContactFilter2D();
            contactFilter.layerMask = interactMask;
            // Set you filters here according to https://docs.unity3d.com/ScriptReference/ContactFilter2D.html
            int colliderCount = myCollider.OverlapCollider(contactFilter, colliders);
            for (int i = 0; i < numColliders; i++)
            {
                if (colliders[i] != null)
                {
                    if (colliders[i].tag.CompareTo("Wizard") == 0)
                    {
                        if (!names.Contains(colliders[i].name))
                        {
                            colliders[i].SendMessage("talkToPlayer");
                            names.Add(colliders[i].name);
                        }
                    }

                }
            }
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
