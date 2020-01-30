using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarAnimator : MonoBehaviour
{
    public Animator slimeAnim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            slimeAnim.SetBool("Attack", true);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            slimeAnim.SetBool("Attack", false);
        }
    }
}
