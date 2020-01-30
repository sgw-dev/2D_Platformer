using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlAnimator : MonoBehaviour
{
    public Animator slimeAnim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            slimeAnim.SetBool("See", true);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            slimeAnim.SetBool("See", false);
        }else if (Input.GetKeyDown(KeyCode.T))
        {
            slimeAnim.SetBool("Attack", true);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            slimeAnim.SetBool("Attack", false);
        }
    }
}
