using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityBase : MonoBehaviour,IAbility
{
    public string AbilityName { get => abilityName; }
    public bool HasUnlocked { get; set; }

    [SerializeField] protected InputReader inputReader;
    [SerializeField] protected string abilityName;

    protected CharacterControl characterControl;
    protected Rigidbody2D playerRigidBody2D;
    
    private void Awake()
    {
        characterControl = GetComponentInParent<CharacterControl>();
        playerRigidBody2D = characterControl.GetComponent<Rigidbody2D>();
    }

    public virtual void InputCallback()
    {
        if (!HasUnlocked)
        {
            return;
        }
        
        UseAbility();
    }

    public virtual void UseAbility()
    {
        if (!HasUnlocked)
        {
            return;
        }
    }
    
}
