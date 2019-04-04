using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrolling : MonoBehaviour
{
    
    int speed = 2;
  
    
   
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //moving back and forth
        if(transform.position.x<= -2.9f)
        {
            speed = 5;
        }

        if (transform.position.x >= 2.9f)
        {
            speed = -5;
        }
        transform.Translate(speed * Time.deltaTime, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        //kills enemy
        if (col.gameObject.tag == "bullet")
        {
            Destroy(gameObject);
            Debug.Log("Enemy destroyed!!");
        }

        //kills player
        if (col.gameObject.tag == "player")
        {
            Destroy(col.gameObject);
            Debug.Log("Player died");
        }

        
    }
}
