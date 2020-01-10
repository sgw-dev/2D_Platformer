using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPathing : MonoBehaviour
{
    /*
     * By: Parker Allen
     * Version: 1.0
     * 
     */

    public LayerMask collisionMask;             //collision mask for ground
    public LayerMask lineOfSightMask;           //player layer and ground layer
    public LayerMask playerMask;
    private ContactFilter2D contactFilter2D;

    public Transform playerPosition;            //player position
    [HideInInspector] public Bounds bound;      //bounds of the first Collider

    private float maxJumpPower;     //force used to go up
    public float gravity;           //gravity on object
    public float jumpHeight;        //max height of jump
    public float jumpTime;          //time to reach max height in jump

    [HideInInspector] public int maxNumOfJumps;      //max number of jumps
    [HideInInspector] public int numOfJumps;         //max jump height of object

    public float walkSpeed;         //speed for walking
    public float sprintSpeed;       //speed for running
    public Vector3 direction;       // direction the enmy will go

    public float searchDistance;                     //distance enemy will search for player
    [HideInInspector] public Vector3 target;         //player's position
    [HideInInspector] public Vector3 point;          //where the enemy will head next

    public bool sprinting, walking, jumping, falling;   //states of movement

    public bool canAttack;              //if enemy can attack again
    private float attackCooldown;       //base delay between attacks

    public int health, maxHealth;       //health

    /*******************************************************************************************************************/
    public void BossStart()
    {
        bound = GetComponent<BoxCollider2D>().bounds;               //bounds of collider

        contactFilter2D = new ContactFilter2D();
        contactFilter2D.SetLayerMask(playerMask);

        point = PointOnGroundBelow(transform.position);             //set first point below boss

        maxNumOfJumps = 1;                                          //max number of jumps

        maxJumpPower = CalculateJumpPower(jumpHeight, jumpTime);    //calculate max jump power

        canAttack = true;       //can attack
        attackCooldown = 2;     //base attack cooldown is 1
    }

    /*******************************************************************************************************************/
    //generate next point to move to and return if it found a target
    public bool GetWaypoint()
    {
        if (!SetTargetPosition())           //check if it has a target
        {
            if (HitPoint() == 0)
            {
                SetRandomPoint();           //get new random point
            }
            return false;
        }
        return true;
    }

    /*******************************************************************************************************************/
    //sets a random point on the ground
    public void SetRandomPoint()
    {
        Vector3 randomDist = Random.Range(-12, 12) * Vector3.right;         //random distance on x axis
        point = PointOnGroundBelow(transform.position + randomDist);        //place point on the ground
        if (point == Vector3.zero)
        {
            SetRandomPoint();       //try again if point not on ground
        }
    }

    /*******************************************************************************************************************/
    //set target position
    public bool SetTargetPosition()
    {
        if (LineOfSightOnPlayer())
        {
            target = playerPosition.position;               //set target to player position
            Vector3 temp = PointOnGroundBelow(target);      //get target positon on ground below
            if (temp == Vector3.zero)
            {
                return false;                               //return false target not above ground
            }
            point = temp;           //set point to target positon on ground below
            return true;            //return true
        }
        target = Vector3.zero;      //can't see target target = zero
        return false;               //return false
    }

    /*******************************************************************************************************************/
    //returns position on ground below give position
    public Vector3 PointOnGroundBelow(Vector3 rayOrigin)
    {
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, jumpHeight + bound.size.y / 2, collisionMask);     //raycast down from given
        if (hit)
        {
            return hit.point + Vector2.up * (bound.size.y / 2);     //return position slightly above ground
        }
        return Vector3.zero;        //return zero
    }

    /*******************************************************************************************************************/
    //check if it has line of sight on player
    public bool LineOfSightOnPlayer()
    {
        Vector2 dir = (playerPosition.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, searchDistance, lineOfSightMask);       //raycast to target
        //Debug.DrawRay(transform.position, dir * searchDistance, Color.black, 2);

        if (hit && hit.transform.CompareTag(playerPosition.tag))                                        //raycast hit player
        {
            //Debug.DrawLine(transform.position, hit.point, Color.yellow);
            return true;                                                        //has line of sight
        }
        return false;       //no line of sight on target
    }

    /*******************************************************************************************************************/
    //returns if point is in bounds
    public int HitPoint()
    {
        if (XDistanceToPoint() < bound.size.x / 2)
        {
            return 0;       //point is in bounds
        }
        return 1;           //point out of bounds
    }

    /*******************************************************************************************************************/
    //returns the distance from position to point
    public float XDistanceToPoint()
    {
        return Mathf.Abs(point.x - transform.position.x);
    }

    /*******************************************************************************************************************/
    //checks a distance for a wall 1 = no wall, 2 = can jump over wall, and 0 = cna't move forward
    public int checkForWall(float distance)
    {
        Vector2 rayOrigin = transform.position + direction * bound.size.x / 2;                      //ray origin at the right or left side of object
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, distance, collisionMask);        //cast ray left or right a distance
        //Debug.DrawRay(rayOrigin, direction * bound.size.x / 2, Color.yellow);

        if (hit)
        {
            return jumpAboveWall(hit.collider.bounds.max.y);                                        //try to jump over wall
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
        if (height < jumpHeight && numOfJumps > 0)      //can jump high enough and has jumps
        {
            return 2;                                   //it can
        }
        return 0;                                       //can't jump over it
    }

    /*******************************************************************************************************************/
    //checks if hole infront of object 1 = no hole, 2 = can jump over hole, and 0 = can't move forward
    public int checkForHole(float distance)
    {
        Vector2 rayOrigin = transform.position + direction * distance;                                  //ray origin left or right at a distance from object
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, jumpHeight, collisionMask);       //cast ray down from orign
        //Debug.DrawRay(rayOrigin, Vector2.down * jumpHeight, Color.green);

        if (hit)
        {
            return 1;                                                                                   //no hole keep going
        }
        else
        {
            return jumpOverPit();                                                                       //hole try to jump over it
        }
    }

    /*******************************************************************************************************************/
    //checks if object can jump over pit
    private int jumpOverPit()
    {
        RaycastHit2D hit = Physics2D.Raycast(point, Vector2.down, 5, collisionMask);                            //checks for ground under point
        if (hit)
        {
            float t = jumpTime;
            t += Mathf.Sqrt(2 * (hit.point.y - (transform.position.y + jumpHeight)) / gravity);                //time in the air

            bool reach = Mathf.Abs(point.x - transform.position.x) / sprintSpeed <= t;     //air time * maxspeedx is greart than ledge point is on
            if (reach && numOfJumps > 0)
            {
                //Debug.DrawRay(transform.position, direction * t * sprintSpeed, Color.yellow);
                return 2;                                                                                           //it can make it
            }
        }
        return 0;       //can't jump over it
    }

    /*******************************************************************************************************************/
    //calculates the velocity needed to jump given y(height) in given t(time)
    public float CalculateJumpPower(float y, float t)
    {
        return (y - .5f * gravity * (t * t)) / t;
    }

    /*******************************************************************************************************************/
    //jumps to given p(point)
    public float Jump(Vector3 p)
    {
        numOfJumps--;                                                       //decrement number of jumps
        float h = p.y - transform.position.y;                               //height needed to jump
        float t = Mathf.Abs(p.x - transform.position.x) / sprintSpeed;      //amount of time for the jump
        jumping = true;                                                     //set state to jumping
        float rtn = CalculateJumpPower(h, t);                               //calulate velocity
        if (rtn > jumpHeight / jumpTime)
        {
            return jumpHeight / jumpTime;           //return velocity
        }
        return rtn;                                 //return velocity
    }

    /*******************************************************************************************************************/
    //set the direction based on where the point is compared to enemy's position
    public void SetDirectionOnPoint()
    {
        direction = (transform.position.x < point.x) ? Vector2.left : Vector2.right;
    }

    public void SetDirectionOnPlayer()
    {
        direction = (transform.position.x < playerPosition.position.x) ? Vector2.left : Vector2.right;
    }

    public Collider2D GetPlayerColliderInCollider(Collider2D collider)
    {
        Collider2D[] temp = new Collider2D[1];
        Physics2D.OverlapCollider(collider, contactFilter2D, temp);
        if (temp[0] != null && temp[0].CompareTag("Player"))
        {
            return temp[0];
        }
        return null;
    }
    public IEnumerator StartBasicAttack(BossAttack ba)
    {
        StartCoroutine(ba.StartCooldown());
        StartCoroutine(AttackCooldown(ba.delay));
        yield return new WaitForSeconds(ba.delay);
        Collider2D pc = GetPlayerColliderInCollider(ba.collider);
        if (pc != null)
        {
            //Debug.Log("hit");
        }
    }

    /*******************************************************************************************************************/
    //givens time for animation to finish
    public IEnumerator AttackCooldown(float cc)
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown * cc);
        canAttack = true;
    }

    /*******************************************************************************************************************/

    public void DebugPoint()
    {
        Debug.DrawLine(transform.position, point, Color.red);
    }
}
