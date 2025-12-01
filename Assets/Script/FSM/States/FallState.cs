using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : PlayerBaseState
{
    private int doubleJumpTrigger = Animator.StringToHash("t_doubleJump");

    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Enter Fall State");
        characterControl.SetFallGravity();
        characterControl.PreventTripleJumpWhenFall();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Update Fall State");
        //characterControl.HandleMove();
        characterControl.TurnCheck();

        if (inputReader.isJumpPressed && characterControl.CanDoubleJump)
        {
            animator.SetTrigger(doubleJumpTrigger);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Exit Fall State");
        animator.ResetTrigger(doubleJumpTrigger);
    }
}
