using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntFlower : MonoBehaviour
{
    /*
     * By: Parker Allen
     * Version: 1.0
     * 
     * Setup:
     *  attach this script to the child gameobject of EntStem
     *  eS is the EntStem script
     *  pivot is a chilc gameobject of this
     *  laser is a prefab
     */

    public EntStem eS;          //EntStem script
    public Transform pivot;     //Transform to rotate around
    public GameObject laser;    //prefab to shoot

    /**************************************************************************************************/

    public void Start()
    {
        StartCoroutine(ShootLaser());       //start coroutine
    }

    /**************************************************************************************************/

    void Update()
    {
        RotateToPlayer();       //rotate to face player
    }

    /**************************************************************************************************/
    //changes the rotation to face the player
    public void RotateToPlayer()
    {
        float angle = Vector3.SignedAngle(transform.position - eS.getPPos().position, transform.up, Vector3.forward);       //calulates the angle
        transform.RotateAround(pivot.position, Vector3.forward, angle * Time.deltaTime * .8f);                                    //rotates to angle around a point
    }

    /**************************************************************************************************/
    //fires a laser
    IEnumerator ShootLaser()
    {
        yield return new WaitForSeconds(2);                                              //wait to charge
        Instantiate(laser, transform.position + transform.up * .1f, transform.rotation);    //fire the laser
        yield return new WaitForSeconds(.2f);                                               //wait to wilt
        gameObject.SetActive(false);                                                        //deactivate this
        eS.StartWilt();                                                                     //EntStem starts to wilt
        StopCoroutine(ShootLaser());                                                        //stop soroutine
    }
}
