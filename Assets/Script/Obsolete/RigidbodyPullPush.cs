using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyPullPush : MonoBehaviour, IInteractable
{
    private CharacterControl _player;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _player = FindObjectOfType<CharacterControl>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void PlayAnimation()
    {
        //
    }

    public void InteractEnter()
    {
        //
    }

    public void InteractExit()
    {
        //
    }

    public void InteractUpdate()
    {
        _rigidbody.velocity = _player.Velocity;
    }
}
