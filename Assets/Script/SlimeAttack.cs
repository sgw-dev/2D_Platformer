using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttack : MonoBehaviour
{
    
    // Start is called before the first frame update
    private CircleCollider2D attackCollider;
    private LayerMask hitMask;

    private float attackTimer;
    public float attackTime;

    public float attackDamage;

    void Start()
    {
        attackCollider = this.gameObject.GetComponentInChildren<CircleCollider2D>();
        hitMask = LayerMask.GetMask("PlayerLayer");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        attackTimer += Time.deltaTime;
        
            if (attackTimer > attackTime)
            {
                attack();
                //Attack animation
                
                //Debug.Log("Attacked");
                attackTimer = 0.0f;
            }
        
    }
    private void attack()
    {
        Collider2D myCollider = attackCollider;
        int numColliders = 10;
        Collider2D[] colliders = new Collider2D[numColliders];
        ArrayList names = new ArrayList();
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.layerMask = hitMask;
        // Set you filters here according to https://docs.unity3d.com/ScriptReference/ContactFilter2D.html
        int colliderCount = myCollider.OverlapCollider(contactFilter, colliders);
        for (int i = 0; i < numColliders; i++)
        {
            if (colliders[i] != null)
            {
                
                if (colliders[i].tag.CompareTo("Player") == 0)
                {
                    if (!names.Contains(colliders[i].name))
                    {
                        colliders[i].SendMessage("applyDamage", attackDamage);
                        names.Add(colliders[i].name);
                        Debug.Log("Got Player");
                    }
                }

            }
        }
    }
}
