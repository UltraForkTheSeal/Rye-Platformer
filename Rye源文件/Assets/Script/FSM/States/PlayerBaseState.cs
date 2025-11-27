using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : StateMachineBehaviour
{
    protected static CharacterControl characterControl;
    protected static InputReader inputReader;
    protected static PlayerLife playerLife;
    protected static PlayerVisual playerVisual;
    //protected static Vector3 PlayerPos => characterControl.transform.position;
    
    //animator stringToHash在各个状态类保存为静态字段
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Enter Base State");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Update Base State");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Exit Base State");
    }
}
