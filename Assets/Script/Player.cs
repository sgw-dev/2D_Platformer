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
    public float verticalSpeed;//how strong your jumps are
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
    private bool attacking = false;

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

    public float attackSpeed;
    public GameObject arm;
    public GameObject body;
    private bool facingRight = true;
    private float armPos;

    // Start is called before the first frame update
    void Start()
    {
        //gets the rigid body on this object
        rb = this.GetComponent<Rigidbody2D>();

        bottomT = bottom.GetComponent<Transform>();
        rightT = right.GetComponent<Transform>();
        leftT = left.GetComponent<Transform>();

        healthbar = GameObject.Find("Canvas/HealthBar").GetComponent<Slider>();
        arm = GameObject.Find("Arm");
        arm.SetActive(false);
        armPos = arm.transform.localPosition.x;
        body = GameObject.Find("Character/Body");
        body.GetComponent<Renderer>().enabled = false;

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
                //Debug.DrawLine(this.transform.position, new Vector3(this.transform.position.x + attackCollider.radius, this.transform.position.y, this.transform.position.z), Color.red, .25f);
                //Debug.DrawRay(this.transform.position, Vector3.right, Color.red, 1f);
                //Debug.Log("Attacked");
                attackTimer = 0.0f;
            }
        }
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
        Debug.Log("Attack");
        StartCoroutine(attackAnimation());
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
                yield return new WaitForSeconds(attackSpeed);
            }
        }
        else {
            arm.transform.Rotate(new Vector3(0, 0, -135));
            yield return new WaitForSeconds(.1f);
            float step = 45;
            for (float i = -135; i <= -45f; i += step)
            {
                arm.transform.Rotate(new Vector3(0, 0, step));
                yield return new WaitForSeconds(attackSpeed);
            }
        }
        
        arm.SetActive(false);
        body.GetComponent<Renderer>().enabled = false;
        arm.transform.eulerAngles = new Vector3(0, 0, 0);
        this.gameObject.GetComponent<Renderer>().enabled = true;
        playerAnim.speed = 1;
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
