using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnusedInteractState : PlayerBaseState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterControl.EnterInteractable();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterControl.MoveInteractable();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("should exit");
        characterControl.DropInteractable();
    }
}
