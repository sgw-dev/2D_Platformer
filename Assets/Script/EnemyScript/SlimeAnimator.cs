using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimator : MonoBehaviour
{

    private bool inAir;
    private bool grounded;
    public float force;
    public Collider2D feet;
    public LayerMask interactMask;
    public Animator slimeAnim;
    // Start is called before the first frame update
    void Start()
    {
        grounded = true;
        inAir = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.G)) {
            slimeAnim.SetBool("Jumped", true);
        }else if (Input.GetKeyDown(KeyCode.H))
        {
            slimeAnim.SetBool("Jumped", false);
        }


        /*if (!grounded && checkGround()) {
            grounded = true;
            slimeAnim.SetBool("Jumped", false);
        }*/
    }
    public bool checkGround() {
        Collider2D myCollider = feet;
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
                return true;

            }
        }
        return false;
    }
}
