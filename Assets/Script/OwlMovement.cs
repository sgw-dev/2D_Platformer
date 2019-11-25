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

    private Vector2 direction;      //direction to move
    private bool diving, sitting;            //if diving or sitting

    /*******************************************************************************************************************/

    public void FixedUpdate()
    {
        if (!diving)
        {
            rb.velocity = direction = Vector2.zero;     //zeros the rigidbodies velocity
            if (lineOfSight())                              //can see player?
            {
                PrepareToDive();                            //prepares to dive
            }
            else if (perch != Vector3.zero)                 //has perch?
            {
                direction = Perch();                        //direction = call to perch
            }
            if (direction != Vector2.zero)                  //direction is not 0
            {
                Move(direction, jumpPower);                 //move direction
            }
            else
            {
                sitting = true;                             //Sit!
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
            StartCoroutine(DiveAttack());       //DIVE! DIVE! DIVE!
        }
        else
        {
            direction = point;                  //move to position to dive from
        }
    }

    /*******************************************************************************************************************/

    IEnumerator DiveAttack()
    {
        Move(target, diveSpeed);                        //move to target at dive speed
        diving = true;                                  //it is diving
        yield return new WaitForSeconds(diveTime);      //wait
        diving = false;                                 //not diving
        StopCoroutine(DiveAttack());                    //end Coroutine
    }
}