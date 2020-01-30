using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BossPathing2 : MonoBehaviour
{
    /*
     * By: Parker Allen
     * Version: 2.0
     */

    public LayerMask collisionMask;     //layers of the ground
    public LayerMask playerMask;        //layer of the player

    private Bounds bound;               //bounds of object
    public MyBounds myBounds;           //my bounds of the object for laziness purposes

    public Transform playerPosition;    //players transform
    private Vector3 target;             //point placed based on player
    private Vector3 point;              //current point to go to
    private List<Vector3> points;       //next points to go to
    private Vector3[] waypoints;        //patroling points *not required
    private int waypointPointer;        //which waypoint it is going to

    private float offset;               //the distance to offset the target point from player position
    public float searchDistance;        //distacnce to search for player
    private float jumpHeight;           //max jump height

    public bool patroling, searching, chasing;  //states of pathing *only chasing is implemented so far
    private bool dontGetNextPoint;              //when really need to go to a point set to true;

    /**************************************************************************************************/
    
    public void Start()
    {
        bound = GetComponent<Collider2D>().bounds;      //get collider bounds
        myBounds.SetMyBounds(bound);                    //turn it into my lazy bounds

        //initialize some things
        points = new List<Vector3>();
        point = transform.position;
        waypoints = new Vector3[0];
        waypointPointer = 0;

        if(jumpHeight == 0)
        {
            jumpHeight = searchDistance;
        }
    }

    /**************************************************************************************************/
    //returns true if it finds a new point to go to, false otherwise
    //looks for player if can't find player trys to get another point and if it still can't continues towards previous point
    public bool FindDestination(float dir)
    {
        SetDontGetNextPoint();      //check to see if it can get another point

        //checks if player is in search distance, line of sight to player, and can place a on the ground where the player is
        if (!dontGetNextPoint && InSearchDistance(playerPosition.position) && LineOfSight(playerPosition.position) && FoundTarget(dir))
        {
            points.Clear();         //clear list of points
            points.Add(target);     //add target to list of points
            chasing = true;         //set state to chasing
            return true;            //found new place to go
        }
        else if(points.Count == 0 && InBounds(point))   //no previous points and current point is in its bounds
        {
            points.Add(NormalPatrol());     //add point to list of points
            chasing = false;                //set state to not chasing
            return true;                    //found new place to go
        }

        chasing = false;        //set state to not chasing
        return false;           //no new points
    }

    /**************************************************************************************************/
    //trys to find a path to the first point by adding more points to list of points
    public void FindPath()
    {
        if(points.Count != 0 && !InYBounds(points[points.Count - 1]))       //don't find a new path if last point is on the same y height
        {
            Vector3 direction = Vector3.right * Mathf.Sign(points[0].x - transform.position.x);     //direction from this object to first point

            if (points[points.Count-1].y > transform.position.y)        //last point above this object
            {
                FindLedgeToGetUp(points[points.Count - 1], direction);      //find ledge based last point's position
                //FindPath(direction);
            }
            else if(points[points.Count - 1].y < transform.position.y)      //last point below this object
            {
                FindLedgeToGetUp(transform.position + myBounds.xhalf * -direction, -direction);     //find ledge from this object's position
                //FindPath(direction);
            }
        }
    }

    /**************************************************************************************************/
    //returns true if able to place point on ground at target
    public bool FoundTarget(float dir)
    {
        target = PlacePointOnGround(playerPosition.position, jumpHeight + myBounds.yhalf);      //set target to player position on ground
        target = OffsetPoint(target, dir, offset);                                              //offset target
        target = PlacePointOnGround(target, myBounds.y);                                        //try to place point on ground

        if (target == Vector3.zero)     //failed to place point
        {
            target = PlacePointOnGround(playerPosition.position, jumpHeight + myBounds.yhalf);  //set target to player position on ground
            target = target = OffsetPoint(target, -dir, offset);                                //offset target the other side of player
            target = PlacePointOnGround(target, myBounds.y);                                    //try to place point on ground

            if (target == Vector3.zero)     //failed to place the point again
            {
                return false;   //return false
            }
        }
        return true;        //return true
    }

    /**************************************************************************************************/
    //returns vectors of offset point
    public Vector3 OffsetPoint(Vector3 p, float direction, float distance)
    {
        if (InBounds(p))    //if the point is in this object
        {
            p.x += direction * (myBounds.x + distance / 2);     //offset point further from this object
            dontGetNextPoint = true;                            //don't get another point
        }
        else if(InYBounds(p))   //only offset point if onthe same y height
        {
            p.x += -direction * distance;   //offset point closer to this object
        }
        return p;   //return the point
    }

    /**************************************************************************************************/
    //returns a waypoint or a random point
    public Vector3 NormalPatrol()
    {
        if(waypoints.Length > 0)    //test if there are any set waypoints
        {
            NextWaypoint();                         //set next waypoint
            return waypoints[waypointPointer];      //return the waypoint
        }
        return GetRandomPoint(transform.position);  //return random point
    }

    /**************************************************************************************************/
    //return true if it found two points, false if 0 or 1 point
    public bool FindLedgeToGetUp(Vector3 p, Vector3 direction)
    {
        Vector3 p1, p2;
        p1 = p2 = Vector3.zero;

        //raycast down to get the ground
        RaycastHit2D hit = Physics2D.Raycast(p, Vector3.down, myBounds.yhalf + 1, collisionMask);
        //Debug.DrawRay(p, Vector3.down * (myBounds.yhalf + 1), Color.green);

        if (hit)      //if there is ground below
        {
            float angle = Vector2.Angle(hit.normal, Vector2.up);    //find the slope of the ground

            if(angle < 5)   //slope of ground is less than 5
            {
                Bounds temp = hit.collider.bounds;      //get bounds of the ground

                if (direction.x > 0)    //direction is to the right
                {
                    p1 = new Vector3(temp.min.x, temp.max.y + myBounds.yhalf);  //p1 equals the top left corner of ground
                }
                else
                {
                    p1 = new Vector3(temp.max.x, temp.max.y + myBounds.yhalf);  //p1 equals the top right corner of ground
                }

                p1 = PlacePointOnGround(p1, myBounds.yhalf + 1);                                        //place p1 on ground
                p2 = PlacePointOnGround(p1 + direction * -myBounds.x, jumpHeight + myBounds.yhalf);     //place p2 on ground offset from p1
            }
        }

        //Debug.DrawRay(p1 + direction * -myBounds.x, Vector3.down, Color.green);
        //Debug.DrawRay(p1, Vector3.down, Color.green);

        if (InXBounds(p1))      //if p1 is above or below this object
        {
            p1 = Vector3.zero;  //set p1 to zero
        }
        if (InXBounds(p2))      //if p2 is above or below this object
        {
            p2 = Vector3.zero;  //set p2 to zero
        }

        if(p1 != Vector3.zero && p2 != Vector3.zero)    //niether p1 or p2 equal zero
        {
            if (Vector3.Distance(p2, transform.position) > Vector3.Distance(p1, transform.position))    //p2 is closer to this object
            {
                points.Add(p2);     //add p2 to points
                points.Add(p1);     //add p1 to points
            }
            else
            {
                points.Add(p1);     //add p1 to points
                points.Add(p2);     //add p2 to points
            }
            return true;            //return true
        }

        else if(p1 != Vector3.zero)     //p1 dosen't equal zero
        {
            points.Add(p1);     //add p1 to points
        }
        else if(p2 != Vector3.zero)     //p1 dosen't equal zero
        {
            points.Add(p2);     //add p2 to points
        }
        return false;       //return false
    }

    /**************************************************************************************************/
    //return a point a random distance from this object
    public Vector3 GetRandomPoint(Vector3 p)
    {
        Vector3 temp = PlacePointOnGround(p + (Random.Range(-2f, 2f) * 10) * Vector3.right, myBounds.yhalf);    //place the random point on the ground

        if (temp == Vector3.zero)       //couldn't place point on ground
        {
            temp = GetRandomPoint(p);   //tra again
        }
        return temp;        //return random point
    }

    /**************************************************************************************************/
    //returns a point on the groun below. the point is then set above the ground at half this objects height
    public Vector3 PlacePointOnGround(Vector3 rayOrigin, float distance)
    {
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, distance + 1, collisionMask);     //raycast down a given distance
        if (hit)
        {
            return hit.point + Vector2.up * myBounds.yhalf;     //return the point above the place the ray hit
        }
        return Vector3.zero;    //zero
    }

    /**************************************************************************************************/
    //return true if player and this object are on the same platform
    public bool OnSamePlatform(Vector3 p)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, myBounds.yhalf + .1f, collisionMask);
        //raycast to get the bounds of platform

        if (hit)
        {
            Bounds temp = hit.collider.bounds;
            if(p.x > temp.min.x && p.x < temp.max.x && InYBounds(p))    //dosen't work
            {
                return true;
            }
        }
        return false;
    }

    /**************************************************************************************************/
    //return true if ray cast doesn't hit a wall
    public bool LineOfSight(Vector3 p)
    {
        Vector3 dir = (p - transform.position).normalized;      //direction to cast ray
        float dis = Vector3.Distance(p, transform.position);    //distance to cast ray

        return !Physics2D.Raycast(transform.position , dir, dis, collisionMask);
    }

    /**************************************************************************************************/
    //return true if the player is in search distance from this object
    public bool InSearchDistance(Vector3 p)
    {
        return searchDistance > Vector3.Distance(p, transform.position);
    }

    /**************************************************************************************************/
    //returns true if the player's x position is between this object left and right bounds
    public bool InXBounds(Vector3 p)
    {
        return Mathf.Abs(p.x - transform.position.x) < myBounds.xhalf;
    }

    /**************************************************************************************************/
    //returns true if the player's y position is between this object top and bottom bounds
    public bool InYBounds(Vector3 p)
    {
        return Mathf.Abs(p.y - transform.position.y) < myBounds.yhalf;
    }

    /**************************************************************************************************/
    //returns if player position in this objects bounds
    public bool InBounds(Vector3 p)
    {
        return InXBounds(p) && InYBounds(p);
    }

    /**************************************************************************************************/
    //set jump height
    public void SetJumpHieght(float h)
    {
        jumpHeight = h;
    }

    /**************************************************************************************************/
    //set target
    public void SetTarget(Vector3 t)
    {
        target = t;
    }

    /**************************************************************************************************/
    //set waypoints
    public void SetWaypoints(Vector3[] v)
    {
        waypoints = v;
    }

    /**************************************************************************************************/
    //increment waypoint pointer
    public void NextWaypoint()
    {
        if(waypoints.Length > 1)
        {
            waypointPointer = (waypointPointer + 1) % waypoints.Length;
        }
    }

    /**************************************************************************************************/
    //get current waypoint
    public Vector3 GetWaypoint()
    {
        return waypoints[waypointPointer];
    }

    /**************************************************************************************************/
    //clear points list
    public void ClearPoints()
    {
        points.Clear();
    }

    /**************************************************************************************************/
    //sets point to last point in list of points and remove last point from the list
    public void NextPoint()
    {
        if (points.Count > 0)
        {
            point = points[points.Count - 1];
            points.RemoveAt(points.Count - 1);
        }
    }

    /**************************************************************************************************/
    //returns point
    public Vector3 GetPoint()
    {
        return point;
    }

    /**************************************************************************************************/
    //set offset
    public void SetOffset(float o)
    {
        offset = o;
    }

    /**************************************************************************************************/
    //test if it can get another point
    public void SetDontGetNextPoint()
    {
        if (InBounds(point) || offset + myBounds.xhalf < Mathf.Abs(point.x - transform.position.x))    //if point in the objects bounds
        {
            dontGetNextPoint = false;   //can get another point
        }
    }

    /**************************************************************************************************/
    //my bounds because I am tired of bounds.size.x / 2
    public struct MyBounds
    {
        public float xhalf, x, yhalf, y;
        public void SetMyBounds(Bounds b)
        {
            xhalf = b.size.x / 2;
            x = b.size.x;
            yhalf = b.size.y / 2;
            y = b.size.y;
        }
    }

    /**************************************************************************************************/
    //draw point
    public void DebugPoint()
    {
        Debug.DrawRay(point, Vector3.down, Color.yellow);
    }

    //draw list of points
    public void DebugPoints()
    {
        foreach(Vector3 p in points)
        {
            Debug.DrawRay(p, Vector3.down, Color.cyan);
        }
    }
}
