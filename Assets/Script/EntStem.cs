using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntStem : MonoBehaviour
{
    /*
     * By: Parker Allen
     * Version: 1.0
     * 
     * Setup:
     *  Has an animator
     *  on the wilted block in animator attach AnimationAutoDestroy
     *  flower is a child with EntFlower script
     */

    private Animator anim;              //the animator of object
    public GameObject flower;           //EntFlower gameobject
    private Transform playerPosition;   //player position

    /**************************************************************************************************/

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();    //sets the animator
        StartCoroutine(GrowFlower());       //starts coroutine for grow flower
    }

    /**************************************************************************************************/
    //setter
    public void SetPlayerPosition(Transform p)
    {
        playerPosition = p;
    }

    /**************************************************************************************************/
    //getter
    public Transform getPPos()
    {
        return playerPosition;
    }

    /**************************************************************************************************/
    //tells animator to start wilt animation
    public void StartWilt()
    {
        anim.SetBool("Wilt", true);     //set wilt bool to true
    }

    /**************************************************************************************************/
    //wait to set flower as active
    IEnumerator GrowFlower()
    {
        yield return new WaitForSeconds(1.5f);      //wait 1.5 seconds
        flower.SetActive(true);                     //set flower active
        StopCoroutine(GrowFlower());                //stop coroutine
    }
}
