using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitSwitch : MonoBehaviour, IInteractable
{
    private CharacterControl _player;
    private Rigidbody2D _rigidbody;
    [SerializeField] private Transform _xLeftBound;
    [SerializeField] private Transform _xRightBound;
    
    private void Awake()
    {
        _player = FindObjectOfType<CharacterControl>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float xPos = Mathf.Clamp(transform.position.x, _xLeftBound.position.x, _xRightBound.position.x);
        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }

    public void PlayAnimation()
    {
        //
    }

    public void InteractEnter()
    {

    }

    public void InteractExit()
    {

    }

    public void InteractUpdate()
    {
        _rigidbody.velocity = _player.Velocity;
    }
}
