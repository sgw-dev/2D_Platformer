using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject target; //The thing the camera is following
    private Transform targetTrans;
    private float x;
    private float y;
    private float targetX;
    private float targetY;

	// Use this for initialization
	void Start () {
        targetTrans = target.transform;
	}
	
	// Update is called once per frame
	void Update () {
        x = this.transform.position.x;
        y = this.transform.position.y;
        targetX = targetTrans.position.x;
        targetY = targetTrans.position.y;
        if (x < targetX)
        {
            this.transform.Translate((targetX - x), y, 0);
        }
        else if (x > targetX) {
            this.transform.Translate((targetX - x), y, 0);
        }
	}
}
