using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandState : PlayerBaseState
{
    private static int _landToJumpTrigger = Animator.StringToHash("t_landToJump");
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterControl.SetNormalGravity();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (inputReader.isJumpPressed)
        {
            animator.SetTrigger(_landToJumpTrigger);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(_landToJumpTrigger);
    }
}
