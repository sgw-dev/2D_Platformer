using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntMovement : MonoBehaviour
{
    /*
     * By: Parker Allen
     * Version: 1.0
     * 
     * Setup:
     *  this goes in the parent of all the skeletons of an enemy
     *  attach skeletons to each child that you want to move
     *  place those in the this skeleton list
     */

    //these are only for organization mostly
    public Skeleton[] leg;
    public Skeleton chest;
    public Skeleton head;
    public Skeleton[] frontArm;
    public Skeleton[] backArm;


    private Skeleton[] body;        //the whole list of skeletons

    private EntAnimation idle;      //idle animation

    private EntAnimation attack;    //attack animation

    private EntAnimation spikeAttack;   //spike attack animation

    private EntAnimation seedAttack;    //seed attack animation

    private EntController eC;           //ent controller
    private int counter = 1;            //counter

    /**************************************************************************************************/

    public void Start()
    {
        eC = GetComponent<EntController>();     //sets the ent controller

        //place the skeletons in body
        body = new Skeleton[] {leg[0], leg[1], chest, head, frontArm[0], frontArm[1], frontArm[2],
            backArm[0], backArm[1], backArm[2]};

        //set the animations
        IdleAnimation();            //comments about it are mostly in here
        SpikeAttackAnimation();
        AttackAnimation();
        SeedAttackAnimation();
    }

    /**************************************************************************************************/
    // rotates ent on y axis to flip it
    public bool FlipEnt(bool f)
    {
        if (f)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);   //flip it
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);     //back to normal
        }

        for(int i = 0; i < body.Length; i++)
        {
            body[i].isFliped(f);                //flip the angle in the skeletons
        }
        return f;                               //return
    }

    /**************************************************************************************************/
    //sets idle animation
    private void IdleAnimation()
    {
        //these are the angle each skeleton will rotate to 
        idle.start = new float[] { 0, 0, 5, 0, 7, 0, 0, 0, 0, 0 };
        idle.end = new float[] { 5, -5, 0, -5, 0, 0, 0, 5, 0, 0 };

        idle.time = new float[] { 3, 3 };   //time to get between the angles

        idle.counterRotation = new bool[10];        //which children will counter rotate the parents
        for (int i = 0; i < body.Length; i++)
        {
            if (idle.end[i] != 0 || idle.start[i] != 0)     //any skeleton dosen't change rotation
            {
                idle.counterRotation[i] = true;             //counter rotate
            }
        }
    }

    /**************************************************************************************************/
    //same as idle but different angles
    private void AttackAnimation()
    {
        attack.start = new float[] { -20, 20, -20, 20, -150, -150, -150, -150, -150, -150 };
        attack.end = new float[] { 30, -30, 40, 0, -70, -30, -20, -70, -30, -20 };
        attack.time = new float[] { 3, .25f };
        attack.counterRotation = new bool[10];
        for (int i = 0; i < body.Length; i++)
        {
            if (attack.end[i] != 0 || attack.start[i] != 0)
            {
                attack.counterRotation[i] = true;
            }
        }
    }

    /**************************************************************************************************/
    //same as idle but different angles
    private void SpikeAttackAnimation()
    {
        spikeAttack.start = new float[] { -20, 10, 5, 15, 75, 20, 0, -30, 30, 0 };
        spikeAttack.end = new float[] { -30, 50, 20, 5, -60, 50, 0, 40, 0, 0 };
        spikeAttack.time = new float[] { 2.5f, .2f};
        spikeAttack.counterRotation = new bool[10];
        for (int i = 0; i < body.Length; i++)
        {
            if (spikeAttack.end[i] != 0 || spikeAttack.start[i] != 0)
            {
                spikeAttack.counterRotation[i] = true;
            }
        }
    }

    /**************************************************************************************************/
    //same as idle but different angles
    private void SeedAttackAnimation()
    {
        seedAttack.start = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        seedAttack.end = new float[] { -50, 70, 20, 0, -30, 30, 0, -30, 40, 0 };
        seedAttack.time = new float[] { 1, 3 };
        seedAttack.counterRotation = new bool[10];
        for (int i = 0; i < body.Length; i++)
        {
            if (seedAttack.end[i] != 0 || seedAttack.start[i] != 0)
            {
                seedAttack.counterRotation[i] = true;
            }
        }
    }

    /**************************************************************************************************/
    //returns boolean and actually moves the skeletons
    public bool Idle()
    {
        if(counter < idle.time[0] * 60)         //counter lower than first time
        {
            for(int i = 0; i < body.Length; i++)
            {
                if(counter == 1)                                        //does this once at beginning
                {
                    body[i].setAngle(idle.time[0], idle.end[i]);        //set the angle to rotate to
                }
                body[i].RotateSkeleton(idle.counterRotation[i]);        //rotate the skeleton
            }
        }
        else if (counter <= idle.time[0] * 120)         //counter lower than first and second time
        {
            for (int i = 0; i < body.Length; i++)
            {
                if(counter == idle.time[0] * 60)                        //does this once at beginning
                {
                    body[i].setAngle(idle.time[1], idle.start[i]);      //set the angle to rotate to
                }
                body[i].RotateSkeleton(idle.counterRotation[i]);        //rotate the skeleton
            }
        }
        else
        {
            counter = 0;            //reset idle
            return false;           //return flase for end of animation
        }
        counter++;                  //increment counter
        return true;                //return true still in animation
    }

    /**************************************************************************************************/
    //same as idle with noted differences
    public bool Attack()
    {
        if (counter < attack.time[0] * 60)
        {
            for (int i = 0; i < body.Length; i++)
            {
                if (counter == 1)
                {
                    body[i].setAngle(attack.time[0], attack.start[i]);
                }
                body[i].RotateSkeleton(attack.counterRotation[i]);
            }
        }
        else if (counter <= (attack.time[0] + attack.time[1]) * 60)
        {
            for (int i = 0; i < body.Length; i++)
            {
                if (counter == attack.time[0] * 60)
                {
                    body[i].setAngle(attack.time[1], attack.end[i]);
                }
                body[i].RotateSkeleton(attack.counterRotation[i]);
            }
        }
        else
        {
            counter = 0;
            return false;
        }
        counter++;
        return true;
    }

    /**************************************************************************************************/
    //same as idle with noted differences
    public bool SpikeAttack()
    {
        if (counter < spikeAttack.time[0] * 60)
        {
            for (int i = 0; i < body.Length; i++)
            {
                if (counter == 1)
                {
                    body[i].setAngle(spikeAttack.time[0], spikeAttack.start[i]);
                }
                body[i].RotateSkeleton(spikeAttack.counterRotation[i]);
            }
        }
        else if (counter < (spikeAttack.time[0] + spikeAttack.time[1]) * 60)
        {
            for (int i = 0; i < body.Length; i++)
            {
                if (counter == spikeAttack.time[0] * 60)
                {
                    body[i].setAngle(spikeAttack.time[1], spikeAttack.end[i]);
                }
                body[i].RotateSkeleton(spikeAttack.counterRotation[i]);
            }
        }
        else if (counter <= (spikeAttack.time[0] * 2 + spikeAttack.time[1]) * 60)       //counter less than first * 2 and second time
        {
            if (counter < (spikeAttack.time[0] + spikeAttack.time[1] * 2) * 60)         //does this once at beginning
            {
                eC.SpawnSpike();                                                        //spawn spikes and pause
            }
        }
        else
        {
            counter = 0;
            return false;
        }
        counter++;
        return true;
    }

    /**************************************************************************************************/
    //same as idle with noted differences
    public bool SeedAttack()
    {
        if(counter < (seedAttack.time[0]) * 60)
        {
            for (int i = 0; i < body.Length; i++)
            {
                if (counter == 1)
                {
                    body[i].setAngle(seedAttack.time[0], seedAttack.end[i]);
                }
                body[i].RotateSkeleton(seedAttack.counterRotation[i]);
            }
        }
        else if(counter < (seedAttack.time[0] + seedAttack.time[1]) * 60)
        {
            if(counter == (seedAttack.time[0] + seedAttack.time[1] - 1) * 60)       //does this once at beginning
            {
                StartCoroutine(eC.SpawnSeed());                                     //spawn seeds and pause
            }
        }
        else if (counter <= (seedAttack.time[0] * 2 + seedAttack.time[1]) * 60)     //counter less than first * 2 and second time
        {
            for (int i = 0; i < body.Length; i++)
            {
                if (counter == (seedAttack.time[0] + seedAttack.time[1]) * 60)
                {
                    body[i].setAngle(seedAttack.time[0], seedAttack.start[i]);
                }
                body[i].RotateSkeleton(seedAttack.counterRotation[i]);
            }
        }
        else
        {
            counter = 0;
            return false;
        }
        counter++;
        return true;
    }

    /**************************************************************************************************/
    //set counter to zero
    public void ZeroCounter()
    {
        counter = 0;
    }

    /**************************************************************************************************/
    //subclass for EntAnimations
    public struct EntAnimation
    {
        public float[] start;
        public float[] end;
        public float[] time;
        public bool[] counterRotation;
    }
}
