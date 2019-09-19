using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Author: Spencer Burke
    
    private Rigidbody2D rb;//the rigid body attached to the gameobject
    public float speed; //how much force is added each time
    public float maxSpeed; //how fast you are alowed to go
    public float verticalSpeed;//how strong you jumps are

    // Start is called before the first frame update
    void Start()
    {
        //gets the rigid body on this object
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if D pressed
        if (Input.GetKey(KeyCode.D)) {
            //if the current x velocity is less that the max alowed
            if(rb.velocity.x < maxSpeed)
            {
                //add force to the object to the right
                rb.AddForce(new Vector2(speed, 0.0f));
                //flip the sprite to face right
                this.GetComponent<SpriteRenderer>().flipX = false;
                
            }
            
        }
        //if A pressed
        if (Input.GetKey(KeyCode.A))
        {
            //is the current x velocity is greater than negative max alowed
            //this is becuase you can be going fast backwards too
            if (rb.velocity.x > -maxSpeed)
            {
                //add force on the object to the left
                rb.AddForce(new Vector2(-speed, 0.0f));
                //flip the sprite to face left
                this.GetComponent<SpriteRenderer>().flipX = true;
                
            }

        }
        //if W pressed
        if (Input.GetKeyDown(KeyCode.W))
        {
            //add force up
            /*Needs more work to limit the jumps*/
            rb.AddForce(new Vector2(0.0f, verticalSpeed*10));
            

        }

    }
}
