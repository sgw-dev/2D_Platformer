﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{
    private Skeleton[] skeleton;
    /*
     * 18
     * LowerBody
     * Upperbody
     * 
     * Neck
     * Head
     * Ears
     * 
     * Front leg top
     * front leg middle
     * fornt leg bot
     * 
     * back front leg top
     * back front leg middle
     * back front leg bot
     * 
     * hind leg top
     * hind leg middle
     * hind leg bot
     * 
     * back hind leg top
     * back hind leg middle
     * back hind leg bot
     * 
     * tail
     */

    private BossAnimation run1, run2, run3, run4;
    private BossAnimation[] run;
    private BossAnimation walk1, walk2, walk3, walk4;
    private BossAnimation[] walk;
    private BossAnimation idle1, idle2;
    private BossAnimation[] idle;
    private BossAnimation jump1, jump2, jump3;
    private BossAnimation[] jump;
    private BossAnimation fall1;
    private BossAnimation[] fall;

    private BossAnimation flop1, flop2;
    private BossAnimation basicAttack1;
    private BossAnimation leaf1, leaf2;

    public bool t;

    private BossAnimationController bossAnimationController;
    private BearMovement bearMovement;
    public Animator headAnimator;

    private void Awake()
    {
        skeleton = GetComponentsInChildren<Skeleton>();
        bossAnimationController = GetComponent<BossAnimationController>();
        bearMovement = GetComponent<BearMovement>();
    }

    private void Start()
    {
        SetRunAnimation();
        SetIdleAnimation();
        SetJumpAnimation();
        SetFallAnimation();
        SetWalkAnimation();
        SetFlopAttackAnimation();
        SetBasicAttackAnimation();
        SetLeafAttackAnimation();
        bossAnimationController.SetInterruptableActions(run);
    }

    private void Update()
    {
        SetInterruptableActionToState(bearMovement);
        if (bossAnimationController.EmptyPriority())
        {
            
        }
        bool isFlip = (bearMovement.direction == Vector3.right) ? true : false;
        bossAnimationController.DoAction(skeleton, isFlip);
    }

    public void AddJumpPriority()
    {
        bossAnimationController.AddPriorityAction(jump1);
        bossAnimationController.AddPriorityAction(jump2);
    }

    public void AddBasicAttackPriority()
    {
        headAnimator.Play("BearBite", 0);
        bossAnimationController.AddPriorityAction(basicAttack1);
    }

    public void AddSlamAttackPriority()
    {
        bossAnimationController.AddPriorityAction(flop1);
        bossAnimationController.AddPriorityAction(flop2);
    }
    //Spencer
    public void AddFlopPriority()
    {
        bossAnimationController.AddPriorityAction(jump2);
    }

    public void AddLeafAttackPriority()
    {
        bossAnimationController.AddPriorityAction(leaf1);
        bossAnimationController.AddPriorityAction(leaf2);
    }

    public void SetInterruptableActionToState(BearMovement bm)
    {
        if (bm.jumping)
        {
            bossAnimationController.SetInterruptableActions(jump);
        }
        else if (bm.falling)
        {
            bossAnimationController.SetInterruptableActions(fall);
        }
        else if (bm.sprinting)
        {
            bossAnimationController.SetInterruptableActions(run);
        }
        else if (bm.walking)
        {
            bossAnimationController.SetInterruptableActions(walk);
        }
        else
        {
            bossAnimationController.SetInterruptableActions(idle);
        }
    }

    public void SetIdleAnimation()
    {
                                        //body  h           f          bf           h           bh          t
        float[] angle2 = new float[] { 0, 0,    0, 0, 0,     0, 0, 0,    0, 0, 0,    0, 0, 0,    0, 0, 0,    0 };
        float[] angle1 = new float[] { 0, -2,    2, 0, 0,    2, 0, 0,    2, 0, 0,    0, 0, 0,    0, 0, 0,    0 };
        idle1 = new BossAnimation(angle2, .3f);
        idle2 = new BossAnimation(angle1, 3);

        idle = new BossAnimation[] { idle1, idle2 };
    }

    private void SetWalkAnimation()
    {
        float[] angle1 = new float[] { 0, 0,     10, -10, 0,     10, 20, -10,   -10, -20, 10,   -10, -10, 20,    0, 0, 0,       0 };
        float[] angle2 = new float[] { 0, 0,     0, 0, 0,       0, 0, 0,        -30, 40, 0,     20, -10, 0,     -30, 20, 0,     0 };
        float[] angle3 = new float[] { 0, 0,     10, -10, 0,    -10, -20, 10,   10, 20, -20,    0, 0, 0,        -10, -10, 20,   0 };
        float[] angle4 = new float[] { 0, 0,     0, 0, 0,       -30, 40, 0,     0, 0, 0,        -30, 20, 0,     20, 10, 0,      0 };

        walk1 = new BossAnimation(angle1, .2f);
        walk2 = new BossAnimation(angle2, .2f);
        walk3 = new BossAnimation(angle3, .2f);
        walk4 = new BossAnimation(angle4, .2f);

        walk = new BossAnimation[] { walk1, walk2, walk3, walk4 };
    }

    private void SetRunAnimation()
    {
        float[] angles1 = new float[] { 10, -10,     10, -10, 0,    -40, 100, -40,  -30, 80, -20,   30, -10, 20,    10, -20, 30,    0 };
        float[] angles2 = new float[] { 0, 0,        0, 0, 0,    50, 40, -40,    40, 20, -20,       -60, 20, -20,   -40, 0, -10,    0 };
        float[] angles3 = new float[] { -10, 10,     10, -10, 0,    -10, 10, 0,     20, 0, 0,       0, 50, -10,     0, 10, -10,     0 };
        float[] angles4 = new float[] { 10, -10,     0, 0, 0,    -60, 60, -40,   -50, 10, 0,        80, -40, 40,    60, 40, -80,    0 };

        run1 = new BossAnimation(angles1, .1f);
        run2 = new BossAnimation(angles2, .1f);
        run3 = new BossAnimation(angles3, .2f);
        run4 = new BossAnimation(angles4, .1f);

        run = new BossAnimation[] { run1, run2, run3, run4 };
    }

    private void SetJumpAnimation()
    {
        float[] angle1 = new float[] { 30, 0, -10, 0, 50, -30, 20, 0, -30, 60, -90, 60, -80, 20, 50, -90, 10, 0 };
        float[] angle2 = new float[] { 20, 10, -30, 20, 50, 40, 40, -40, 50, 50, -40, -30, -10, 0, -20, -10, 0, 0 };
        float[] angle3 = new float[] { 0, 10, -30, 20, 50, 40, 40, -40, 50, 50, -40, -30, -10, 0, -20, -10, 0, 0 };

        jump1 = new BossAnimation(angle1, .2f);
        jump2 = new BossAnimation(angle2, .1f);
        jump3 = new BossAnimation(angle3, 2);

        jump = new BossAnimation[] { jump3 };
    }

    private void SetFallAnimation()
    {
        float[] angle1 = new float[] { -30, 10, -20, 10, 20, 10, 30, -40, 20, 40, -40, 10, 10, 10, 10, 10, 10, 0 };

        fall1 = new BossAnimation(angle1, 2);

        fall = new BossAnimation[] { fall1 };
    }

    private void SetFlopAttackAnimation()
    {
        float[] angle1 = new float[] { -80, 0,    30, 30, 0,     -20, -20, 40,    -20, -20, 40,    50, 30, 0,    50, 30, 0,    0 };
        float[] angle2 = new float[] { 20, -20,    0, 0, 0,     -40, -40, 70,    -40, -40, 70,    -60, 40, 0,    -60, 40, 0,    0 };

        flop1 = new BossAnimation(angle1, 1f);
        flop2 = new BossAnimation(angle2, .2f);
    }

    private void SetBasicAttackAnimation()
    {
        float[] angle1 = new float[] { -10, 0,    -30, 50, 0,     -30, -20, 60,    0, 0, 0,    20, -10, 0,    30, -10, 0,    0 };

        basicAttack1 = new BossAnimation(angle1, .2f);
    }

    private void SetLeafAttackAnimation()
    {
        float[] angle1 = new float[] { 0, 0,    -30, -20, 0,     0, 0, 0,    0, 0, 0,    0, 0, 0,    0, 0, 0,    0 };
        float[] angle2 = new float[] { 0, 0,    -35, -25, 0,     0, 0, 0,    0, 0, 0,    0, 0, 0,    0, 0, 0,    0 };

        leaf1 = new BossAnimation(angle1, .3f);
        leaf2 = new BossAnimation(angle2, 1.5f);
    }
}
