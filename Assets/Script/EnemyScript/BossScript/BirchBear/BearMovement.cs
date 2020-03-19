using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearMovement : BossMovement
{
    BossPathing2 bossPathing;
    public Collider2D biteCollider;
    private float distanceToBite;

    private void Start()
    {
        bossPathing = GetComponent<BossPathing2>();
        BossMovementStart();

        distanceToBite = biteCollider.bounds.size.x;
        bossPathing.SetOffset(distanceToBite);
    }

    private void FixedUpdate()
    {
        if (GetCharacterMovement().sides.bot)
        {
            SetDirection(bossPathing.GetPoint());
        }
        ResetState();
        CalculateMoveType(bossPathing.myBounds.xhalf);
        HorizontalMovment();
        VerticalMovement();

        Vector3 temp = GetVelocity();
        //transform.Translate(temp);
        transform.Translate(new Vector3(Mathf.Abs(temp.x) * -1, temp.y));
    }

    public void Update()
    {
        if (bossPathing.FindDestination(direction.x))
        {
            bossPathing.FindPath();
            bossPathing.NextPoint();
        }

        bossPathing.DebugPoint();
        bossPathing.DebugPoints();
        DebugDirection();
    }
}
