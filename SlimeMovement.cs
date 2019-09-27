using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : EnemyPathing
{
    /*
     * By: Parker Allen
     * Ver: 1.0
     * 
     * Setup:
     * 
     * jumpPower = 3000 to 4000
     * speedX = 300 to 400
     * maxSpeedX = 2 to 4
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

    // where the enemy will head next
    private Vector2 point;

    // direction the enmy will go
    private Vector3 direction;
    void FixedUpdate()
    {
        //check if object is on ground
        this.checkOnGround();

        //sets point
        point = getWaypoint();

        //sets the direction
        setDirection();

        //int to say if enemy should not move, move and/or jump
        int moveType = checkForWall(direction) * checkForHole(direction, point);

        //move and/or jump
        tryMoving(moveType);
    }

    //set the direction based on where the point is compared to enemy's position
    private void setDirection()
    {
        direction = (transform.position.x < point.x) ? Vector2.right : Vector2.left;
    }

    //move if the int is not 0
    //jumps if int is greater than 1
    private void tryMoving(int i)
    {
        if (i > 1)
        {
            jump();
        }
        if (i > 0)
        {
            move();
        }
    }

    //jump up and subtract from number of jumps
    private void jump()
    {
        numOfJumps--;
        rb.AddForce(Vector2.up * jumpPower);
    }

    //moves left or right
    private void move()
    {
        if(Mathf.Abs(rb.velocity.x) < maxSpeedX)
        {
            rb.AddForce(direction * speedX);
        }
        sr.flipX = direction.x > 1;
    }

    //displays where the point is for debugging purposes
    private void displayPoint(Vector2 p)
    {
        Debug.DrawRay(p, Vector2.down, Color.green);
    }
}
