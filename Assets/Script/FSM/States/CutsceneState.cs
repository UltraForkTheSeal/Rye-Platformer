using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneState : PlayerBaseState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Enter Cutscene");
        characterControl.CanUpdateVelocity = false;
        characterControl.FreezePosition();
        playerVisual.SetPlayerLight(false);
        
        animator.ResetTrigger("t_sleep");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Exit Cutscene");
        characterControl.CanUpdateVelocity = true;
        characterControl.UnfreezePosition();
        playerVisual.SetPlayerLight(true);
    }
}

