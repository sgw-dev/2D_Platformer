using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerLaserMovement : MonoBehaviour
{
    /*
     * By: Parker Allen
     * Version: 1.0
     * 
     * Setup:
     *  speed = 14
     *  duration = 1
     */

    public float speed;         //speed of projectile
    public float duration;      //how long it lasts

    /**************************************************************************************************/

    public void Start()
    {
        StartCoroutine(DestroyAfterTime());     //start count down until self destruct
    }

    /**************************************************************************************************/

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;        //move projectile forward
    }

    /**************************************************************************************************/
    //destroy after time
    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(duration);      //wait
        Destroy(this.gameObject);                       //destroy this
        StopCoroutine(DestroyAfterTime());              //stop coroutine
    }

    /**************************************************************************************************/
    //destroy on contact with wall or player;
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.gameObject.SendMessage("applyDamage", 1.0f);
            Destroy(this.gameObject);       //detroy this
        }else if (collision.transform.CompareTag("Stage"))
        {
            Destroy(this.gameObject);
        }
    }
    
}
