using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepState : PlayerBaseState
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterControl.CanUpdateVelocity = false;
        playerVisual.SetPlayerLight(false);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {


    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterControl.CanUpdateVelocity = true;
        animator.ResetTrigger("t_sleep");
    }
}