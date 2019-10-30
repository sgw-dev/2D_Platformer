using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : EnemyPathing
{
    /*
     * By: Parker Allen
     * Ver: 1.1
     * 
     * Setup:
     * 
     * jumpPower = 6000
     * smallerjump = 1500
     * speedX = 2000
     * maxSpeedX = 14
     * searchdistance = 10?
     * 
     * Rigidbody:
     * mass = 10
     * gravity = 1.3
     * 
     * 2DBoxCollider: x2
     * one collider with slippery material
     * the other with sticky material
     */


    public int timeBetJump;     //time between when it lands and jumps
    private int counter;        //counter to keep track of time

    public float smallerjump;   //a jump that move less horizontally

    /**************************************************************************************************/
    void FixedUpdate()
    {
        if (checkOnGround())                //check if object is on ground
        {
            counter++;                      //add to counter
        }

        if(counter > timeBetJump)
        {
            counter = 0;                    //reset counter

            getWaypoint();                  //sets next point

            setDirection();                 //sets the direction

            int moveType = checkForWall(bound.size.x / 2) * checkForHole(maxSpeedX * jumpTime * Time.deltaTime);        //int to tell if slime can jump
            int smallJump = checkForHole(maxSpeedX * jumpTime * Time.deltaTime / 2);                                    //int to tell if slime can do a smaller jump

            tryMoving(moveType, smallJump);     //try moving
        }
    }

    /**************************************************************************************************/
    //decides whether to jump, smaller jump, or change points
    private void tryMoving(int i, int sm)
    {
        if (i == 1)                             //normal jump
        {
            Jump(Vector2.up * jumpPower);
            Move(speedX);
        }
        else if(sm == 1)                        //smaller jump
        {
            Jump(Vector2.up * jumpPower);
            Move(smallerjump);
        }
        else                                    //change points
        {
            nextPoint();
        }
    }

    /**************************************************************************************************/
    //displays where the point is for debugging purposes
    private void displayPoint(Vector2 p)
    {
        Debug.DrawRay(p, Vector2.down, Color.green);
    }
}
