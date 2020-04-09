using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearMovement : BossMovement
{
    BossPathing2 bossPathing;
    public Collider2D biteCollider;
    public Collider2D slamCollider;
    private float distanceToBite;

    public BossAttack basicAttack, slamAttack, leafAttack;

    public GameObject leafPrefab;

    private BearController bearController;

    private void Start()
    {
        bearController = GetComponent<BearController>();
        bossPathing = GetComponent<BossPathing2>();
        BossMovementStart();

        distanceToBite = biteCollider.bounds.size.x;
        bossPathing.SetOffset(distanceToBite);
        SetAttacks();
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

        if (canAttack)
        {
            TryAttacking();
        }
    }

    private void TryAttacking()
    {
        if(leafAttack.canUse)
        {
            StartCoroutine(StartLeafAttack());
            return;
        }
        Player temp = GetPlayerInCollider(slamCollider);
        if (temp != null)
        {
            if (slamAttack.canUse)
            {
                StartCoroutine(StartSlamAttack());
            }
            else if (basicAttack.canUse)
            {
                StartCoroutine(StartBasicAttack());
            }
        }
    }

    IEnumerator StartSlamAttack()
    {
        StartCoroutine(AttackCooldown(1.4f));
        StartCoroutine(slamAttack.StartCooldown());
        bearController.AddSlamAttackPriority();
        yield return new WaitForSeconds(slamAttack.delay);
        Player temp = GetPlayerInCollider(slamCollider);
        if (temp != null)
        {
            temp.applyDamage(3);
        }
    }

    IEnumerator StartBasicAttack()
    {
        StartCoroutine(AttackCooldown(.4f));
        StartCoroutine(basicAttack.StartCooldown());
        bearController.AddBasicAttackPriority();
        yield return new WaitForSeconds(basicAttack.delay);

        Player temp = GetPlayerInCollider(biteCollider);
        if (temp != null)
        {
            temp.applyDamage(2);
        }
    }

    IEnumerator StartLeafAttack()
    {
        StartCoroutine(AttackCooldown(2.5f));
        StartCoroutine(leafAttack.StartCooldown());
        bearController.AddLeafAttackPriority();
        yield return new WaitForSeconds(leafAttack.delay);

        StartCoroutine(SpawnLeafs());
    }

    IEnumerator SpawnLeafs()
    {
        for(int i = 0; i <= 6; i++)
        {
            GameObject temp = Instantiate(leafPrefab) as GameObject;
            temp.transform.position = transform.position + Vector3.up * Random.Range(-1 , 3f);
            temp.transform.position -= direction * 10;
            temp.transform.rotation = Quaternion.Euler(0,0, (direction.x == 1) ? 0 : 180);
            yield return new WaitForSeconds(.2f);
        }
    }

    private void SetAttacks()
    {
        basicAttack = new BossAttack(biteCollider, 0, .1f, 2);
        slamAttack = new BossAttack(slamCollider, 0, 1.1f, 7);
        leafAttack = new BossAttack(20, 0, 1f, 14);
    }
}
