using UnityEngine;

public class DropInteractable : PlayerBaseState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterControl.DropInteractable();
        characterControl.CanUpdateVelocity = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterControl.CanUpdateVelocity = true;
    }
}