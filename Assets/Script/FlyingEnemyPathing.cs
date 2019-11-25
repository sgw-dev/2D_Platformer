using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyPathing : EnemyPathing
{
    /*
     * By: Parker Allen
     * Version 1.0
     * 
     * Setup:
     * If using LockRotation:
     *      Move the sprite renderer to a empty game object child of object
     *      Set sprite to that child
     */

    public float searchAngle;                   //angle to search
    public float degBetweenSearches;            //angle between searches
    public float searchDistanceWall;            //distance to search for wall
    public int minNumOfSearches;                //min searches for wall
    [HideInInspector] public bool needToAvoid;  //bool if need to avoid wall

    public Vector3 perch;                       //Position where enemy will sit
    public bool sit;                            //if enemy is sitting
    public GameObject sprite;                   //child game object used to lock rotation

    /*******************************************************************************************************************/
    //Calculates vector to avoid a wall smoothly
    public Vector3 Avoid()
    {
        needToAvoid = false;                                                                //dosen't need to avoid wall
        for (int i = 0; i < searchAngle / degBetweenSearches; i++)                          //loop for search angle / degBetweenSearches
        {
            int n = ((i % 2 == 1) ? -1 : 1) * ((i + 1) / 2);                                //flips int from + to - ex. 1, -1, 2, -2, etc.
            Vector3 dir = Quaternion.Euler(0, 0, n * degBetweenSearches) * transform.up;    //calculates angle from direction it is facing

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, searchDistanceWall, collisionMask);   //I cast RAY!!
            //Debug.DrawRay(transform.position, dir * searchDistanceWall, Color.red);

            if (hit)
            {
                needToAvoid = true;                 //if hits something it needs to avoid
            }
            if (!hit && i > minNumOfSearches)       //dosen't hit anything and done min number of searches
            {
                if (!needToAvoid)
                {
                    return transform.up;            //if didn't find a wall continue forward
                }
                return dir;                         //else go to the first ray cast that doesn't hit a wall
            }
        }
        return transform.up;                        //forward march!
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
    //return the corner position of first found ground
    public Vector3 FindGround()
    {
        Vector3 p = Vector3.zero;                                       //returned vector
        for (int i = 0; i < searchAngle / degBetweenSearches; i++)      //loop for search angle / degBetweenSearches
        {
            int n = ((i % 2 == 1) ? -1 : 1) * ((i + 1) / 2);            //flips int from + to - ex. 1, -1, 2, -2, etc.
            Vector3 dir = Quaternion.Euler(0, 0, n * degBetweenSearches) * transform.up;        //calculates angle from direction it is facing

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, searchDistance, collisionMask);       //I cast RAY!!
            Debug.DrawRay(transform.position, dir * searchDistance, Color.yellow, 5);

            if (hit)
            {
                //gets the left or right side of ground
                p.x = (transform.position.x < hit.transform.position.x) ? (hit.collider.bounds.min.x + bound.size.x / 2) : (hit.collider.bounds.max.x - bound.size.x / 2);
                p.y = hit.collider.bounds.max.y + bound.size.y / 2;         //get the top of the ground + half of enemy height
                return p;                                                   //return vector
            }
        }
        return p;                                                           //return vector
    }

    /*******************************************************************************************************************/
    //test if enemy has unimpeded vision on player
    public bool lineOfSight()
    {
        Vector2 dir = (playerPosition.position - transform.position).normalized;                            //direction to player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, searchDistance, base.layerMask);      //raycast to target
        //Debug.DrawRay(transform.position, dir * searchDistance, Color.black);

        if (hit && hit.transform.CompareTag(playerPosition.tag))                                        //raycast hit player
        {
            base.target = hit.point;                                                                    //set target to players position
            return true;                                                                                //enemy does have line of sight on player
        }
        return false;                                                                                   //enemy cannot see player
    }

    /*******************************************************************************************************************/
    //moves flying enemies
    public void Move(Vector2 v, float speed)
    {
        v -= (Vector2)transform.position;                                       //place vector at origin
        float angle = Vector3.SignedAngle(Vector3.up, v, Vector3.forward);      //converts vector to an angle
        transform.rotation = Quaternion.Euler(0, 0, angle);                     //rotates enemy to angle

        rb.AddForce(v.normalized * speed);                 //add force in direction facing
    }

    /*******************************************************************************************************************/
    //stop the enemy if on its perch
    public Vector2 Perch()
    {
        if (Vector3.Distance(transform.position, perch) < .1f)     //on postion of perch
        {
            return Vector2.zero;    //return 0
        }
        return perch;               // return position of perch
    }

    /*******************************************************************************************************************/
    //counters the rotation to lock sprite rotation horizontaly
    public void LockRotation()
    {
        sprite.transform.rotation = Quaternion.Euler(0, 0, -transform.rotation.z);      //set sprite rotation to -object rotation
    }
}