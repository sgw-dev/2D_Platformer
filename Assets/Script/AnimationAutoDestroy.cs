using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAutoDestroy : StateMachineBehaviour
{
    /*
     * By: Parker Allen
     * Version: 1.0
     * 
     * Setup:
     *  attach to any animator block that you want to detroy at the end of its animation
     */

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(animator.gameObject, stateInfo.length);     //destroys the game object
    }
}
