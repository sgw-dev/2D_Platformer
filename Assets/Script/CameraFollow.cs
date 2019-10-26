using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Author: Spencer Burke

    public GameObject stageObject; //The stage the player is on    must be assigned

    public Transform targetTrans; //that thing's transform
    private float targetX; //the target's x position
    private float targetY; //the target's y position
    private Camera cam; //The camera component of the camera object
    private Sprite stage; //the sprite component of the stage
    private float camVertExtent; //the camera's view port height
    private float camHorzExtent; //the camera's view port width
    //the world cordinates of the stage sides
    private float leftBound;
    private float rightBound;
    private float bottomBound;
    private float topBound;

    public Transform bottomLeft;
    public Transform topRight;

    // Use this for initialization
    void Start()
    {
        //get the target's transform
        //targetTrans = target.transform;
        //get the camera component on the camera gameobject
        cam = this.GetComponent<Camera>();
        //get the sprite component of the stage
        stage = stageObject.GetComponent<SpriteRenderer>().sprite;
        //gets the vertical and horizontal size of the camera
        camVertExtent = cam.orthographicSize;
        camHorzExtent = cam.aspect * camVertExtent;
        //adjust where the camera can travel by modifying the bounds of the stage accounting for the
        //    width and height of the camera
        leftBound = bottomLeft.position.x + camHorzExtent;
        rightBound = topRight.position.x - camHorzExtent;
        bottomBound = bottomLeft.position.y + camVertExtent;
        topBound = topRight.position.y - camVertExtent;

        
    }

    // Update is called once per frame
    void Update()
    {
        //rechecks the target's position
        targetX = targetTrans.position.x;
        targetY = targetTrans.position.y;
        //sets the camera's position to be on the target, but still 
        //    between the bounds
        float camX = Mathf.Clamp(targetX, leftBound, rightBound);
        float camY = Mathf.Clamp(targetY, bottomBound, topBound);
        //physically sets the camera's position
        cam.transform.position = new Vector3(camX, camY, -12.0f);
    }
}
