using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafMovement : MonoBehaviour
{
    /*
     * By: Parker Allen
     * Version: 1.0
     */

    public float speed;         //speed of projectile
    public float duration;      //how long it lasts
    private bool fullSize;

    /**************************************************************************************************/

    public void Start()
    {
        StartCoroutine(GrowSize());
    }

    /**************************************************************************************************/

    void Update()
    {
        if(fullSize)
            transform.position += transform.right * speed * Time.deltaTime;        //move projectile forward
    }

    IEnumerator GrowSize()
    {
        for(int i = 0; i <= 100; i++)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, .01f);
            yield return null;
        }
        fullSize = true;
        StartCoroutine(DestroyAfterTime());     //start count down until self destruct
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
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Stage"))
        {
            Destroy(this.gameObject);       //detroy this
        }
    }
}
