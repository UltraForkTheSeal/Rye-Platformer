using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehaviour : MonoBehaviour,IInteractable
{
    private Rigidbody2D _rigidbody2D;
    private CharacterControl _characterControl;
    [SerializeField] private Collider2D edgeCollider;
    private bool _canMove = true;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _characterControl = FindObjectOfType<CharacterControl>();
    }
    
    private void Update()
    {

    }

    public void PlayAnimation()
    {
        
    }

    public void InteractEnter()
    {
    }

    public void InteractExit()
    {
        if (!_canMove) return;
        
        _rigidbody2D.velocity = Vector2.zero;
    }

    public void InteractUpdate()
    {
        if(!_canMove) return;
        _rigidbody2D.velocity = _characterControl.Velocity;
    }

    public void Stop()
    {
        _canMove = false;
    }

}
