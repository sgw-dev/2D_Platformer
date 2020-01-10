using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack
{
    /*
     * By: Parker Allen
     * Version: 1.0
     * 
     */

    public Collider2D collider;     //colliders for melee attacks
    public float range;             //range for ranged attacks
    public int damage;              //damage of attack
    public float delay;             //delay for attack
    public float coolDown;          //cooldown for attack
    public bool canUse;             //if the attack can be used again

    /**************************************************************************************************/
    //constructor for melee attacks
    public BossAttack(Collider2D c2d, int dmg, float de, float cd)
    {
        collider = c2d;
        damage = dmg;
        delay = de;
        coolDown = cd;
        canUse = true;
    }

    /**************************************************************************************************/
    //constructor for ranged attacks
    public BossAttack(float r, int dmg, float de, float cd)
    {
        range = r;
        damage = dmg;
        delay = de;
        coolDown = cd;
        canUse = true;
    }

    /**************************************************************************************************/
    //start cooldown for attack
    public IEnumerator StartCooldown()
    {
        canUse = false;
        yield return new WaitForSeconds(coolDown);
        canUse = true;
    }
}
