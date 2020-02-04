using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Author: Spencer Burke

    public static Player main;
    private Rigidbody2D rb;//the rigid body attached to the gameobject
    private float speed; //how much force is added each time
    private float maxSpeed; //how fast you are alowed to go
    private float sprint;
    private float verticalSpeed;//how strong your jumps are
    public bool frozen = false;
    public float attackSpeed;
    private float attackTimer = 0.0f;
    private CapsuleCollider2D attackCollider;
    private Animator playerAnim;
    //private bool running = false;

    public float maxHealth;
    public float health;
    private Slider healthbar;
    private float healthWidth;
    private float maxWidth;

    public GameObject deathCanvas;
    public GameObject deathVisual;

    private GameObject bottom;
    private Transform bottomT;
    private Transform rightT;
    private Transform leftT;

    public LayerMask collisionMask;
    public LayerMask hitMask;

    private float checkDistance;
    private int numJumps;
    private float horizSpeed;

    private float armSpeed;
    public GameObject arm;
    public GameObject body;
    private bool facingRight = true;
    private float armPos;

    public bool attacking = false;
    public bool dead = false;

    public Collider2D[] colliders;

    private void Awake()
    {
        //Let main Player be called as `Player.main`
        main = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Initalise Variables
        speed = 400;
        maxSpeed = 5.5f;
        sprint = 3;
        verticalSpeed = 450;
        //attackSpeed = .5f;
        checkDistance = .11f;
        numJumps = 2;
        horizSpeed = 800;
        armSpeed = .02f;

        attackCollider = GameObject.Find("Character/Attack").GetComponent<CapsuleCollider2D>();
        playerAnim = this.GetComponent<Animator>();
        bottom = GameObject.Find("Character/Grounder");
        bottomT = bottom.GetComponent<Transform>();
        rightT = GameObject.Find("Character/Right").GetComponent<Transform>();
        leftT = GameObject.Find("Character/Left").GetComponent<Transform>();


        //gets the rigid body on this object
        rb = this.GetComponent<Rigidbody2D>();

        healthbar = GameObject.Find("Canvas/HealthBar").GetComponent<Slider>();
        arm = GameObject.Find("Arm");
        arm.SetActive(false);
        armPos = arm.transform.localPosition.x;
        body = GameObject.Find("Character/Body");
        body.GetComponent<Renderer>().enabled = false;
        this.gameObject.GetComponent<Renderer>().enabled = true;
        deathVisual.GetComponent<Renderer>().enabled = false;

        health = maxHealth;
        maxWidth = healthbar.maxValue;
        healthWidth = maxWidth;
        healthbar.value = healthWidth;

        colliders = GetComponents<Collider2D>();
        

    }

    public void ToggleFrozen()
    {
        frozen = !frozen;
    }

    void FixedUpdate()
    {
        if(!frozen)
        {
            Movement();
        }
    }

    // Manages player horizontal movement
    void Movement()
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
                body.GetComponent<SpriteRenderer>().flipX = false;
                if (!facingRight) {
                    flipArm(true);
                    facingRight = true;
                }
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
                body.GetComponent<SpriteRenderer>().flipX = false;
                if (!facingRight)
                {
                    flipArm(true);
                    facingRight = true;
                }
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
                body.GetComponent<SpriteRenderer>().flipX = true;
                if (facingRight)
                {
                    flipArm(false);
                    facingRight = false;
                }

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
                if(body == null)
                {
                    Start();
                }
                body.GetComponent<SpriteRenderer>().flipX = true;
                if (facingRight)
                {
                    flipArm(false);
                    facingRight = false;
                }
            }
            playerAnim.SetBool("running", true);
        }
        else
        {
            playerAnim.SetBool("running", false);
        }

        

    }
    public void Update()
    {
        if (!dead) { 
        attackTimer += Time.deltaTime;
        
        //if Fire1 pressed
        if (Input.GetButtonDown("Fire1"))
        {
            if (attackTimer > attackSpeed)
            {
                attack();
                attacking = true;
                
                attackTimer = 0.0f;
            }
        }
        
        if (checkBottom())
        {
            numJumps = 2;
            playerAnim.SetBool("grounded", true);
        }
        else
        {
            playerAnim.SetBool("grounded", false);
        }
        //if jump pressed
        if (Input.GetButtonDown("Jump") & !frozen)
        {
            //add force up

            if (checkRight())
            {
                rb.AddForce(new Vector2(-(horizSpeed * 10), horizSpeed * 5.5f));
                //playerAnim.SetTrigger("jump");
            }
            else if (checkLeft())
            {
                rb.AddForce(new Vector2((horizSpeed * 10), horizSpeed * 5.5f));
                //playerAnim.SetTrigger("jump");
            }
            else if (numJumps > 0)
            {
                rb.AddForce(new Vector2(0.0f, verticalSpeed * 10));
                playerAnim.SetTrigger("jump");
                numJumps--;
            }
        }
        }
    }

    private void flipArm(bool faceRight) {
        //flip arm
        if (faceRight)
        {
            arm.transform.localPosition = new Vector3(armPos, arm.transform.localPosition.y, arm.transform.localPosition.z);
            arm.transform.localScale = new Vector3(-arm.transform.localScale.x, arm.transform.localScale.y, arm.transform.localScale.z);
        }
        else {
            arm.transform.localPosition = new Vector3(0.0f, arm.transform.localPosition.y, arm.transform.localPosition.z);
            arm.transform.localScale = new Vector3(-arm.transform.localScale.x, arm.transform.localScale.y, arm.transform.localScale.z);
        }
    }
    private void attack()
    {
        StartCoroutine(attackAnimation());
        Collider2D myCollider = attackCollider;
        int numColliders = 10;
        Collider2D[] colliders = new Collider2D[numColliders];
        ArrayList names = new ArrayList();
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.layerMask = hitMask;
        // Set you filters here according to https://docs.unity3d.com/ScriptReference/ContactFilter2D.html
        //int colliderCount = myCollider.OverlapCollider(contactFilter, colliders);
        int colliderCount = Physics2D.OverlapCollider(attackCollider, contactFilter, colliders);
        for (int i = 0; i < numColliders; i++)
        {
            if (colliders[i] != null)
            {
                if (colliders[i].tag.CompareTo("Enemy") == 0 || colliders[i].tag.CompareTo("Owl")==0)
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
    IEnumerator attackAnimation()
    {
        playerAnim.speed = 0;
        this.gameObject.GetComponent<Renderer>().enabled = false;
        body.GetComponent<Renderer>().enabled = true;
        arm.SetActive(true);
        if (facingRight)
        {
            arm.transform.Rotate(new Vector3(0, 0, 135));
            yield return new WaitForSeconds(.1f);
            float step = 45;
            for (float i = 135; i >= 45f; i -= step)
            {
                arm.transform.Rotate(new Vector3(0, 0, -step));
                yield return new WaitForSeconds(armSpeed);
            }
        }
        else {
            arm.transform.Rotate(new Vector3(0, 0, -135));
            yield return new WaitForSeconds(.1f);
            float step = 45;
            for (float i = -135; i <= -45f; i += step)
            {
                arm.transform.Rotate(new Vector3(0, 0, step));
                yield return new WaitForSeconds(armSpeed);
            }
        }
        
        arm.SetActive(false);
        body.GetComponent<Renderer>().enabled = false;
        arm.transform.eulerAngles = new Vector3(0, 0, 0);
        this.gameObject.GetComponent<Renderer>().enabled = true;
        playerAnim.speed = 1;
        attacking = false;
        StopCoroutine(attackAnimation());
    }

    public bool checkRight()
    {
        RaycastHit2D hit = Physics2D.Raycast(rightT.transform.position, Vector2.right, checkDistance, collisionMask);
        if (hit.collider != null)
        { return true; }
        else
        { return false; }
    }
    public bool checkLeft()
    {
        RaycastHit2D hit = Physics2D.Raycast(leftT.transform.position, Vector2.left, checkDistance, collisionMask);
        if (hit.collider != null)
        { return true; }
        else
        { return false; }
    }
    public bool checkBottom()
    {
        if(bottom == null)
        {
            Start();
        }
        
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
        playerAnim.SetTrigger("hit");
        health -= damage;
        if (health <= 0 && !dead)
        {
            die();
            dead = true;
        }
        float ratio = maxWidth / maxHealth;
        float width = healthbar.value;
        healthbar.value = width - (ratio * damage);
    }
    public void die()
    {
        Time.timeScale = 0f;
        //make player walk through-able
        foreach(Collider2D c in colliders)
        {
            c.enabled = false;
        }
        this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        deathCanvas.SetActive(true);
        this.gameObject.GetComponent<Renderer>().enabled = false;
        deathVisual.GetComponent<Renderer>().enabled = true;
    }
    public bool getAttacking()
    {
        return attacking;
    }
    public void updateHealthBar()
    {
        float ratio = maxWidth / maxHealth;
        float width = healthbar.value;
        healthbar.value = width;
    }
}
