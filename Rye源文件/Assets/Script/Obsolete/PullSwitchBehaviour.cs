using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullSwitchBehaviour : MonoBehaviour,IInteractable
{
    private Rigidbody2D _rigidbody2D;
    private CharacterControl _characterControl;
    private SliderJoint2D _sliderJoint2D;
    //public float VelocityX => _rigidbody2D.velocity.x;
    
    public Action SwitchPulled;
    public Action SwitchReleased;
    
    
    private void Awake()
    {
        _sliderJoint2D = GetComponent<SliderJoint2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _characterControl = FindObjectOfType<CharacterControl>();
    }

    // private void Update()
    // {
    //     if (Mathf.Abs(VelocityX) > 0.001f)
    //     {
    //         SwitchPulled?.Invoke(VelocityX);
    //     }
    // }

    public void PlayAnimation()
    {
        
    }

    public void InteractEnter()
    {
        _sliderJoint2D.useMotor = false;
    }

    public void InteractExit()
    {
        _sliderJoint2D.useMotor = true;
        SwitchReleased?.Invoke();
    }

    public void InteractUpdate()
    {
        _rigidbody2D.velocity = _characterControl.Velocity;
        SwitchPulled?.Invoke();
    }
}
