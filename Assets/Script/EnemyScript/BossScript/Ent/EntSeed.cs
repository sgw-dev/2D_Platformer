using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntSeed : MonoBehaviour
{
    /*
     * By: Parker Allen
     * Version: 1.0
     * 
     * Setup:
     *  speed = 8
     */

    public GameObject stem;     //gameobject with EntStem Script

    public float speed;                 //speed of projectile
    private float duration;             //how long it lasts
    private bool spawnAnother;          //if it spawns another seed
    private Vector3 playerPosition;     //positon above player
    private Transform pPos;             //transform of player

    public LayerMask collisionMask;     //collision mask for ground

    /**************************************************************************************************/

    public void Start()
    {
        StartCoroutine(DestroyAfterTime());     //start count down until self destruct
    }

    /**************************************************************************************************/

    void FixedUpdate()
    {   
        HitGround();                                                        //check if hit ground
        transform.position += transform.up * speed * Time.deltaTime;        //move projectile forward
    }

    /**************************************************************************************************/
    //sets duration and players transform
    public void SetDuration(float d, Transform tp)
    {
        duration = d;
        pPos = tp;
    }

    /**************************************************************************************************/
    //sets duration, position above player, transform nad wheater to spawn another seed
    public void SetDown(float d, Vector3 p, Transform tp, bool s)
    {
        duration = d;
        playerPosition = p;
        pPos = tp;
        spawnAnother = s;
    }

    /**************************************************************************************************/
    //spawn another seed above player
    private void SpawnSeed()
    {
        Quaternion rot = Quaternion.Euler(0, 0, transform.eulerAngles.z - 180);                     //rotate it down
        EntSeed temp = Instantiate(this.gameObject, playerPosition, rot).GetComponent<EntSeed>();   //spawn seed
        temp.SetDuration(5, pPos);                                                                  //set the duration and transform
    }

    /**************************************************************************************************/
    //check if it hits ground
    public void HitGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, speed * Time.deltaTime, collisionMask);  //raycast
        if (hit)
        {
            Vector3 stemOrigin = new Vector3(transform.position.x, hit.collider.bounds.max.y + .9f, 0);     //place to spawn stem
            EntStem temp = Instantiate(stem, stemOrigin, Quaternion.identity).GetComponent<EntStem>();      //spawn stem
            temp.SetPlayerPosition(pPos);                                                                   //set player transform
            Destroy(this.gameObject);                                                                       //destroy seed
        }
    }

    /**************************************************************************************************/
    //destroy after time
    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(duration);      //wait

        if (spawnAnother)
        {
            SpawnSeed();        //spawn another seed
        }

        Destroy(this.gameObject);                       //destroy this
        StopCoroutine(DestroyAfterTime());              //stop coroutine
    }

    /**************************************************************************************************/
    //destroy on contact with wall or player;
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Destroy(this.gameObject);       //detroy this
        }
    }
}
