using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BossPathing2))]
[RequireComponent(typeof(CharacterMovement))]
public class BossMovement : MonoBehaviour
{
    /*
     * By: Parker Allen
     * Version: 1.0
     */

    private BossPathing2 bossPathing;                //boss pathing
    private ContactFilter2D playerFilter;           //player filter
    private CharacterMovement characterMovement;    //character movement

    public float jumpHeight;        //jump height
    public float gravity;           //gravity *negative value
    public float jumpTime;          //time used to reach jump height
    private float maxJumpSpeed;     //max jump speed

    [HideInInspector] public int maxNumberOfJumps, numberOfJumps;     //number of jumps and the max number of jumps

    public float walkSpeed, sprintSpeed;    //walking speed and sprinting speed

    public Vector3 direction;       //direction
    public Vector3 velocity;        //velocity

    [HideInInspector] public bool walking, sprinting, jumping, falling;   //state of object
    [HideInInspector] public int moveType;                                //int to tell how object should move
    //0 = stop  1 = move on ground  2 or more = jump

    [HideInInspector] public bool canAttack;          //cooldown between different attacks
    [HideInInspector] public bool stopToAttack;       //stop moving to attack
    private float attackCooldown;   //multiplyier for time between attacks

    public int maxHealth;   //max health
    private int health;     //health

    /**************************************************************************************************/
    //initialize some things
    public void BossMovementStart()
    {
        bossPathing = GetComponent<BossPathing2>();
        playerFilter = new ContactFilter2D();
        playerFilter.SetLayerMask(bossPathing.playerMask);

        characterMovement = GetComponent<CharacterMovement>();
        characterMovement.StartCharacterSkeleton();

        maxNumberOfJumps = 1;
        maxJumpSpeed = jumpHeight;
        bossPathing.SetJumpHieght(jumpHeight);

        canAttack = true;
        attackCooldown = 2;
    }

    /**************************************************************************************************/
    //calculates velocity to move horizontaly
    //*call after calculating move type
    public void HorizontalMovment()
    {
        if(characterMovement.sides.bot && !(jumping || falling))    //on the ground and not jumping or falling
        {
            if(moveType > 0 && !stopToAttack)   //move type over 0 and not sotping to attack
            {
                float disToPlayer = Mathf.Abs(transform.position.x - bossPathing.GetPoint().x);     //distance to player

                if(bossPathing.chasing && disToPlayer > 10)     //boss is chasing player and player further than 10 away
                {
                    velocity.x = sprintSpeed;   //set x of velocity to sprint speed
                    sprinting = true;           //is sprinting
                }
                else
                {
                    velocity.x = walkSpeed;     //set x of velocity to walking speed
                    walking = true;             //is waliking
                }
            }
            else
            {
                velocity.x = 0;     //else set x of velocity to 0
            }
            numberOfJumps = maxNumberOfJumps;   //reset number of jumps
        }

        if(moveType == 0)       //if move type equals 0
        {
            walking = sprinting = false;    //sprinting and walking equal false
        }

        velocity.x *= direction.x;      //set x of veloctiy in direction
    }

    /**************************************************************************************************/
    //calculates velocity to move verticly returns if jumping
    //*call after Horizontal movement
    public bool VerticalMovement()
    {
        bool jumped = false;    //is it jumping

        if(moveType >= 2 && numberOfJumps > 0)      //move type greater than 1 and number of jumps greater than 0
        {
            velocity = Jump(bossPathing.GetPoint());    //calulate velocity to jump
            jumped = true;                              //it is jumping
        }
        else if(characterMovement.sides.bot || characterMovement.sides.top)     //hits ground or ceiling
        {
            velocity.y = 0;                 //set y of velocity to 0
        }
        else
        {
            falling = velocity.y < 0;       //falling if y of velcity is less than 0
        }
        velocity.y += gravity * Time.deltaTime;     //add gravity to y of velocity

        return jumped;      //return jumped
    }

    /*******************************************************************************************************************/
    //checks a distance for a wall 1 = no wall, 2 = can jump over wall, and 0 = cna't move forward
    public int checkForWall(float distance)
    {
        Vector2 rayOrigin = transform.position + direction * bossPathing.myBounds.xhalf + Vector3.up * bossPathing.myBounds.yhalf;  //ray origin at the right or left side of object
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, distance, bossPathing.collisionMask);        //cast ray left or right a distance
        //Debug.DrawRay(rayOrigin, direction * distance, Color.red);

