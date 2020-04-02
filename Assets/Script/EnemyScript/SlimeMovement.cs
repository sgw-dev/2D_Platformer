using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : EnemyPathing
{
    /*
     * By: Parker Allen
     * Ver: 1.1.1
     * 
     * Setup:
     * 
     * jumpPower = 6000
     * smallerjump = 1000
     * speedX = 2000
     * maxSpeedX = 8
     * searchdistance = 8
     * timeBetJump = 60
     * 
     * Rigidbody:
     * mass = 10
     * gravity = 1.3
     * 
     * 2DBoxCollider: x2
     * one collider with slippery material
     * the other with sticky material
     */


    private int timeBetJump;     //time between when it lands and jumps
    private int counter;        //counter to keep track of time

    private float smallerjump;   //a jump that move less horizontally

    //Spencer
    private Animator anim;
    private bool attackFlag = true;
    private int attackTimer;
    private int attackTime = 100;
    private Player playerScript;


    /**************************************************************************************************/

    //Spencer
    public void Start() {
        
        anim = this.GetComponent<Animator>();
        jumpPower = 6000;
        smallerjump = 1000;
        speedX = 2000;
        maxSpeedX = 8;
        searchDistance = 8;
        timeBetJump = 60;
        EnemyStart();
        playerScript = playerPosition.GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        if (attackTimer >= attackTime)
        {
            attackTimer = 0;
            attackFlag = true;
        }
        else
        {
            attackTimer++;
        }

        if (checkOnGround())                //check if object is on ground
        {
            counter++;                      //add to counter
            anim.SetBool("Ground", true);
        }
        else
        {
            anim.SetBool("Ground", false);
            
        }

        if (counter > timeBetJump)
        {
            counter = 0;                    //reset counter

            hitWaypoint();

            getWaypoint();                  //sets next point

            setDirection();                 //sets the direction

            int moveType = checkForWall(bound.size.x / 2) * checkForHole(maxSpeedX * jumpTime * Time.deltaTime);        //int to tell if slime can jump
            int smallJump = checkForHole(maxSpeedX * jumpTime * Time.deltaTime / 2);                                    //int to tell if slime can do a smaller jump
            if (!playerScript.dead)
            {
                tryMoving(moveType, smallJump);     //try moving
            }
        }
    }

    /**************************************************************************************************/
    //decides whether to jump, smaller jump, or change points
    private void tryMoving(int i, int sm)
    {
        if (i == 1)                             //normal jump
        {
            anim.SetTrigger("Jump");
            Jump(Vector2.up * jumpPower);
            Move(speedX, maxSpeedX);
        }
        else if (sm == 1)                        //smaller jump
        {
            anim.SetTrigger("Jump");
            Jump(Vector2.up * jumpPower);
            Move(smallerjump, maxSpeedX);
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
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && attackFlag && !anim.GetBool("Ground"))
        {
            other.SendMessage("applyDamage", 1f);
            attackFlag = false;
        }
    }
    

}
