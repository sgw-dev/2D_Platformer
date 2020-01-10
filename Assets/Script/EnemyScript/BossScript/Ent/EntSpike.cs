using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntSpike : MonoBehaviour
{
    /*
     * By: Parker Allen
     * Version: 1.0
     * 
     * Setup:
     *  Has an animator
     *  on the SpikeOut block in animator attach AnimationAutoDestroy
     */

    private Animator animator;  //Animator of object
    private Collider2D hitBox;

    /**************************************************************************************************/

    public void Start()
    {
        animator = GetComponentInChildren<Animator>();  //set the animator
        hitBox = GetComponent<BoxCollider2D>();
        StartCoroutine(Animate());      //start coroutine for Animate
    }

    /**************************************************************************************************/
    //spawn next spike recursivly
    public IEnumerator SpawnNextSpike(int num, Vector3 dir)
    {
        yield return new WaitForSeconds(.2f);       //a little delay
        if(num != 0)
        {
            EntSpike temp = Instantiate(this.gameObject, transform.position + dir, transform.rotation).GetComponent<EntSpike>();    //spawn spike
            StartCoroutine(temp.SpawnNextSpike(num - 1, dir));      //start coroutine of next spike to spawn another in direction
        }
        StopCoroutine(SpawnNextSpike(num, dir));    //stop this coroutine
    }

    /**************************************************************************************************/
    //when to to do out animation
    IEnumerator Animate()
    {
        yield return new WaitForSeconds(1);     //wait
        hitBox.enabled = false;
        animator.SetBool("Out", true);          //do out animation
        StopCoroutine(Animate());               //stop coroutine
    }
}