        if (hit && !bossPathing.InYBounds(bossPathing.GetPoint()))      //hits a wall and point not in y bounds
        {
            return jumpAboveWall(hit.collider.bounds.max.y);        //try to jump over wall
        }
        else
        {
            return 1;
        }
    }

    /*******************************************************************************************************************/
    //checks if object can jump over wall
    private int jumpAboveWall(float height)
    {
        if (height < jumpHeight && numberOfJumps > 0)      //can jump high enough and has jumps
        {
            //Debug.Log("1");       //debug what is causeing the jump
            return 2;               //it can
        }
        return 0;
    }

    /*******************************************************************************************************************/
    //checks if hole infront of object 1 = no hole, 2 = can jump over hole, and 0 = can't move forward
    public int checkForHole(float distance, Vector3 p)
    {
        Vector2 rayOrigin = transform.position + direction * distance;      //ray origin left or right at a distance from object
        float rayLength = jumpHeight + bossPathing.myBounds.yhalf;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, bossPathing.collisionMask);       //cast ray down from orign
        //Debug.DrawRay(rayOrigin, Vector2.down * rayLength, Color.red);

        if (hit)
        {
            return 1;       //no hole keep going
        }
        else
        {
            return jumpOverPit(p);      //hole try to jump over it
        }
    }

    /*******************************************************************************************************************/
    //checks if object can jump over pit
    private int jumpOverPit(Vector3 p)
    {
        float t = jumpTime;
        t += Mathf.Sqrt(2 * (p.y - (transform.position.y + jumpHeight)) / gravity);     //time in the air

        bool reach = Mathf.Abs(p.x - transform.position.x) / sprintSpeed <= t;     //air time * maxspeedx is greart than ledge point is on
        if (reach && numberOfJumps > 0)
        {
            //Debug.DrawRay(transform.position, direction * t * sprintSpeed, Color.yellow);
            //Debug.Log("2");       //debug what is causeing the jump
            return 2;           //it can make it
        }
        return 0;
    }

    /**************************************************************************************************/
    //return int based on if needs to jump up to point
    public int PointAbove(Vector3 p)
    {
        if (!characterMovement.sides.climbing)  //not climbing slope
        {
            if(transform.position.y + bossPathing.myBounds.yhalf < p.y && Mathf.Abs(p.x - transform.position.x) < walkSpeed * jumpTime)    //point above and close
                return CanJumpUpToPoint(p);
        }
        return 1;   //doesn't need to jump
    }

    /**************************************************************************************************/
    //return int if it can jump to point
    public int CanJumpUpToPoint(Vector3 p)
    {
        if(p.y - transform.position.y < jumpHeight)
        {
            //Debug.Log("3");       //debug what is causeing the jump
            return 2;   //it can
        }
        return 0;   //it can't
    }

    /**************************************************************************************************/
    //calculate move type
    public void CalculateMoveType(float dis)
    {
        moveType = checkForHole(dis, bossPathing.GetPoint());
        if(bossPathing.GetPoint().y > transform.position.y)
        {
            moveType *= checkForWall(dis) * PointAbove(bossPathing.GetPoint());     //only check if point is above position
        }

        //if anything returns 0 it needs to stop
        //if anything returns 2 it needs to jump
        //if everything returns 1 keep going forward

        if (bossPathing.InBounds(bossPathing.GetPoint()))
        {
            bossPathing.NextPoint();    //get next point if hits point
            moveType = 0;               //set move type to 0
        }
    }

    /**************************************************************************************************/
    //caculates the jump power vertically
    public float CalculateJumpHeight(float h, float t)
    {
        return (h - .5f * gravity * (t * t)) / t;
    }

    /**************************************************************************************************/
    //calulates the jump power horizontally
    public float CalculateJumpDistance(float dis, float t)
    {
        return dis / t;
    }

    /**************************************************************************************************/
    //retuns vector of jump
    public Vector3 Jump(Vector3 p)
    {
        numberOfJumps--;    //decreament the number of jumps
        jumping = true;     //set state to jumping

        float h = p.y - transform.position.y;                               //height between this object and point
        float t = Mathf.Abs(p.x - transform.position.x) / sprintSpeed;      //time for jump
        float y = CalculateJumpHeight(h, t);                                //get vetical jump power
        float x = CalculateJumpDistance(p.x - transform.position.x, t);     //get horizontal jump power

        if (y > maxJumpSpeed)   //cap y jump power to max jump speed
        {
            return new Vector3(x, maxJumpSpeed);
        }
        return new Vector3(x, y);
    }

    /**************************************************************************************************/
    //set direction of object based from the rear end of the object
    public void SetDirection(Vector3 p)
    {
        if(transform.position.x + -direction.x * bossPathing.myBounds.xhalf > p.x)
        {
            direction = Vector2.left;
        }
        else
        {
            direction = Vector2.right;
        }
    }

    /**************************************************************************************************/
    //returns direction
    public Vector3 GetDirection()
    {
        return direction;
    }

    /**************************************************************************************************/
    //returns velocity to move after adjustments from obstacles
    public Vector3 GetVelocity()
    {
        return characterMovement.inputMoveSkeleton(velocity * Time.deltaTime);
    }

    /**************************************************************************************************/
    //return the player controller from the inside of a collider
    public Player GetPlayerInCollider(Collider2D collider)
    {
        Collider2D[] temp = new Collider2D[1];
        Physics2D.OverlapCollider(collider, playerFilter, temp);    //check for player in collider
        if (temp[0] != null && temp[0].CompareTag("Player"))
        {
            return temp[0].GetComponent<Player>();    //return player's controller
        }
        return null;    //return null
    }

    /**************************************************************************************************/
    //start cooldown between attacks
    public IEnumerator AttackCooldown(float cc)
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown * cc);   //wait given time times attack cooldown
        canAttack = true;
    }

    /**************************************************************************************************/
    //reset the state of object
    public void ResetState()
    {
        sprinting = !(jumping || falling) && bossPathing.chasing;   //sprinting if chasing player and not falling of jumping
        walking = falling = jumping = false;
        if (velocity.y > 0)
        {
            jumping = true;     //if y of velocity is grater than 0 jumping is true
        }
    }

    public CharacterMovement GetCharacterMovement()
    {
        return characterMovement;
    }

    public void DebugDirection()
    {
        Debug.DrawRay(transform.position, direction, Color.yellow);
    }
}
