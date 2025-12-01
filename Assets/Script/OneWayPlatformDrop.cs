using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlatformEffector2D))]
public class OneWayPlatformDrop : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    private Collider2D _playerCollider;
    private Collider2D _platformCollider;
    [SerializeField] private float _disableCollisionTime = 0.5f;
    private bool _shouldDrop;
    
    private void Awake()
    {
        _platformCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (_playerCollider != null && _input.IsInteractPerformed)
        {
            _shouldDrop = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerCollider = other.collider;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (_playerCollider != null && _shouldDrop)
        {
            _shouldDrop = false;
            StartCoroutine(DisableCollision());
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerCollider = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        //Physics2D.IgnoreCollision(_playerCollider, _platformCollider, true);
        _platformCollider.enabled = false;
        yield return new WaitForSeconds(_disableCollisionTime);
        //Physics2D.IgnoreCollision(_playerCollider, _platformCollider, false);
        _platformCollider.enabled = true;

    }
}
