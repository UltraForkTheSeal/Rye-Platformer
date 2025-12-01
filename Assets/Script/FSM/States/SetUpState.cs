using UnityEngine;

public class SetUpState : PlayerBaseState
{
    //用于获取CharacterController和InputReader引用
    //不执行其他操作，直接过度到Idle

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Enter SetUp State");
        characterControl = animator.GetComponentInParent<CharacterControl>();
        playerLife = animator.GetComponentInParent<PlayerLife>();
        playerVisual = animator.GetComponentInParent<PlayerVisual>();
        inputReader = characterControl.InputReader;
        
        characterControl.SetNormalGravity();
        characterControl.CanUpdateVelocity = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }


}
