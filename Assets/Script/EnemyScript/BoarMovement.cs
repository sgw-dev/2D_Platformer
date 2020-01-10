using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarMovement : EnemyPathing
{
    /*
     * By: Parker Allen
     * Version 1.0
     * 
     * Setup:
     *  jumpPower = 100
     *  speedx = 500
     *  maxspeedx = 5
     *  searchdistance = 8
     *  charge distance = 10
     *  charge speed = 1000
     *  max charge speed = 10
     *  rest time = 1
     *  prepare time = 1
     *  
     *  Boxcollider2D:
     *      stickeyPlayer material
     *      
     *  Rigidbody2D:
     *      mass = 15
     *      gravity = 1
     */

    private float chargeDistance;                    //distance boar will charge
    private float chargeSpeed, maxChargeSpeed;       //acceleration and max speed for charging
    private float restTime, perpareTime;             //time for resting and preparing

    private bool resting, charging, preparing;      //the state of boar

    /**************************************************************************************************/

    public void Start()
    {
        EnemyStart();
        jumpPower = 100;
        speedX = 500;
        maxSpeedX = 5;
        searchDistance = 8;
        chargeDistance = 10;
        chargeSpeed = 1000;
        maxChargeSpeed = 10;
        restTime = 1;
        perpareTime = 1;
    }

    /**************************************************************************************************/

    void Update()
    {
        if (charging)
        {
            Charge();                               //CHARGE!!!
        }
        else if(!(charging || resting))
        {
            hitWaypoint();                          //check if hits waypoint and changes it

            getWaypoint();                          //sets the next waypoint

            setDirection();                         //direction to next waypoint

            if (!preparing)
            {
                if (target != Vector2.zero)
                {
                    StartCoroutine(Prepare());      //start preparing if player in sight
                }
                else
                {
                    Patrol();                       //patrol
                }
            }
        }
    }

    /**************************************************************************************************/
    //normal moving
    private void Patrol()
    {
        int moveType = checkForWall(bound.size.x / 2) * checkForHole(bound.size.x / 2);        //int to tell if can move
        if (moveType == 1)
        {
            Move(speedX, maxSpeedX);        //move
        }
    }

    /**************************************************************************************************/
    //charging at player
    private void Charge()
    {
        int moveType = checkForWall(bound.size.x / 2) * checkForHole(bound.size.x / 2);        //int to tell if slime can jump

        if (withinBound(point))
        {
            StartCoroutine(Rest());                 //rest once reach charge distance
        }
        else if(moveType == 1)
        {
            Move(chargeSpeed, maxChargeSpeed);      //charge
        }
    }

    /**************************************************************************************************/
    //pause before charging
    IEnumerator Prepare()
    {
        preparing = true;                                               //preparing
        sr.flipX = direction.x < 0;                                     //change sprites direction
        yield return new WaitForSeconds(perpareTime);                   //wait
        preparing = false;                                              //not preparing
        point = transform.position + (direction * chargeDistance);      //set place to charge to
        charging = true;                                                //charging
        StopCoroutine(Prepare());                                       //end coroutine
    }

    /**************************************************************************************************/
    //rest after charging
    IEnumerator Rest()
    {
        charging = false;                               //not charging
        resting = true;                                 //resting
        yield return new WaitForSeconds(restTime);      //wait
        resting = false;                                //not resting
        StopCoroutine(Rest());                          //end coroutine
    }
}
