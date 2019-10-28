using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Author: Spencer Burke
    
    private Rigidbody2D rb;//the rigid body attached to the gameobject
    public float speed; //how much force is added each time
    public float maxSpeed; //how fast you are alowed to go
    public float sprint;
    public float verticalSpeed;//how strong you jumps are
    public float attackTime;
    private float attackTimer = 0.0f;
    public Collider2D attackCollider;

    public GameObject stage;
    public GameObject bottom;
    public GameObject right;
    public GameObject left;
    private Transform bottomT;
    private Transform rightT;
    private Transform leftT;

    public LayerMask collisionMask;
    public LayerMask hitMask;

    public float checkDistance;
    private int numJumps = 1;
    public float horizSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //gets the rigid body on this object
        rb = this.GetComponent<Rigidbody2D>();

        bottomT = bottom.GetComponent<Transform>();
        rightT = right.GetComponent<Transform>();
        leftT = left.GetComponent<Transform>();
    }

    // Update is called once per frame

    void FixedUpdate() {
        RaycastHit2D hit = Physics2D.Raycast(bottom.transform.position, Vector2.down, checkDistance, collisionMask);
        if (hit.collider != null) {
            numJumps = 1;
        }
    }
    void Update()
    {

        if (Input.GetButtonDown("Fire3")) {
            Debug.Log("Fire3"); 
        }/*
        if (Input.GetButtonUp("Fire3"))
        {
            maxSpeed -= sprint;
        }*/
        //if D pressed and sprint
        if (Input.GetAxisRaw("Horizontal") >0.0f && Input.GetButton("Fire3")
            || Input.GetButton("Fire3") && Input.GetAxisRaw("Horizontal") > 0.0f) {
            //if the current x velocity is less that the max alowed
            if (rb.velocity.x < (maxSpeed + sprint))
            {
                //add force to the object to the right
                rb.AddForce(new Vector2(speed, 0.0f));
                //flip the sprite to face right
                this.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        //if D pressed
        if (Input.GetAxisRaw("Horizontal") > 0.0f )
        {
            //if the current x velocity is less that the max alowed
            if (rb.velocity.x < maxSpeed)
            {
                //add force to the object to the right
                rb.AddForce(new Vector2(speed, 0.0f));
                //flip the sprite to face right
                this.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        //if A pressed and sprint
        if (Input.GetAxisRaw("Horizontal") < 0.0f && Input.GetButton("Fire3")
            || Input.GetButton("Fire3") && Input.GetAxisRaw("Horizontal") <  0.0f)
        {
            //is the current x velocity is greater than negative max alowed
            //this is becuase you can be going fast backwards too
            if (rb.velocity.x > -(maxSpeed + sprint))
            {
                //add force to the object to the left
                rb.AddForce(new Vector2(-speed, 0.0f));
                //flip the sprite to face left
                this.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        //if A pressed
        if (Input.GetAxisRaw("Horizontal") < 0.0f)
        {
            //is the current x velocity is greater than negative max alowed
            //this is becuase you can be going fast backwards too
            if (rb.velocity.x > -maxSpeed)
            {
                //add force on the object to the left
                rb.AddForce(new Vector2(-speed, 0.0f));
                //flip the sprite to face left
                this.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        
        //if jump pressed
        if (Input.GetButtonDown("Jump"))
        {
            //add force up
            if (checkRight()) {
                rb.AddForce(new Vector2(-(horizSpeed * 10), horizSpeed*5.5f));
            } else if (checkLeft())
            {
                rb.AddForce(new Vector2((horizSpeed * 10), horizSpeed*5.5f));
            }
            else if (numJumps > 0) {
                rb.AddForce(new Vector2(0.0f, verticalSpeed*10));
                numJumps--;
            }
        }
        attackTimer += Time.deltaTime;
        //if Fire1 pressed
        if (Input.GetButtonDown("Fire1"))
        {           
            if (attackTimer > attackTime)
            {
                attack();
                //Debug.Log("Attacked");
                attackTimer = 0.0f;
            }          
        }

    }
    private void attack() {
        Collider2D myCollider = attackCollider;
        int numColliders = 10;
        Collider2D[] colliders = new Collider2D[numColliders];
        ArrayList names = new ArrayList();
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.layerMask = hitMask;
        // Set you filters here according to https://docs.unity3d.com/ScriptReference/ContactFilter2D.html
        int colliderCount = myCollider.OverlapCollider(contactFilter, colliders);
        for (int i = 0; i < numColliders; i++) {
            if (colliders[i] != null) {
                if (colliders[i].tag.CompareTo("Enemy") == 0) {
                    if (!names.Contains(colliders[i].name))
                    {
                        colliders[i].SendMessage("applyDamage", 1);
                        names.Add(colliders[i].name);
                    }
                }
                
            }
        }
    }

    public bool checkRight() {
        RaycastHit2D hit = Physics2D.Raycast(rightT.transform.position, Vector2.right, checkDistance, collisionMask);
        if (hit.collider != null)
        {
            return true;
        }
        else {
            return false;
        }
    }
    public bool checkLeft()
    {
        RaycastHit2D hit = Physics2D.Raycast(leftT.transform.position, Vector2.left, checkDistance, collisionMask);
        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
