using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public CircleCollider2D attackCollider;
    public Animator playerAnim;
    private bool running = false;

    public float maxHealth;
    private float health;
    public Slider healthbar;
    private float healthWidth;
    private float maxWidth;

    public GameObject attackVisual;

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
    public int numJumps = 2;
    public float horizSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //gets the rigid body on this object
        rb = this.GetComponent<Rigidbody2D>();

        bottomT = bottom.GetComponent<Transform>();
        rightT = right.GetComponent<Transform>();
        leftT = left.GetComponent<Transform>();

        healthbar = GameObject.Find("Canvas/HealthBar").GetComponent<Slider>();

        health = maxHealth;
        maxWidth = healthbar.maxValue;
        healthWidth = maxWidth;

    }

    void FixedUpdate()
    {

        //if D pressed and sprint
        if (Input.GetAxisRaw("Horizontal") > 0.0f && Input.GetButton("Fire3")
            || Input.GetButton("Fire3") && Input.GetAxisRaw("Horizontal") > 0.0f)
        {
            //if the current x velocity is less that the max alowed
            if (rb.velocity.x < (maxSpeed + sprint))
            {
                //add force to the object to the right
                rb.AddForce(new Vector2(speed, 0.0f));
                //flip the sprite to face right
                this.GetComponent<SpriteRenderer>().flipX = false;
            }
            playerAnim.SetBool("running", true);
        }
        //if D pressed
        else if (Input.GetAxisRaw("Horizontal") > 0.0f)
        {
            //if the current x velocity is less that the max alowed
            if (rb.velocity.x < maxSpeed)
            {
                //add force to the object to the right
                rb.AddForce(new Vector2(speed, 0.0f));
                //flip the sprite to face right
                this.GetComponent<SpriteRenderer>().flipX = false;
            }
            playerAnim.SetBool("running", true);
        }
        //if A pressed and sprint
        else if (Input.GetAxisRaw("Horizontal") < 0.0f && Input.GetButton("Fire3")
            || Input.GetButton("Fire3") && Input.GetAxisRaw("Horizontal") < 0.0f)
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
            playerAnim.SetBool("running", true);
        }
        //if A pressed
        else if (Input.GetAxisRaw("Horizontal") < 0.0f)
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
            playerAnim.SetBool("running", true);
        }
        else
        {
            playerAnim.SetBool("running", false);
        }

        attackTimer += Time.deltaTime;
        //hides attack animation
        /*if (attackTimer > .25f && attackVisual.activeSelf == true)
        {
            attackVisual.SetActive(false);
        }*/
        //if Fire1 pressed
        if (Input.GetButtonDown("Fire1"))
        {
            if (attackTimer > attackTime)
            {
                attack();
                //Attack animation
                Debug.DrawLine(this.transform.position, new Vector3(this.transform.position.x + attackCollider.radius, this.transform.position.y, this.transform.position.z), Color.red, .25f);
                //Debug.DrawRay(this.transform.position, Vector3.right, Color.red, 1f);
                //Debug.Log("Attacked");
                attackTimer = 0.0f;
            }
        }

    }
    public void Update()
    {
        if (checkBottom())
        {
            numJumps = 2;
        }
        //if jump pressed
        if (Input.GetButtonDown("Jump"))
        {
            //add force up

            if (checkRight())
            {
                rb.AddForce(new Vector2(-(horizSpeed * 10), horizSpeed * 5.5f));
            }
            else if (checkLeft())
            {
                rb.AddForce(new Vector2((horizSpeed * 10), horizSpeed * 5.5f));
            }
            else if (numJumps > 0)
            {
                rb.AddForce(new Vector2(0.0f, verticalSpeed * 10));
                numJumps--;
            }
        }
    }

    private void attack()
    {
        //Debug.DrawRay(this.transform.position, Vector3.right, Color.red, .25df);
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
                if (colliders[i].tag.CompareTo("Enemy") == 0)
                {
                    if (!names.Contains(colliders[i].name))
                    {
                        colliders[i].SendMessage("applyDamage", 1.0f);
                        names.Add(colliders[i].name);
                    }
                }

            }
        }
    }

    public bool checkRight()
    {
        RaycastHit2D hit = Physics2D.Raycast(rightT.transform.position, Vector2.right, checkDistance, collisionMask);
        if (hit.collider != null)
        {
            return true;
        }
        else
        {
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
    public bool checkBottom()
    {
        Collider2D myCollider = bottom.GetComponent<Collider2D>();
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
                if (colliders[i].tag.CompareTo("Stage") == 0)
                {
                    return true;
                }

            }
        }
        return false;
    }
    public void applyDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            die();
        }
        float ratio = maxWidth / maxHealth;
        float width = healthbar.value;
        healthbar.value = width - (ratio * damage);
    }
    public void die()
    {

    }
}
