using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntController : MonoBehaviour
{
    /*
     * By: Parker Allen
     * Version: 1.0
     * 
     * Setup:
     *  attach prefab to spike, seed
     *  attach gameobject to shield
     */

    private EntMovement eM;             //EntMovement
    public LayerMask collisionMask;     //collision mask for ground
    public Transform playerPosition;    //player transform

    private bool idling;                //is idling
    private bool attacking;             //is attacking
    private bool spikeAttacking;        //is spikeattackin
    private bool spikeCoolDown;         //is spike attack on cooldown
    private bool seedAttacking;         //is seed attacking
    private bool seedCoolDown;          //is seed attack on cooldown
    private bool shielding;             //is shield active

    private bool doingAction;           //is doing an action
    private bool pauseBetAction;        //is puseing between actions

    public GameObject spike;            //prefab with EntSpike
    public GameObject seed;             //prefab with EntSeed
    public GameObject shield;           //child object

    private Vector3 direction;          //direction to player

    /**************************************************************************************************/

    void Start()
    {
        eM = GetComponent<EntMovement>();   //set EntMovement
    }

    /**************************************************************************************************/

    public void Update()
    {
        if (!doingAction)
        {
            SetDirection();     //not doing action the set direction
        }
        if (seedAttacking)
        {
            seedAttacking = eM.SeedAttack();        //set seed attacking to false when animation ends
        }
        else if (spikeAttacking)
        {
            spikeAttacking = eM.SpikeAttack();      //set spike attaking to false when animation ends
        }
        else if (attacking)
        {
            attacking = eM.Attack();                //set attacking to false when animation ends
        }
        else
        {
            if (!pauseBetAction)
            {
                StartCoroutine(SetAction());        //set next action
            }
            eM.Idle();                              //other wise Idle
        }
    }

    /**************************************************************************************************/
    //sets the action after a pause
    IEnumerator SetAction()
    {
        pauseBetAction = true;                  //stops it from calling this again
        yield return new WaitForSeconds(1);     //the pause
        pauseBetAction = false;                 //this can be called again

        if(!seedCoolDown && InRangedAttackRange())
        {
            seedAttacking = true;               //seed attack if player in range
            StartCoroutine(SeedCoolDown());     //start coroutine for cooldown
            shield.SetActive(true);             //shield is active
        }
        else if(!spikeCoolDown && InRangedAttackRange())
        {
            spikeAttacking = true;              //spike attack if player in range
            StartCoroutine(SpikeCoolDown());    //start coroutine for cooldown
            shield.SetActive(false);            //deactivate shield
        }
        else if (InMeleeAttackRange())
        {
            attacking = true;                   //attack if player in range
            shield.SetActive(false);            //deactivate shield
        }
        else
        {
            shield.SetActive(true);             //else shield is active
        }

        doingAction = seedAttacking || spikeAttacking || attacking;     //doing action if attacking
        if (doingAction)
        {
            eM.ZeroCounter();           //reset counter if doing an action
        }
        StopCoroutine(SetAction());     //stop coroutine
    }

    /**************************************************************************************************/
    //sets the direction based on player position
    public void SetDirection()
    {
        direction = eM.FlipEnt(playerPosition.position.x > transform.position.x) ? Vector3.right : Vector3.left;
    }

    /**************************************************************************************************/
    //test if player in melee range
    public bool InMeleeAttackRange()
    {
        float dis = Mathf.Abs(playerPosition.position.x - transform.position.x);        //distance to player
        if(dis < 7)
        {
            return true;        //return true if within 7
        }
        return false;           //return false
    }

    /**************************************************************************************************/
    //test if player in ranged attack range
    public bool InRangedAttackRange()
    {
        float dis = Mathf.Abs(playerPosition.position.x - transform.position.x);        //distance to player
        if (dis < 17)
        {
            return true;        //return true if within 17
        }
        return false;           //return flase
    }

    /**************************************************************************************************/
    //spawns a spike on ground and the number that will spawn in a direction
    public void SpawnSpike()
    {
        Vector3 rayOrigin = transform.position + direction * 5;                             //origin for ray
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 10, collisionMask);   //raycast
        if (hit)
        {
            Vector2 spikeOrigin = hit.point + Vector2.up * 1.25f;                                               //origin for spike
            EntSpike temp = Instantiate(spike, spikeOrigin, transform.rotation).GetComponent<EntSpike>();       //instantiate spike
            StartCoroutine(temp.SpawnNextSpike(4, direction * 3));                                              //start coroutine to spawn 4 spike 3 apart
        }
    }

    /**************************************************************************************************/
    //shoot 5 seeds stright up
    public IEnumerator SpawnSeed()
    {
        for(int i = 0; i < 5; i++)
        {
            float ran = Random.Range(0, direction.x * 5);       //random number between 0 and 5 or -5 and 0, based on direction
            Quaternion rot = Quaternion.Euler(0, 0, ran);       //rotation from up by the random number

            Vector3 playerPos = playerPosition.position + new Vector3(ran - (direction.x * 2), 15, 0);  //position second seed spawns from above player 
                                                                                                        //shifted horizontaly by the random number

            EntSeed temp = Instantiate(seed, transform.position, rot).GetComponent<EntSeed>();      //instantiate the seed
            temp.SetDown(2, playerPos, playerPosition, true);                                       //set the duration second spawn location, player transform, and spawn another

            yield return new WaitForSeconds(.2f);                                                   //wait to fire again
        }
        StopCoroutine(SpawnSeed());         //stop coroutine
    }

    /**************************************************************************************************/
    //cool down for the spike
    IEnumerator SpikeCoolDown()
    {
        spikeCoolDown = true;                   //spike on cooldown
        yield return new WaitForSeconds(45);    //wait 25s
        spikeCoolDown = false;                  //spike off cooldown
        StopCoroutine(SpikeCoolDown());         //stop coroutine
    }

    /**************************************************************************************************/
    //cool down for the seeds
    IEnumerator SeedCoolDown()
    {
        seedCoolDown = true;                    //seeds on cooldown
        yield return new WaitForSeconds(45);    //wait 45s
        seedCoolDown = false;                   //seeds off cooldown
        StopCoroutine(SeedCoolDown());          //stop coroutine
    }
}
