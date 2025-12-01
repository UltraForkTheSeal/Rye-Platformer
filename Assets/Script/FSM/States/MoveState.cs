using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : PlayerBaseState
{
    private static int _moveToJumpTrigger = Animator.StringToHash("t_moveToJump");
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //HandleMove
        characterControl.TurnCheck();
        
        //Transition
        //Move - Idle
        //Move - Jump
        if (inputReader.isJumpPressed)
        {
            animator.SetTrigger(_moveToJumpTrigger);
            //Debug.Break();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(_moveToJumpTrigger);
    }
}
