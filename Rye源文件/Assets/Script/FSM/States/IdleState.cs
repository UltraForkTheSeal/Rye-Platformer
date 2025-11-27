using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerBaseState
{
    private static int _idleToJumpTrigger = Animator.StringToHash("t_idleToJump");
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterControl.ApplyDownwardsForce = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Idle - Move
        //Idle - Jump
        if (inputReader.isJumpPressed)
        {
            animator.SetTrigger(_idleToJumpTrigger);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterControl.ApplyDownwardsForce = false;
        animator.ResetTrigger(_idleToJumpTrigger);
    }

}
