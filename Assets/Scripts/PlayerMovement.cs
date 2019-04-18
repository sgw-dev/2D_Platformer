using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Parker
 * 
 * Setup:
 *  attach to player
 *  create 4 empty objects that children to player
 *  add 2d box collider to each child
 *  resize colliders
 *  check is trigger
 * 
 * Notes:
 *  commented numbers are values I used
 * 
 */

public class PlayerMovement : MonoBehaviour 
{
    private Rigidbody2D rb;

    //offset x -.09
    //size x .005, y .16
    public Collider2D leftCollider;

    //offset x .09
    //size x .005, y .16
    public Collider2D rightCollider;

    //offset y -.09
    //size x .16, y .005
    public Collider2D groundCollider;
    public Collider2D interactCollider;

    public float maxSpeed; //5
    public float speedx; //5
    public float jumpPower; //500
    public float wallJumpPower; //250

    public int maxNumOfJumps;
    private int numOfJumps;

    private bool onGround;
    private bool onWall;
    private bool inAir = true;

    public string tagOfGround;
    public float onWallDrag; //6
    public float normalDrag; //0

    public string interactKey;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!rb.freezeRotation)
        {
            rb.freezeRotation = true;
        }
    }

    public void FixedUpdate()
    {
        playerState();
        playerMovement();
        if (Input.GetKeyDown(interactKey))
        {
            interact();
        }
        if (Input.GetKeyUp(interactKey))
        {
            interact();
        }
    }

    //check which drag to use and reset jumps when on ground
    private void playerState()
    {
        if (onWall)
        {
            rb.drag = onWallDrag;
        }
        else if (rb.velocity.y == 0)
        {
            onGround = true;
            onWall = false;
            inAir = false;
            numOfJumps = maxNumOfJumps;
        }
        else
        {
            rb.drag = normalDrag;
        }
    }

    //enable/disable interact collider
    public void interact()
    {
        interactCollider.enabled = !interactCollider.enabled;
    }

    //create vector then check how to use it and clamps speed
    public void playerMovement()
    {
        Vector2 v2 = clampSpeed(new Vector2(playerMovementX(), playerMovementY()));
        if (onGround)
        {
            rb.velocity = v2;
        }
        else
        {
            rb.AddForce(v2);
        }
    }

    //clamps speed
    private Vector2 clampSpeed(Vector2 v)
    {
        if(Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            return new Vector2(0, v.y);
        }
        return v;
    }

    //returns speed for x and rotates object
    private float playerMovementX()
    {
        if (Input.GetKey("a"))
        {
            faceLeft();
            return -speedx;
        }
        if (Input.GetKey("d"))
        {
            faceRight();
            return speedx;
        }
        return 0;
    }

    private void faceLeft()
    {
        transform.eulerAngles = new Vector3(0, 180, 0);
    }

    private void faceRight()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    //return speed for y
    private float playerMovementY()
    {
        if (Input.GetKeyDown("space"))
        {
            if (onWall)
            {
                return wallJump();
            }
            else if (numOfJumps > 0)
            {
                return jump();
            }            
        }
        return 0;
    }

    //jump in direction opposite of direction player is facing
    private float wallJump()
    {
        rb.velocity = Vector2.zero;
        if (transform.rotation.y != 0)
        {
            rb.AddForce(new Vector2(wallJumpPower, jumpPower));
        }
        else
        {
            rb.AddForce(new Vector2(-wallJumpPower, jumpPower));
        }
        return .1f;
    }

    private float jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        onGround = false;
        numOfJumps--;
        return jumpPower;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onGround = false;
        onWall = false;
        inAir = true;
    }

    //first check if on ground then check if on wall and not ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tagOfGround))
        {
            if (groundCollider.IsTouching(collision.collider))
            {
                onGround = true;
                onWall = false;
                inAir = false;
            }
            else if ((leftCollider.IsTouching(collision.collider) ||
                rightCollider.IsTouching(collision.collider)) && 
                !onGround)
            {
                onGround = false;
                onWall = true;
                inAir = false;
            }
        }
    }
}
