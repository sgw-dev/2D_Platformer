using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlMovement : FlyingEnemyPathing
{ /*
     * By: Parker Allen
     * Ver: 1.0
     * 
     * Expieremental!!
     * 
     * Setup:
     * 
     * jumpPower = 200?
     * speedX = dosen't matter
     * maxSpeedX = dosen'tmatter
     * searchdistance = 10
     * 
     * searchAngle = 270
     * degBetweenSearches = 5
     * searchDistanceWall = 1
     * minNumberOfSearches = 1
     * 
     * perch = required
     * 
     * points = don't use
     * 
     * sprite = child object with spriteRenderer
     * 
     * dive Position = 8, 2
     * dive speed = 500?
     * dive time = 1?
     * 
     * Rigidbody:
     * mass = 1
     * gravity = 0
     * 
     *  1 2DBoxCollider
     */

    public Vector2 divePosition;        //distance from the player to dive from
    public float diveSpeed;             //speed of dive
    public float diveTime;              //duration of dive

    private Vector2 owlDirection;      //direction to move
    private bool diving, sitting;            //if diving or sitting

    //Spencer
    private Animator anim;
    private SpriteRenderer OwlSprite;
    public bool attack;
    public bool attackFlag = true;
    private Player playerScript;

    /*******************************************************************************************************************/

    //Spencer
    public void Start() {
        EnemyStart();
        anim = this.GetComponentInChildren<Animator>();
        OwlSprite = this.GetComponentInChildren<SpriteRenderer>();
        jumpPower = 200;
        searchDistance = 10;
        searchAngle = 270;
        degBetweenSearches = 5;
        searchDistanceWall = 1;
        minNumOfSearches = 1;
        divePosition = new Vector2(8, 2);
        diveSpeed = 500;
        diveTime = 1;

        //Spencer
        perch = this.transform.position;
        attack = false;

        playerScript = playerPosition.GetComponent<Player>();
    }
    public void FixedUpdate()
    {
        attack = anim.GetBool("Attack");
        if (!diving)
        {
            anim.SetBool("See", true);
            owlDirection = Vector2.zero;
            rb.velocity = owlDirection;     //zeros the rigidbodies velocity
            if (lineOfSight() && !playerScript.dead)                              //can see player?
            {
                PrepareToDive();                            //prepares to dive
            }
            else if (perch != Vector3.zero)                 //has perch?
            {
                owlDirection = Perch();                        //direction = call to perch
            }
            if (owlDirection != Vector2.zero)                  //direction is not 0
            {
                Move(owlDirection, jumpPower);                 //move direction
            }
            else
            {
                sitting = true;                             //Sit!
                anim.SetBool("See", false);
            }
        }
        LockRotation();                                     //lock the rotation
        //Debug.DrawLine(transform.position, direction, Color.cyan);
    }

    /*******************************************************************************************************************/
    //moves into position to conemnce dive attack
    private void PrepareToDive()
    {
        //position to dive from
        point.x = playerPosition.position.x + ((transform.position.x < playerPosition.position.x) ? -divePosition.x : divePosition.x);
        point.y = playerPosition.position.y + divePosition.y;

        if (withinBound(point))
        {
            //checks if player is on the right or left to flip sprite
            if ((playerPosition.position.x - transform.position.x) > 0)
            {
                OwlSprite.flipX = true;
            }
            else {
                OwlSprite.flipX = false;
            }
            StartCoroutine(DiveAttack());       //DIVE! DIVE! DIVE!
        }
        else
        {
            owlDirection = point;                  //move to position to dive from
        }
    }

    /*******************************************************************************************************************/

    IEnumerator DiveAttack()
    {
        anim.SetBool("Attack", true);
        Move(target, diveSpeed);                        //move to target at dive speed
        diving = true;                                  //it is diving
        yield return new WaitForSeconds(diveTime);      //wait
        diving = false;                                 //not diving
        anim.SetBool("Attack", false);
        StopCoroutine(DiveAttack());                    //end Coroutine
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (attack && other.CompareTag("Player") && attackFlag)
        {
            
            other.SendMessage("applyDamage", 2f);
            attackFlag = false;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("AttackBouble"))
        {
            
            Player script = other.gameObject.GetComponentInParent<Player>();
            if (script.attacking)
            {
                Debug.Log("Ouch!");
                this.gameObject.GetComponent<EnemyHealth>().applyDamage(1f);
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player") && !attackFlag)
        {

            attackFlag = true;
        }
    }

}