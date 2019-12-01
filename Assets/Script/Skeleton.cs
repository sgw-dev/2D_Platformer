using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    /*
     * By: Parker Allen
     * Version: 1.0
     * 
     * Setup:
     *  pivotPoint is the transform of a game object
     *  childSkeleton is a list of skeletons that are connected to it
     */

    public Transform pivotPoint;            //point the object rotates around
    public List<Skeleton> childSkeleton;    //skeletons that children of this
    private float angle;                    //angle that object is rotated
    private int flip;                       //-1 or 1 flip angle to negative

    /**************************************************************************************************/
    //if the object is fliped the angle needs to be negative
    public void isFliped(bool f)
    {
        flip = (f) ? -1 : 1;    //sets flip to 1 or -1 if object is fliped
    }

    /**************************************************************************************************/
    //rotates object around a point
    public void RotateSkeleton(bool rc)
    {
        if (angle != 0)
        {
            transform.RotateAround(pivotPoint.position, Vector3.forward, angle);    //rotates object around a point by a angle
            if (rc)
            {
                foreach (var child in childSkeleton)
                {
                    child.CounterRotation(-angle);          //only rotate object not the children
                }
            }
        }
    }

    /**************************************************************************************************/
    //counters the parents rotation
    public void CounterRotation(float a)
    {
        transform.RotateAround(pivotPoint.position, Vector3.forward, a);    //rotates object around a point by a angle
    }

    /**************************************************************************************************/
    //sets the angle to rotate
    public void setAngle(float t, float end)
    {
        float currAngle = transform.eulerAngles.z;                      //gets the current rotation between 0 and 360
        currAngle = (currAngle > 180) ? currAngle - 360 : currAngle;    //this lets the rotation be between -180 and 180
        angle = (end - currAngle) / t * Time.deltaTime * flip;          //the angle to move based on diference in angle over time and flip
    }
}
