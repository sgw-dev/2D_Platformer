using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : EnemyPathing
{
    /*
     * By: Parker Allen
     * Version: 1.0
     * 
     * Setup:
     *  sr = sprite renderer of head
     *  search distance = 10
     *  wraith of the tooth spitting snake = prefab of projectile
     *  tooth origin = empty child object's
     *  spit rate = 1
     *  
     * BoxCollider2D
     * RigidBody2D
     *  mass = ?
     *  gravity = 1
     *  
     * issues:
     *  shooting the tooth cause snake to move backwards
     */

    public GameObject wraithOfTheToothSpittingSnake;        //projectile
    public Transform toothOrigin;                           //place where projectile spawns
    public SpriteRenderer sr;                               //sprite of head

    public int spitRate;                                    //time between each shoot
    private int counter;                                    //counter for time

    /**************************************************************************************************/

    void Update()
    {
        sr.flipX = transform.position.x < playerPosition.position.x;        //flips the sprite
        if (counter >= spitRate && SnakeSight())
        {
            Spit();                                                         //Shoot projectile
        }
        else if(counter < spitRate)
        {
            counter++;                                                      //counter for fire rate
        }
    }

    /**************************************************************************************************/
    //snake has line of sight on player
    private bool SnakeSight()
    {
        Vector2 dir = (playerPosition.position - transform.position).normalized;                            //direction to player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, searchDistance, base.layerMask);      //raycast to target
        //Debug.DrawRay(transform.position, dir * searchDistance, Color.black);

        if (hit && hit.transform.CompareTag(playerPosition.tag))                                        //raycast hit player
        {
            base.target = hit.point;                                                                    //set target to players position
            return true;                                                                                //enemy does have line of sight on player
        }
        return false;
    }

    /**************************************************************************************************/
    //shoot teeth at the player
    private void Spit()
    {
        counter = 0;

        Vector2 dir = (target - (Vector2)transform.position).normalized;                            //direction to player
        float angle = Vector3.SignedAngle(Vector3.up, dir, Vector3.forward);                        //converts vector to an angle

        Instantiate(wraithOfTheToothSpittingSnake, toothOrigin.position, Quaternion.Euler(0, 0, angle));    //Spawn projectile
    }
}
