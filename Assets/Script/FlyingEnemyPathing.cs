using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyPathing : EnemyPathing
{

    public float searchAngle;
    public float degBetweenSearches;
    public float searchDistanceWall;
    public int minNumOfSearches;
    public Vector3 perch;
    public bool sit;
    public GameObject sprite;
    [HideInInspector] public bool needToAvoid;

    public Vector3 Avoid()
    {
        needToAvoid = false;
        for (int i = 0; i < searchAngle / degBetweenSearches; i++)
        {
            int n = ((i % 2 == 1) ? -1 : 1) * ((i + 1) / 2);
            Vector3 dir = Quaternion.Euler(0, 0, n * degBetweenSearches) * transform.up;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, searchDistanceWall, collisionMask);
            //Debug.DrawRay(transform.position, dir * searchDistanceWall, Color.red);

            if (hit)
            {
                needToAvoid = true;
            }
            if (!hit && i > minNumOfSearches)
            {
                if (!needToAvoid)
                {
                    return transform.up;
                }
                return dir;
            }
        }
        return transform.up;
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

    public Vector3 FindGround()
    {
        Vector3 p = Vector3.zero;
        for (int i = 0; i < searchAngle / degBetweenSearches; i++)
        {
            int n = ((i % 2 == 1) ? -1 : 1) * ((i + 1) / 2);
            Vector3 dir = Quaternion.Euler(0, 0, n * degBetweenSearches) * transform.up;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, searchDistance, collisionMask);
            //Debug.DrawRay(transform.position, dir * searchDistance, Color.yellow);

            if (hit)
            {
                p.x = (transform.position.x < hit.transform.position.x) ? (hit.collider.bounds.min.x + bound.size.x / 2) : (hit.collider.bounds.max.x - bound.size.x / 2);
                p.y = hit.collider.bounds.max.y + bound.size.y / 2;
                return p;
            }
        }
        return p;
    }

    public bool lineOfSight()
    {
        Vector2 dir = (playerPosition.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, searchDistance, base.layerMask);       //raycast to target
        //Debug.DrawRay(transform.position, dir * searchDistance, Color.black);

        if (hit && hit.transform.CompareTag(playerPosition.tag))                                        //raycast hit player
        {
            base.target = hit.point;
            return true;
        }
        return false;
    }

    public void Move(Vector2 v)
    {
        v -= (Vector2)transform.position;
        float angle = Vector3.SignedAngle(Vector3.up, v, Vector3.forward);
        transform.rotation = Quaternion.Euler(0, 0, angle);

        rb.AddForce(v.normalized * jumpPower * Time.deltaTime);
    }

    public Vector2 Perch()
    {
        if (withinBound(perch))
        {
            return Vector2.zero;
        }
        return perch;
    }

    public void LockRotation()
    {
        sprite.transform.rotation = Quaternion.Euler(0, 0, -transform.rotation.z);
    }
}