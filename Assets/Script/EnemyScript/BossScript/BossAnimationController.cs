using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    /*
     * By: Parker Allen
     * Version: 1.0
     * 
     */

    private List<BossAnimation> priorityActions;    //a queue for actions that take priority

    private BossAnimation[] interruptableActions;   //an array of actions that can be changed or inturrupted at any time
    private int IAArrayCounter;                     //the current action its on

    private int counter;        //counter for time

    /**************************************************************************************************/
    //when object is first loaded
    private void Awake()
    {
        priorityActions = new List<BossAnimation>();    //initialize list
    }

    /**************************************************************************************************/
    //flip body, and tells which action should be done
    public void DoAction(Skeleton[] skele, bool isFlipped)
    {
        FlipBoss(isFlipped, skele);         //flips all the skeletons
        if(priorityActions.Count > 0)
        {
            AnimateAction(skele, priorityActions[0], true);     //animate action as a priority action
        }
        else if(interruptableActions != null)
        {
            AnimateAction(skele, interruptableActions[IAArrayCounter], false);      //animate action as an iterruptable action
        }
    }

    /**************************************************************************************************/
    //rotateskeleton and ends an action
    public void AnimateAction(Skeleton[] skele, BossAnimation ba, bool isPriority)
    {
        if (counter == 0)
        {
            SetSkeletonRotationAngle(skele, ba);        //set skeleton rotation
        }

        if (counter < ba.time * 60)
        {
            RotateAllSkeletons(skele);                  //rotates skeletons
        }

        counter++;

        if (counter >= ba.time * 60)
        {
            EndAction(isPriority);               //ends the action
        }
    }

    /**************************************************************************************************/
    //sets the rotation rate for all the skeletons
    private void SetSkeletonRotationAngle(Skeleton[] skele, BossAnimation ba)
    {
        for (int i = 0; i < skele.Length; i++)
            skele[i].setAngle(ba.time, ba.angles[i]);
    }

    /**************************************************************************************************/
    //tells all the skeletons to rotate
    private void RotateAllSkeletons(Skeleton[] skele)
    {
        for (int i = 0; i < skele.Length; i++)
            skele[i].RotateSkeleton();
    }

    /**************************************************************************************************/
    //end an action
    private void EndAction(bool p)
    {
        counter = 0;        //reset counter
        if (p)
        {
            priorityActions.RemoveAt(0);        //if it was a priority action remove it from list
            IAArrayCounter = 0;                 //reset interruptable counter
        }
        else
        {
            IncrementAICounter();               //increment interruptable counter
        }
    }

    /**************************************************************************************************/
    //flips rotation of the skeletons and rotates the main body
    public void FlipBoss(bool t, Skeleton[] skele)
    {
        transform.rotation = (t) ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);     //rotate body on the y axis

        for (int i = 0; i < skele.Length; i++)
        {
            skele[i].isFliped(t);       //flip all the skeletons
        }
    }

    /**************************************************************************************************/
    //increments the interruptable action counter
    private void IncrementAICounter()
    {
        IAArrayCounter = (IAArrayCounter + 1) % interruptableActions.Length;
    }

    /**************************************************************************************************/
    //add action to priority queue
    public void AddPriorityAction(BossAnimation ba)
    {
        priorityActions.Add(ba);    //add action to queue
        counter = 0;                //reset counter
    }

    /**************************************************************************************************/
    //set the interruptable action
    public void SetInterruptableActions(BossAnimation[] ba)
    {
        if(!ba.Equals(interruptableActions))        //not the same action
        {
            interruptableActions = ba;              //set interruptable action
            counter = 0;                            //reset action
            IAArrayCounter = 0;                     //reset interruptable action counter
        }
    }

    /**************************************************************************************************/
    //empty priority queue
    public bool EmptyPriority()
    {
        return priorityActions.Count == 0;
    }
}
