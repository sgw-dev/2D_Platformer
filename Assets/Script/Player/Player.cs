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
	public float currentXP;
	public float maxXP;
	public Slider xpbar;
	public int gold;
	private Text goldpanel;

	public int level;
	private Text levelpanel;

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

    private bool facingRight = true;

    public bool attacking = false;
    public bool dead = false;

    private Collider2D[] colliders;
    public Sprite shield;
    public GameObject weapon;
    public GameObject shieldHolder;
    private bool blocking = false;

    private float jumpDelay = .1f;//This is to prevent spaming the jump key;
    private float jumpTimer;


    public float defaultMaxHP = 10f;

    private int attackDamage;
    private int defence;
    private int defaultAttackDamage = 1;
    public Inventory inventory;
    private Text attackText;
    private Text defenceText;

    
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
        verticalSpeed = 11;
        //attackSpeed = .5f;
        checkDistance = .11f;
        numJumps = 2;
        horizSpeed = 7;

        attackCollider = GameObject.Find("Character/Attack").GetComponent<CapsuleCollider2D>();
        playerAnim = GameObject.Find("Character/PlayerBody").GetComponent<Animator>();
        bottom = GameObject.Find("Character/Grounder");
        bottomT = bottom.GetComponent<Transform>();
        rightT = GameObject.Find("Character/Right").GetComponent<Transform>();
        leftT = GameObject.Find("Character/Left").GetComponent<Transform>();


        //gets the rigid body on this object
        rb = this.GetComponent<Rigidbody2D>();

        healthbar = GameObject.Find("Canvas/HealthBar").GetComponent<Slider>();
		xpbar     = GameObject.Find("Canvas/Inventory/CharacterPanel/StatsPanel/XP Slider").GetComponent<Slider>();
		levelpanel= GameObject.Find("Canvas/Inventory/CharacterPanel/StatsPanel/Level").GetComponent<Text>();
		goldpanel = GameObject.Find("Canvas/Inventory/CharacterPanel/StatsPanel/GoldText").GetComponent<Text>();

        shieldHolder.SetActive(false);

        health = maxHealth;
        maxWidth = healthbar.maxValue;
        healthWidth = maxWidth;
        healthbar.value = healthWidth;
		
		currentXP = 0;//change
		maxXP     = 100;//change to some variable later
		xpbar.wholeNumbers = true;//can remove if changing the editor flag
		xpbar.maxValue     = maxXP;
		xpbar.value        = currentXP;

		level = 1;//load level from save
		levelpanel.text = level+"";

		gold = 0;//load from save
		goldpanel.text = gold+"";

        colliders = GetComponents<Collider2D>();
        inventory = GetComponentInChildren<Inventory>();
        attackText = GameObject.Find("Canvas/Inventory/CharacterPanel/StatsPanel/Attack").GetComponent<Text>();
        attackDamage = defaultAttackDamage;
        attackText.text = attackDamage.ToString();
        defenceText = GameObject.Find("Canvas/Inventory/CharacterPanel/StatsPanel/Defence").GetComponent<Text>();
        defence = 0;
        defenceText.text = defence.ToString();
        updateStats();
    }
    public void updateStats()
    {
        //Get Weapon Damage
        Item temp = inventory.getEquiped(4);
        if (temp != null && temp.getId()!= 0)
        {
            attackDamage = temp.getValue();
            weapon.GetComponent<SpriteRenderer>().sprite = temp.getImage();
        }
        else
        {
            attackDamage = defaultAttackDamage;
            weapon.GetComponent<SpriteRenderer>().sprite = null;
        }
        attackText.text = attackDamage.ToString();
        //Get Helmet Defence
        temp = inventory.getEquiped(0);
        defence = 0;
        if (temp != null)
        {
            defence += temp.getValue();
        }
        //Get Chest Defence
        temp = inventory.getEquiped(1);
        if (temp != null)
        {
            defence += temp.getValue();
        }
        //Get Boot Defence
        temp = inventory.getEquiped(2);
        if (temp != null)
        {
            defence += temp.getValue();
        }
        defenceText.text = defence.ToString();
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
        else
        {
            playerAnim.SetBool("running", false);
        }
    }
    void flip(bool left)
    {
        float scaleX = Mathf.Abs(transform.localScale.x);
        if (left)
        {
            scaleX *= -1;
        }
        transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
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
                playerAnim.SetFloat("speed", 1.3f);
                //flip the sprite to face right
                flip(false);
                if (!facingRight) {
                    //flipArm(true);
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
                playerAnim.SetFloat("speed", 1.0f);
                //flip the sprite to face right
                flip(false);
                if (!facingRight)
                {
                    //flipArm(true);
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
                playerAnim.SetFloat("speed", 1.3f);
                flip(true);
                if (facingRight)
                {
                    //flipArm(false);
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
                playerAnim.SetFloat("speed", 1.0f);
                flip(true);
                if (facingRight)
                {
                    //flipArm(false);
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
            jumpTimer += Time.deltaTime;
        
            //if Fire1 pressed
            if (Input.GetButtonDown("Fire1") & !frozen)
            {
                if (attackTimer > attackSpeed)
                {
                    attack();
                    attacking = true;
                
                    attackTimer = 0.0f;
                }
            }
            if (Input.GetButtonDown("Fire2"))
            {
                playerAnim.SetTrigger("blockup");
                blocking = true;
                shieldHolder.SetActive(true);
                weapon.SetActive(false);
                frozen = true;
                //blockUpAnimation();
            }
            if (Input.GetButtonUp("Fire2"))
            {
                playerAnim.SetTrigger("blockdown");
                blocking = false;
                shieldHolder.SetActive(false);
                weapon.SetActive(true);
                frozen = false;
                //StartCoroutine(blockDownAnimation());
            }

            if (checkBottom()){
                numJumps = 2;
                playerAnim.SetBool("grounded", true);
            }
            else{
                playerAnim.SetBool("grounded", false);
            }
            //if jump pressed
            if (Input.GetButtonDown("Jump") & !frozen & (jumpTimer > jumpDelay))
            {
                //add force up

                if (checkRight())
                {
                    rb.velocity = new Vector2(-horizSpeed, (verticalSpeed/1.5f));
                    //rb.AddForce(new Vector2(-(horizSpeed * 10), horizSpeed * 5.5f));
                    //playerAnim.SetTrigger("jump");
                }
                else if (checkLeft())
                {
                    rb.velocity = new Vector2(-horizSpeed, (verticalSpeed / 1.5f));
                    //rb.AddForce(new Vector2((horizSpeed * 10), horizSpeed * 5.5f));
                    //playerAnim.SetTrigger("jump");
                }
                else if (numJumps > 0)
                {
                    //Trying to directly set the player's velocity to give better feel to jump
                    rb.velocity = new Vector2(rb.velocity.x, verticalSpeed);
                    //rb.AddForce(new Vector2(0.0f, verticalSpeed * 10));
                    playerAnim.SetTrigger("jump");
                    numJumps--;
                }
            }
        }
    }
    private void attack()
    {
        //start animation
        //StartCoroutine(attackAnimation());
        playerAnim.SetTrigger("attack");
        //Finding enemies to hit
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
                if (colliders[i].tag.CompareTo("Enemy") == 0 || colliders[i].tag.CompareTo("Owl") == 0)
                {

                    if (!names.Contains(colliders[i].name))
                    {
                        colliders[i].SendMessage("applyDamage", (float)attackDamage);
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
        
        if (!blocking)
        {
            playerAnim.SetTrigger("hit");

            health -= damage/((float)defence + 1f);
            if (health <= 0 && !dead)
            {
                die();
                dead = true;
            }
            float ratio = maxWidth / maxHealth;
            float width = healthbar.value;
            healthbar.value = width - (ratio * damage);
        }
       
        
    }
    public void die()
    {
        playerAnim.SetTrigger("dead");
        Time.timeScale = 0f;
        //make player walk through-able
        foreach(Collider2D c in colliders)
        {
            c.enabled = false;
        }
        this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        deathCanvas.SetActive(true);
        //this.gameObject.GetComponent<Renderer>().enabled = false;
        //deathVisual.GetComponent<Renderer>().enabled = true;
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

	//call this to give the player experience, note only add
	public void AddXP(int amount) {
		
		//dont do this
		if(amount<0) {
			Debug.LogError("Create another method to decrement xp");
		}
		
		currentXP += amount;
	
		if(currentXP<maxXP) {//no level occurs just readjust the slider
			xpbar.value+=currentXP;
		} else  { //levelup is occuring adjust values
			currentXP = 0 + (currentXP-maxXP); //if extra xp is gained count toward next level
			
			LevelUp();

			maxXP=100;//can change at some point to a variable
			xpbar.value=currentXP;
		}
	}
	
	public void LevelUp() {
		level++;
		levelpanel.text=level+"";
		//do other things like adjust stats
	}

	public void GoldTransact(int amount) {
		if(gold+amount < 0 ) {
			Debug.LogWarning("You cannot afford this");
			return;
		}
		gold += amount;
		goldpanel.text=gold+"";
	}

}
