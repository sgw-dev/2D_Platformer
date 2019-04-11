using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This only needs 2 colliders 1 for the whole object and 1 for interact area
 * does not use wall tags 
 */

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D rb;

    public Collider2D interactCollider;

    public float speedx;
    public float jumpPower;
    public float wallJumpPower;

    public int maxNumOfJumps;
    public int numOfJumps;

    public bool onGround;
    public bool onWall;
    public bool inAir;

    public string tagOfGround;
    public float onWallDrag;
    public float normalDrag;

    public string interactKey;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

    //diable/enable collider used for interaction
    public void interact()
    {
        interactCollider.enabled = !interactCollider.enabled;
    }

    //determines if player is on a wall or ground
    private void playerState()
    {
        if (rb.velocity.y == 0 && rb.velocity.x != 0)
        {
            onGround = true;
            onWall = false;
            inAir = false;
            numOfJumps = maxNumOfJumps;
        }
        else
        {
            onGround = false;
            if (onWall)
            {
                rb.drag = onWallDrag;
            }
            else
            {
                rb.drag = normalDrag;
            }
        }
    }

    //if the player is on the ground use velocity else use addforce
    public void playerMovement()
    {
        Vector2 v2 = new Vector2(playerMovementX(), playerMovementY());
        if (onGround)
        {
            rb.velocity = v2;
        }
        else
        {
            rb.AddForce(v2);
        }
    }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tagOfGround))
        {
            onGround = true;
            onWall = true;
            inAir = false;
        }
    }
}
