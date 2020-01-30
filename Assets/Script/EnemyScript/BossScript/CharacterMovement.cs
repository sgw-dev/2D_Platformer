using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CharacterMovement : RayCastController
{
    /// <summary>
        /// by: Parker
        /// Sebastian Lague has a really good tutorial on youtube that went over most of this
        /// 
        /// setup:
        /// 1. Attach this and playermovment3 to player
        /// 2. Does not need rigidbody but if using rigidbody set it to kinematic(should still work)
        /// 3. Player should not have same layer of ground
        /// 4. Platforms need to have very skinny colliders
        /// 
    /// </summary>
    
    SpriteRenderer spriteRen;
    public string tagOfPlatforms;

    public float maxClimbAngle;
    public float maxDesendAngle;

    private Collider2D collider;

    //tells what sides are colliding
    public CollisionSides sides;

    public void StartCharacter()
    {
        spriteRen = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public void StartCharacterSkeleton()
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public Vector3 inputMove(Vector3 velocity)
    {
        faceDir(velocity.x);
        UpdateRaycastOrigins();
        sides.reset();
        sides.velocityOld = velocity;
        return move(velocity);
    }

    public Vector3 inputMoveSkeleton(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        sides.reset();
        sides.velocityOld = velocity;
        return move(velocity);
    }

    //moves and flips sprite of object
    public Vector3 move(Vector3 velocity)
    {
        if (velocity.y < 0)
        {
            desendSlope(ref velocity);
        }
        if (velocity.x != 0)
        {
            horizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            verticalCollisions(ref velocity);
        }
        return velocity;
    }

    //make sprite face right to flip correctly
    private void faceDir(float x)
    {
        if (x < 0)
        {
            spriteRen.flipX = true;
        }
        else if (x > 0)
        {
            spriteRen.flipX = false;
        }
    }

    //test collision to left and right
    void horizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth + .01f;

        //tests each point for collisions Velocity x distance away
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            //helpful for debugging
            //Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit && !hit.transform.CompareTag(tagOfPlatforms))
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                //checks and deals with slopes
                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    if (sides.desending)
                    {
                        sides.desending = false;
                        velocity = sides.velocityOld;
                    }
                    float distToSlopeStart = 0;

                    //fixes any error going between 2 different slopes
                    if (slopeAngle != sides.oldSlopeAngle)
                    {
                        distToSlopeStart = hit.distance - skinWidth;
                        velocity.x -= distToSlopeStart * directionX;
                    }

                    //deal with climbing slopes
                    climbSlope(ref velocity, slopeAngle);
                    velocity.x += distToSlopeStart * directionX;
                }

                //if will hit wall or on slope set object right infront of wall
                if (!sides.climbing || slopeAngle > maxClimbAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    //move object up based on slope and movement x
                    if (sides.climbing)
                    {
                        velocity.y = Mathf.Tan(sides.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }
                    sides.left = directionX == -1;
                    sides.right = directionX == 1;
                }
            }
        }
    }

    //tests each point for collisions Velocity y distance away 
    void verticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            //helpful for degugging
            //Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            //if will hit ground/cieling or on slope set object right infront of ground/cieling
            if (hit && !(hit.transform.CompareTag(tagOfPlatforms) && (directionY > 0 || sides.dropTroughPlatform)))
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                //move object left/right based on slope and movement y
                if (sides.climbing)
                {
                    velocity.x = velocity.y / Mathf.Tan(sides.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }
                sides.bot = directionY == -1;
                sides.top = directionY == 1;
            }
        }
    }

    //climbs slopes
    private void climbSlope(ref Vector3 velocity, float slopeAngle)
    {
        float climbY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
        if (velocity.y <= climbY)
        {
            velocity.y = climbY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * velocity.x;
            sides.bot = true;
            sides.climbing = true;
            sides.slopeAngle = slopeAngle;
        }
    }

    //if object is falling test if slope below it
    private void desendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Tan(maxDesendAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x) + skinWidth;

        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, collisionMask);

        //Helpful for debugging
        //Debug.DrawRay(rayOrigin, Vector2.down * directionX * rayLength, Color.blue);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            //make sure not flat ground
            if (slopeAngle != 0)
            {
                velocity.y -= Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * velocity.x;
                sides.slopeAngle = slopeAngle;
                sides.desending = true;
                sides.bot = true;
            }
        }
    }

    //tells player state, which sides are colliding with object and desending or climbing
    public struct CollisionSides
    {
        public bool top, bot, left, right;

        public bool climbing, desending;
        public float oldSlopeAngle, slopeAngle;
        
        public bool dropTroughPlatform;

        public Vector3 velocityOld;

        public void reset()
        {
            top = bot = right = left = false;
            climbing = false;
            oldSlopeAngle = slopeAngle;
            slopeAngle = 0;
        }
    }
}
