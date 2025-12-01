using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerBaseState
{
    private static int doubleJumpTrigger = Animator.StringToHash("t_doubleJump");

    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Enter Jump State");
        //Handle Jump
        //characterControl.HandleJump();
        characterControl.JumpTrigger = true;
        characterControl.SetNormalGravity();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Update Jump State");
        //characterControl.HandleMove();
        characterControl.TurnCheck();

        if (inputReader.isJumpPressed && characterControl.CanDoubleJump)
        {
            animator.SetTrigger(doubleJumpTrigger);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Exit Jump State");
        animator.ResetTrigger(doubleJumpTrigger);
    }
}
