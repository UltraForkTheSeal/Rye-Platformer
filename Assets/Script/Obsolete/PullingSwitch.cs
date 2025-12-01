using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullingSwitch : MonoBehaviour,IInteractable
{
    [SerializeField] private Transform _leftBound;
    [SerializeField] private Transform _rightBound;
    [SerializeField] private Transform _switchTransform;
    [SerializeField] private float _retractingSpeed = 3f;
    private CharacterControl _characterControl;
    private Rigidbody2D _rigidbody;
    private bool _isPulling;
    [SerializeField] private FallingDoor _fallingDoor;
    
    private void Awake()
    {
        _characterControl = FindObjectOfType<CharacterControl>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!_isPulling)
        {
            MoveTowards(_rightBound.position, _retractingSpeed);
        }

        if (_rigidbody.velocity.x < 0f)
        {
            _fallingDoor.MoveUp();
        }
        else if (_rigidbody.velocity.x > 0f)
        {
            _fallingDoor.MoveDown();
        }
        
        float xPos = Mathf.Clamp(_switchTransform.position.x, _leftBound.position.x, _rightBound.position.x);
        _switchTransform.position = new Vector3(xPos, _switchTransform.position.y, _switchTransform.position.z);
    }
    
    public void PlayAnimation()
    {
        //
    }

    public void InteractEnter()
    {
        _isPulling = true;
        _fallingDoor.isBeingPulled = true;
    }

    public void InteractExit()
    { 
        _isPulling = false;
        _fallingDoor.isBeingPulled = false;
        _rigidbody.velocity = Vector2.zero;
    }

    public void InteractUpdate()
    {
        _rigidbody.velocity = _characterControl.Velocity;
    }
    
    private void MoveTowards(Vector3 targetPos, float speed)
    {
        _switchTransform.position = Vector3.MoveTowards(_switchTransform.position,targetPos,speed * Time.deltaTime);
    }
}
