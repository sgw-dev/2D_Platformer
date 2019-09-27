using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    /*
     * By: Parker Allen
     * Ver: 1.0
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
     * 
     * collisionMask = layer of the ground
     * playerPosition = transform of the player
     */

    public List<Transform> points = new List<Transform>();
    public LayerMask collisionMask;
    public Transform playerPosition;
    public float jumpPower;
    public float speedX;
    public float maxSpeedX;
    public float searchDistance;
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public int numOfJumps;

    //Player's position
    private Vector2 target;

    //points positions
    private Vector2[] waypoints;

    //saves the place in waypoints array
    private int wpPointer = 0;

    //bounds of the first Collider
    private Bounds bound;

    //gravity on object
    private float gravity;

    //max jump height of object
    private float jumpHeight;

    //max number of jumps
    private int maxNumOfJumps;

    //all layers except its own layer
    private int layerMask;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        bound = GetComponent<BoxCollider2D>().bounds;

        //all layers except its own layer
        layerMask = 1 << 12;
        layerMask = ~layerMask;

        //converts points to waypoints
        waypoints = setWaypoints(points);

        maxNumOfJumps = 1;
        gravity = rb.gravityScale * Physics2D.gravity.magnitude;
        float v0 = jumpPower / rb.mass;
        jumpHeight = (v0 * v0) / (2 * gravity);
    }

    //returns the postion of either the player or a waypoint
    public Vector2 getWaypoint()
    {
        hitWaypoint();
        if(target != Vector2.zero)
        {
            return target;
        }
        return waypoints[wpPointer];
    }

    //if the object reachs a point sets the next point
    private void hitWaypoint()
    {
        if (!lineOfSight() && withinBound(target))
        {
            target = Vector2.zero;
        }
        else if (withinBound(waypoints[wpPointer]))
        {
            nextPoint();
        }
    }

    //checks if object hits point
    private bool withinBound(Vector2 p)
    {
        if(Mathf.Abs(transform.position.x - p.x) < bound.size.x / 2 && Mathf.Abs(transform.position.y - p.y) < bound.size.y / 2)
        {
            return true;
        }
        return false;
    }

    //sets target to below player position if object has line of sigth and within
    //search distance
    private bool lineOfSight()
    {
        Vector2 dir = (playerPosition.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, searchDistance, layerMask);
        if (hit && hit.transform.CompareTag(playerPosition.tag))
        {
            RaycastHit2D hit2 = Physics2D.Raycast(hit.transform.position, Vector2.down, jumpHeight, collisionMask);
            if (hit2)
            {
                target = hit2.point + Vector2.up * (bound.size.y / 2);
                return true;
            }
        }
        return false;
    }

    //changes wppointer to next waypoint, 
    //when reaches end of array it starts over
    private void nextPoint()
    {
        wpPointer = (wpPointer + 1) % waypoints.Length;
    }

    //transform points to Vector2 waypoints
    private Vector2[] setWaypoints(List<Transform> p)
    {
        Vector2[] rtn = new Vector2[p.Count];
        for(int i = 0; i < p.Count; i++)
        {
            rtn[i] = TtoV2(p[i]);
        }
        return rtn;
    }

    //cast raydown to see if on ground if true the reset number of jumps
    public void checkOnGround()
    {
        for(int i = 0; i < 3; i++)
        {
            float x = bound.min.x + i * bound.size.x;
            Vector2 rayOrgin = new Vector2(x, transform.position.y - bound.size.y / 2);
            RaycastHit2D hit = Physics2D.Raycast(rayOrgin, Vector2.down, .11f, collisionMask);
            if (hit)
            {
                numOfJumps = maxNumOfJumps;
            }
        }
    }

    //checks for a wall infront of it if no wall, can move
    public int checkForWall(Vector3 direction)
    {
        Vector2 rayOrigin = transform.position + direction * (bound.size.x / 2);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, bound.size.x / 2, collisionMask);
        if (hit)
        {
            return jumpAboveWall(hit.collider.bounds.max.y);
        }
        else
        {
            return 1;
        }
    }

    //checks if can jump over wall if object can jump else stop
    private int jumpAboveWall(float height)
    {
        if (height < jumpHeight && numOfJumps > 0)
        {
            return 2;
        }
        return 0;
    }

    //checks if hole infront of object if not hole can keep moveing
    public int checkForHole(Vector3 direction, Vector2 point)
    {
        Vector2 rayOrigin = transform.position + direction * (bound.size.x * (3 / 2));
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, jumpHeight, collisionMask);
        if (hit)
        {
            return 1;
        }
        else
        {
            return jumpOverPit(point);
        }
    }

    //checks if object can jump over pit, if it can jump else stop
    private int jumpOverPit(Vector2 point)
    {
        RaycastHit2D hit = Physics2D.Raycast(point, Vector2.down, 5, collisionMask);
        if (hit)
        {
            float time = Mathf.Abs(Vector3.Distance(point, transform.position)) / maxSpeedX;
            float y = transform.position.y + jumpHeight + .5f * gravity * (time * time);
            if (y > point.y && numOfJumps > 0)
            {
                return 2;
            }
        }
        return 0;
    }

    //converts transform to Vector2
    private Vector2 TtoV2(Transform t)
    {
        return new Vector2(t.position.x, t.position.y);
    }
}
