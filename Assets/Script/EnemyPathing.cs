using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    /*
     * By: Parker Allen
     * Ver: 1.2
     * 
     * Notes:
     * target, waypoint and point will all be called point, unless
     * otherwised specified
     * 
     * if the enemy stops going through the waypoints it because you are probably
     * forcing it to do somthing it can't you tyrant! He is trying his best don't
     * make him do the impossible!
     * 
     * Setup:
     * to create path for the enemy create empty objects and put the transforms
     * in to the points list
     * go to "Edit" > "Project Settings" > "Physics2D" and uncheck SlimeLayer by SlimeLayer
     * 
     * collisionMask = layer of the ground
     * playerPosition = transform of the player
     */

    public LayerMask collisionMask;     //collision mask for ground
    [HideInInspector] public int layerMask;              //all layers except its own layer

    public Transform playerPosition;
    private SpriteRenderer sr;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Bounds bound;      //bounds of the first Collider

    public float jumpPower;         //force used to go up
    private float gravity;          //gravity on object
    private float jumpHeight;       //max height of jump
    [HideInInspector] public float jumpTime;    //time to reach max height in jump

    private int maxNumOfJumps;      //max number of jumps
    private int numOfJumps;         //max jump height of object

    public float speedX;            //force used to go left or right
    public float maxSpeedX;         //used for estimating distances
    private Vector3 direction;      // direction the enmy will go

    public List<Transform> points = new List<Transform>();      //list of waypoints enemy will follow
    public float searchDistance;    //distance enemy will search for player
    [HideInInspector] public Vector2 target;         //player's position
    [HideInInspector] public Vector2[] waypoints;    //points positions
    [HideInInspector] public int wpPointer = 0;      //saves the place in waypoints array
    public Vector2 point;          //where the enemy will head next

    /*******************************************************************************************************************/
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();                           //rigidbody
        sr = GetComponent<SpriteRenderer>();                        //spritrenderer
        bound = GetComponent<BoxCollider2D>().bounds;               //bounds of collider

        layerMask = 1 << 12;                                        //all layers except its own layer
        layerMask = ~layerMask;

        waypoints = setWaypoints(points);                           //converts transforms to waypoints

        maxNumOfJumps = 1;                                          //max number of jumps
        gravity = rb.gravityScale * Physics2D.gravity.magnitude;    //gravity on object
        float v0 = jumpPower / rb.mass;
        jumpTime = v0 / gravity;                                    //jump time
        jumpHeight = (v0 * v0) / (2 * gravity);                     //max jump height
    }

    /*******************************************************************************************************************/
    //nextpoint to move to
    public void getWaypoint()
    {
        if (target != Vector2.zero)                 //check if it has a target
        {
            point = target;                         //move to player
        }
        else
        {
            point = waypoints[wpPointer];           //move to next waypoint
        }
    }

    /*******************************************************************************************************************/
    //if the object reachs a point sets the next point
    public void hitWaypoint()
    {
        if (!lineOfSight() && withinBound(target))      //check line of sight and on target
        {
            target = Vector2.zero;                      //sets target to zero
        }
        else if (withinBound(waypoints[wpPointer]))     //check on waypoint
        {
            nextPoint();                                //get next waypoint
        }
    }

    /*******************************************************************************************************************/
    //checks if object hits point
    public bool withinBound(Vector2 p)
    {
        if (Mathf.Abs(transform.position.x - p.x) < bound.size.x / 2 && Mathf.Abs(transform.position.y - p.y) < bound.size.y / 2)
        {
            return true;
        }
        return false;
    }

    /*******************************************************************************************************************/
    //check if it has line of sight on player and player over ground
    private bool lineOfSight()
    {
        Vector2 dir = (playerPosition.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, searchDistance, layerMask);       //raycast to target
        //Debug.DrawRay(transform.position, dir * searchDistance, Color.black, 2);

        if (hit && hit.transform.CompareTag(playerPosition.tag))                                        //raycast hit player
        {
            RaycastHit2D hit2 = Physics2D.Raycast(hit.transform.position, Vector2.down, jumpHeight, collisionMask);     //raycast down from player
            if (hit2)
            {
                target = hit2.point + Vector2.up * (bound.size.y / 2);                                  //sets slightly above the ground the player is above
                return true;
            }
        }
        return false;
    }

    /*******************************************************************************************************************/
    //changes wppointer to next waypoint, 
    //when reaches end of array it starts over
    public void nextPoint()
    {
        wpPointer = (wpPointer + 1) % waypoints.Length;
    }

    /*******************************************************************************************************************/
    //transform points to Vector2 waypoints
    public Vector2[] setWaypoints(List<Transform> p)
    {
        Vector2[] rtn = new Vector2[p.Count];
        for (int i = 0; i < p.Count; i++)
        {
            rtn[i] = TtoV2(p[i]);
        }
        return rtn;
    }

    /*******************************************************************************************************************/
    //checks if obj is on ground
    public bool checkOnGround()
    {
        for (int i = 0; i < 3; i++)                                                                 //loops 3 times
        {
            float x = transform.position.x - i * (bound.size.x / 2) + (bound.size.x / 2);           //used to get left, right and center of object
            Vector2 rayOrgin = new Vector2(x, transform.position.y - bound.size.y / 2);             //ray orgin at bottom left, bottom center and bottom right
            RaycastHit2D hit = Physics2D.Raycast(rayOrgin, Vector2.down, .11f, collisionMask);      //raycast down
            //Debug.DrawRay(rayOrgin, Vector2.down * .11f, Color.red);

            if (hit)
            {
                numOfJumps = maxNumOfJumps;                                                         //reset number of jumps
                return true;
            }
        }
        return false;
    }

    /*******************************************************************************************************************/
    //checks a distance for a wall
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
        nextPoint();                                    //it cannot get a next waypoint
        return 0;
    }

    /*******************************************************************************************************************/
    //checks if hole infront of object
    public int checkForHole(float distance)
    {
        Vector2 rayOrigin = transform.position + direction * distance;                                  //ray origin left or right at a distance from object
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, jumpHeight, collisionMask);       //cast ray down from orign
        Debug.DrawRay(rayOrigin, Vector2.down * jumpHeight, Color.green, 3);

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
            t += Mathf.Sqrt(2 * (hit.point.y - (transform.position.y + jumpHeight)) / -gravity);                //time in the air

            bool reach = transform.position.x + t * maxSpeedX * Time.deltaTime > hit.collider.bounds.min.x;     //air time * maxspeedx is greart than ledge point is on
            if (reach && numOfJumps > 0)
            {
                //Debug.DrawRay(transform.position, Vector3.right * t * maxSpeedX * Time.deltaTime, Color.red, 5);
                return 2;                                                                                           //it can make it
            }
        }
        nextPoint();                                                                                                //couldn't make it get next waypoint
        return 0;
    }

    /*******************************************************************************************************************/
    //Enemy jumps with given vector
    public void Jump(Vector2 jump)
    {
        if (numOfJumps > 0)         //check number of jumps
        {
            numOfJumps--;           //subtracts number of jumps
            rb.AddForce(jump);      //jump
        }
    }

    /*******************************************************************************************************************/
    //moves left or right at a rate given
    public void Move(float move)
    {
        if (Mathf.Abs(rb.velocity.x) < maxSpeedX)       //can't add more force one it is above max
        {
            rb.AddForce(direction * move);
        }
        sr.flipX = direction.x > 1;                     //change sprites direction
    }

    /*******************************************************************************************************************/
    //set the direction based on where the point is compared to enemy's position
    public void setDirection()
    {
        direction = (transform.position.x < point.x) ? Vector2.right : Vector2.left;
    }

    /*******************************************************************************************************************/
    //converts transform to Vector2
    private Vector2 TtoV2(Transform t)
    {
        return new Vector2(t.position.x, t.position.y);
    }
}