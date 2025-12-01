using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachineBrain : MonoBehaviour
{
    [SerializeField] 
    private InputReader _inputReader;
    private CharacterControl _characterControl;
    private PlayerLife _playerLife;
    private Teleport _teleport;
    
    private Animator _animator;
   // private DashAbility _dashAbility;

    //全局更新Animator Parameter
    private int inputXFloat = Animator.StringToHash("f_inputX");
    private int velocityYFloat = Animator.StringToHash("f_velocityY");
    private int isGroundedBool = Animator.StringToHash("b_isGrounded");
    //private int hasHitWallBool = Animator.StringToHash("b_hasHitWall");
    private int interactTrigger = Animator.StringToHash("t_interact");
    private int hasInteractableBool = Animator.StringToHash("b_hasInteractable");

    
    
    //AnyState transition
    private int diedTrigger = Animator.StringToHash("t_died");
    private int respawnTrigger = Animator.StringToHash("t_respawn");
    private int dashPerformedTrigger = Animator.StringToHash("t_dashPerformed");
    private int dashEndedTrigger = Animator.StringToHash("t_dashEnded");
    private int cutSceneStartedTrigger = Animator.StringToHash("t_cutsceneStarted");
    private int cutSceneEndedTrigger = Animator.StringToHash("t_cutsceneEnded");
    
    private void Awake()
    {
        _characterControl = GetComponent<CharacterControl>();
        _playerLife = GetComponent<PlayerLife>();
        _teleport = _characterControl.GetComponent<Teleport>();
        
        _animator = GetComponentInChildren<Animator>();
        
        
        //_dashAbility = FindObjectOfType<DashAbility>();
    }

    private void OnEnable()
    {
        _playerLife.Died += PlayerLife_OnDied;
        _playerLife.Respawned += PlayerLife_OnRespawned;
        // _dashAbility.DashPerformed += OnDashed;
        // _dashAbility.DashEnded += DashEnded;
    }
    
    private void OnDisable()
    {
        _playerLife.Died -= PlayerLife_OnDied;
        _playerLife.Respawned -= PlayerLife_OnRespawned;
        // _dashAbility.DashPerformed -= OnDashed;
        // _dashAbility.DashEnded -= DashEnded;
    }

    
    private void DashEnded()
    {
        _animator.SetTrigger(dashEndedTrigger);
    }


    private void OnDashed()
    {
        _animator.SetTrigger(dashPerformedTrigger);
    }

    private void PlayerLife_OnDied()
    {
        _animator.SetTrigger(diedTrigger);
    }

    private void PlayerLife_OnRespawned()
    {
        _animator.SetTrigger(respawnTrigger);
    }

    
    private void Update()
    {
        
        _animator.SetBool(isGroundedBool,_characterControl.OnGround);
        //这里可以修改为直接从inputReader中获取
        _animator.SetFloat(inputXFloat,Mathf.Abs(_characterControl.inputVector.x));
        
        // 检测到有interact物体，进入interactState
        // 已有interact物体，从interactState退出

        if (_inputReader.isInteractPressed)
        {
            _animator.SetTrigger(interactTrigger);
        }
        _animator.SetBool(hasInteractableBool,_characterControl.HasInteractable);
    }

    private void FixedUpdate()
    {
        _animator.SetFloat(velocityYFloat,_characterControl.Velocity.y * _teleport.AboveTeleportPlaneFlag);
    }


    public void EnterCutscene()
    {
        _animator.SetTrigger(cutSceneStartedTrigger);
    }

    public void ExitCutscene()
    {
        _animator.SetTrigger(cutSceneEndedTrigger);
    }

    public void EnterSleep()
    {
        _animator.SetTrigger("t_sleep");
    }
}
