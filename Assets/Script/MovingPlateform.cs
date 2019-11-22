using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlateform : RayCastController
{
    //what platform can move
    public LayerMask passengerMask;

    //points are based on objects current position
    public Vector3[] localWaypoints;
    Vector3[] globalWaypoints;

    //speed of platform
    public float speed;

    //if you want to go in reverse order of waypoints
    public bool cyclic;

    //pause at each point
    public float waitTime;

    //controls acceleration and deaccleration of platform - 0 is very snappy and 2 is criminallly smooth
    [Range(0, 2)]
    public float easeAmount;

    //previous way point
    int fromWaypointIndex;
    float percentBetweenWaypoints;
    float nextMoveTime;
    Vector3 velocity;

    //***Spencer Adds: added this variable because gameobjects allready have a vriable named 'collider' behind the scenes
    //I am thinking you just forgot to declare it as a new variable
    public BoxCollider2D myCollider;

    //List<CharacterMovement> passengerList = new List<CharacterMovement>();

    private void Start()
    {
        globalWaypoints = new Vector3[localWaypoints.Length];
        for (int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }
        //Spencer: changed name to reflect new added variable
        myCollider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    private void FixedUpdate()
    {
        velocity = movePlatform();
        transform.Translate(velocity);
        UpdateRaycastOrigins();
        ListPassengers();
        MovePassengers();
    }

    //moves between points using precents and lerp
    private Vector3 movePlatform()
    {
        if (Time.time < nextMoveTime)
        {
            return Vector3.zero;
        }

        fromWaypointIndex %= globalWaypoints.Length;
        int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
        float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
        percentBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;
        percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
        float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);

        Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easedPercentBetweenWaypoints);

        if (percentBetweenWaypoints >= 1)
        {
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;

            if (!cyclic)
            {
                if (fromWaypointIndex >= globalWaypoints.Length - 1)
                {
                    fromWaypointIndex = 0;
                    System.Array.Reverse(globalWaypoints);
                }
            }
            nextMoveTime = Time.time + waitTime;
        }

        return newPos - transform.position;
    }

    float Ease(float x)
    {
        float a = easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    //add passengers to list if they have charactermovment component
    public void ListPassengers()
    {
        //passengerList.Clear();
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.topLeft;
            float raylength = skinWidth + Mathf.Abs(velocity.y) + .01f;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, raylength, passengerMask);

            Debug.DrawRay(rayOrigin, Vector2.up * (raylength), Color.yellow);

            /*if (hit && hit.collider.GetComponent<CharacterMovement>() != null)
            {
                CharacterMovement temp = hit.collider.GetComponent<CharacterMovement>();
                if (!passengerList.Contains(temp))
                {
                    passengerList.Add(temp);
                }
            }*/
        }
    }

    //moves passengers
    public void MovePassengers()
    {
        /*for (int i = 0; i < passengerList.Count; i++)
        {
            if (passengerList[i].sides.bot)
            {
                passengerList[i].move(velocity);
            }
        }*/
    }
}