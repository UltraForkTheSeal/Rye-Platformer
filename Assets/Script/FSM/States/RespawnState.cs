using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnState : PlayerBaseState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Enter Respawn State");
        characterControl.CanUpdateVelocity = false;
        playerLife.Respawn();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterControl.CanUpdateVelocity = false;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterControl.CanUpdateVelocity = true;
    }
}
