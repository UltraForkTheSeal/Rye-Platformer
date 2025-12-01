using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DashAbility : PlayerAbilityBase
{
    public Action DashPerformed;
    public Action DashEnded;
    
    [SerializeField] private float _dashSpeed = 10f;
    [SerializeField] private float _abilityTimeDuration = 1.5f; 
    [SerializeField] private float _coolDownTime = 1f;
    private Coroutine _coolDownCoroutine;

    private void OnEnable()
    {
        inputReader.DashPerformed += InputCallback;
    }

    private void OnDisable()
    {
        inputReader.DashPerformed -= InputCallback;
    }

    public override void InputCallback()
    {
        //Debug.Log("Dash Input");
        // if (!HasUnlocked)
        // {
        //     return;
        // }
        
        //CD协程没有在运行，可以使用能力
        if (_coolDownCoroutine == null)
        {
            UseAbility();
            DashPerformed?.Invoke();
            _coolDownCoroutine = StartCoroutine(AbilityWithCoolDown());
        }
    }

    public override void UseAbility()
    {
        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        float t = 0f;
        float gScale = playerRigidBody2D.gravityScale;
        
        playerRigidBody2D.gravityScale = 0f;
        playerRigidBody2D.velocity = Vector2.right * (characterControl.FacingX * _dashSpeed);
        //playerRigidBody2D.AddForce(transform.right * _dashSpeed,ForceMode2D.Impulse);
        
        while (t < _abilityTimeDuration)
        {
            t += Time.deltaTime;
            yield return null;
        }

        playerRigidBody2D.gravityScale = gScale;
        playerRigidBody2D.velocity = new Vector2(0f, playerRigidBody2D.velocity.y);
        DashEnded?.Invoke();
    }
    
    private IEnumerator AbilityWithCoolDown()
    {
        //Debug.Log("Start CD");
        float t = 0f;
        while (t < _coolDownTime)
        {
            //Debug.Log(t);
            //只有在落地时 才开始计算CD
            if (characterControl.OnGround)
            {
                t += Time.deltaTime;
            }
            yield return null;
        }
        
        _coolDownCoroutine = null;
        //Debug.Log("CD End");
    }
}
