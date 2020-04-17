using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntMovement : BossPathing
{
    /*
     * By: Parker Allen
     * Version: 2.0
     * 
     */

    public BossAttack basicAttack, seedAttack, spikeAttack;

    public Collider2D basicAttackCollider;

    public GameObject spike, seed, shield;

    private EntController entController;

    //Spencer
    public bool dead;

    private void Start()
    {
        entController = GetComponent<EntController>();
        SetAttacks();
        BossStart();
        die();
    }

    private void Update()
    {
        if (canAttack && !dead)
        {
            SetDirectionOnPlayer();
            shield.SetActive(true);
            TryAttacking();
        }
    }

    public void TryAttacking()
    {
        if ((seedAttack.canUse || spikeAttack.canUse) && Vector3.Distance(playerPosition.position, transform.position) < seedAttack.range)
        {
            if (seedAttack.canUse)
            {
                StartCoroutine(StartSeedAttack());
            }
            else if (spikeAttack.canUse)
            {
                StartCoroutine(StartSpikeAttack());
                shield.SetActive(false);
            }
        }
        else if (basicAttack.canUse)
        {
            Collider2D temp = GetPlayerColliderInCollider(basicAttackCollider);
            if (temp != null)
            {
                StartCoroutine(StartBasicAttack(basicAttack));
                entController.AddBasicAttackPriority();
                shield.SetActive(false);
            }
        }
    }

    private IEnumerator StartSpikeAttack()
    {
        StartCoroutine(AttackCooldown(2.3f));
        StartCoroutine(spikeAttack.StartCooldown());
        entController.AddSpikeAttackPriority();
        Debug.Log("spike");
        yield return new WaitForSeconds(spikeAttack.delay);

        SpawnSpike();
    }

    private IEnumerator StartSeedAttack()
    {
        StartCoroutine(AttackCooldown(2.5f));
        StartCoroutine(seedAttack.StartCooldown());
        entController.AddSeedAttackPriority();
        yield return new WaitForSeconds(seedAttack.delay);

        StartCoroutine(SpawnSeed());
    }

    /**************************************************************************************************/
    //spawns a spike on ground and the number that will spawn in a direction
    public void SpawnSpike()
    {
        Vector3 rayOrigin = transform.position + direction * -5;                             //origin for ray
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 10, collisionMask);   //raycast
        //Debug.DrawRay(rayOrigin, Vector2.down * 10, Color.yellow, 5);

        if (hit)
        {
            Vector2 spikeOrigin = hit.point + Vector2.up * 1.25f;                                               //origin for spike
            EntSpike temp = Instantiate(spike, spikeOrigin, transform.rotation).GetComponent<EntSpike>();       //instantiate spike
            StartCoroutine(temp.SpawnNextSpike(4, direction * -3));                                              //start coroutine to spawn 4 spike 3 apart
        }
    }

    /**************************************************************************************************/
    //shoot 5 seeds stright up
    public IEnumerator SpawnSeed()
    {
        for (int i = 0; i < 5; i++)
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

    private void SetAttacks()
    {
        basicAttack = new BossAttack(basicAttackCollider, 0, 1.3f, 3);
        seedAttack = new BossAttack(searchDistance, 0, 1.5f, 30);
        spikeAttack = new BossAttack(searchDistance, 0, 1.5f, 20);
    }
    public void die()
    {
        //Ent Death
        dead = true;
        Debug.Log("Ent dead");
        this.transform.GetChild(3).gameObject.SetActive(false);
        entController.AddDeathPrioity();
    }
}
