using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerMovement : MonoBehaviour {

    public float maxSpeed;//3-5
    public float groundSpeed;//5-8
    public float sprintSpeed;//8-12
    public float wallClimb;//4-8
    public float airControl;//3

    public float gravity;//-10
    public float wallGravity;//-1 - -3
    public float jumpSpeed;//8-12
    public int maxNumOfJumps;//2
    private int numOfJumps;
    public bool stopXOnJump;//true

    public string sprintButton;
    private bool sprinting;

    private Vector2 input;

    public Vector3 velocity;//leave alone

    //subobject with a collider
    public Collider2D interactCollider;
    public string interactKey;

    CharacterMovement controller;

    void Start()
    {
        controller = GetComponent<CharacterMovement>();
    }

    private void FixedUpdate()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        isSprinting();
        velocityX(input.x);
        velocityY();
        controller.inputMove(velocity * Time.deltaTime);
        interact();
    }

    //test if sprinting
    private void isSprinting()
    {
        if (Input.GetKey(sprintButton) && controller.sides.bot)
        {
            sprinting = true;
        }
        else
        {
            sprinting = false;
        }
    }

    //if on ground then velocity x = groundspeed or sprintspeed
    //else velocity += aircontrol
    private void velocityX(float dir)
    {
        if (controller.sides.bot)
        {
            if (sprinting)
            {
                velocity.x = dir * sprintSpeed;
            }
            else
            {
                velocity.x = dir * groundSpeed;
            }
            numOfJumps = maxNumOfJumps;
        }
        else
        {
            velocity.x += airSpeedX(dir * airControl * Time.deltaTime);
        }
    }

    //if air speed is greater than maxspeed then slow down velocity x to max speed
    private float airSpeedX(float v)
    {
        if (Mathf.Abs(velocity.x) > maxSpeed)
        {
            if (Mathf.Sign(v) == Mathf.Sign(velocity.x))
            {
                v = 0;
            }
            v += (maxSpeed - Mathf.Abs(velocity.x)) * Mathf.Sign(velocity.x) * Time.deltaTime;
        }
        return v;
    }

    private void velocityY()
    {
        //velocity y = 0 if on ground or ceiling
        if (controller.sides.bot || controller.sides.top)
        {
            velocity.y = 0;
        }

        //when sprinting up to wall, climbs wall(not a jump)
        if (sprinting && (controller.sides.left || controller.sides.right))
        {
            velocity = Vector3.up * wallClimb;
        }

        //jump
        else if (Input.GetKeyDown("space"))
        {
            jump();
        }

        //allows player to drop through platform
        if (input.y < 0)
        {
            controller.sides.dropTroughPlatform = true;
        }
        else
        {
            controller.sides.dropTroughPlatform = false;
        }

        //add gravity
        velocity.y += (getGravity() * Time.deltaTime);
    }

    private void jump()
    {
        //if on ground normal jump
        if (controller.sides.bot)
        {
            numOfJumps--;
            velocity = new Vector2(input.x, 1) * jumpSpeed;
        }

        //if on wall can jump opposite of wall(dosen't count as jump)
        else if ((controller.sides.left && input.x > 0) ||
            (controller.sides.right && input.x < 0))
        {
            velocity = new Vector2(input.x, input.y) * jumpSpeed;
        }

        //else special jump
        else if (numOfJumps > 0)
        {
            velocity = eightDirJump();
        }
    }

    //allows jumps in 8 directions (N, NE, E, SE, S, SW, W, NW)
    private Vector2 eightDirJump()
    {
        numOfJumps--;
        if (input.x != 0 || input.y != 0)
        {
            return new Vector2(input.x, input.y) * jumpSpeed;
        }
        return Vector2.up * jumpSpeed;
    }

    //determine which gravity to use (wall or normal)
    private float getGravity()
    {
        if ((controller.sides.right || controller.sides.left) && velocity.y < 0)
        {
            return wallGravity;
        }
        else
        {
            return gravity;
        }
    }

    public void interact()
    {
        if (Input.GetKey(interactKey))
        {
            interactCollider.enabled = true;
        }
        else interactCollider.enabled = false;
    }
}
