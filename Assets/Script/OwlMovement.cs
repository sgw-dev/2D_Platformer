using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlMovement : FlyingEnemyPathing
{

    private Vector2 direction;

    public void FixedUpdate()
    {
        rb.velocity = direction = Vector2.zero;
        //direction = Avoid();
        if (!needToAvoid)
        {
            if (lineOfSight())
            {
                direction = target;
            }
            else if (perch != Vector3.zero)
            {
                direction = Perch();
            }
            else
            {
                perch = FindGround();
            }
        }
        if (direction != Vector2.zero)
        {
            Move(direction);
        }
        LockRotation();
        Debug.DrawLine(transform.position, direction, Color.cyan);
    }
}