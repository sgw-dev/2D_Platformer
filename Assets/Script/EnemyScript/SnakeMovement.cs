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
     *  spit rate = 120
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
    public SpriteRenderer head;                               //sprite of head

    private Animator anim;                                  //Spencer: Sanke head animator
    private LayerMask mask;
    private float waitTime;
    private bool canAttack = false;

    public int spitRate;                                    //time between each shoot
    public int counter;                                    //counter for time

    //Spencer: Add start function to set anim variable
    void Start() {
        anim = GameObject.Find("Snake/Head").GetComponent<Animator>();
        mask = LayerMask.GetMask("PlayerLayer");
        waitTime = 45f;
        searchDistance = 10;
        spitRate = 120;
        EnemyStart();

    }

    /**************************************************************************************************/

    void Update()
    {
        head.flipX = transform.position.x < playerPosition.position.x;        //flips the sprite
        //goes up to the wait time
        if (counter >= spitRate )
        {
            //starts the animation
            if (!canAttack)
            {
                canAttack = true;
                anim.SetTrigger("Attack");
                counter++;
            }
            //waits until the animation is over then attack
            else if (counter >= (spitRate + waitTime) && SnakeSight())
            {
                canAttack = false;
                Spit();                                                         //Shoot projectile
            }
            else if (counter < (spitRate + waitTime)) {
                counter++;
            }
            
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
                                                                   //Spencer: changed to mask, wasn't seeing the player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, searchDistance, mask);      //raycast to target
        Debug.DrawRay(transform.position, dir * searchDistance, Color.black);
        if (hit) {
            
        }
        if (hit && hit.transform.CompareTag(playerPosition.tag))                                        //raycast hit player
        {
            target = hit.point;                                                                    //set target to players position
            return true;                                                                          //enemy does have line of sight on player
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
