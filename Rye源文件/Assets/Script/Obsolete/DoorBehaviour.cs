using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DoorBehaviour : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private SliderJoint2D _sliderJoint2D;
    private JointMotor2D _currentMotor;
    [SerializeField] private float _upSpeed = 1f;
    [SerializeField] private float _downSpeed = 0.5f;
    
    private void Awake()
    {
        _sliderJoint2D = GetComponent<SliderJoint2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        _sliderJoint2D.useMotor = true;

        _currentMotor.motorSpeed = _downSpeed;
        _currentMotor.maxMotorTorque = 10000f;      //为达到motorSpeed,所能应用的最大力，与组件中数值相同（10000）
        _sliderJoint2D.motor = _currentMotor;
    }

    public void MotorMoveUp()
    {
        Debug.Log("Motor Move Up");
        //
        // // 拉长绳子 门上升
        // if (pullSpeed < 0f)
        // {
        //     pullSpeed *= _upSpeed;
        // }
        // // 绳子自然收回 门下落
        // else if (pullSpeed > 0f)
        // {
        //     pullSpeed *= _downSpeed;
        // }
        
        _currentMotor.motorSpeed = -_upSpeed;
        _sliderJoint2D.motor = _currentMotor;
    }

    public void ClearVelocity()
    {
        _rigidbody2D.velocity = Vector2.zero;
    }

    public void MoveDown()
    {
        _currentMotor.motorSpeed = _downSpeed;
        _sliderJoint2D.motor = _currentMotor;
    }
    
    public void StopMotor()
    {
        _currentMotor.motorSpeed = 0f;
        _sliderJoint2D.motor = _currentMotor;
        _rigidbody2D.velocity = Vector2.zero;
    }
    
}
