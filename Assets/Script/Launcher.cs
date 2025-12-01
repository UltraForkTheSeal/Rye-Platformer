using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Launcher : MonoBehaviour
{
    [SerializeField] private float _launchWaitTime;
    [SerializeField] private float _addForceTime;
    private WaitForSeconds _launchWait;
    private WaitForSeconds _addForceDuration;
    private Coroutine _launchCoroutine;
    private bool _shouldAddForce; 
    

    [SerializeField] private float _targetHeight = 2f;
    [SerializeField] private bool _ignoreGravity;
    private Rigidbody2D _rigidbody2D;
    
    [SerializeField] private RumbleParams _launchRumble;

    private void Awake()
    {
        _launchWait = new WaitForSeconds(_launchWaitTime);
        _addForceDuration = new WaitForSeconds(_addForceTime);
        
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_shouldAddForce)
        { 
            // acc = (2 * height) / time ^ 2
            float acceleration = (2 * _targetHeight) / (_addForceTime * _addForceTime);
            acceleration += _ignoreGravity ? Physics2D.gravity.magnitude * _rigidbody2D.gravityScale : 0f;
            
            // acc 与物体质量无关
            Vector2 force = acceleration * _rigidbody2D.mass * Vector2.up;
            
            _rigidbody2D.AddForce(force);
        }
    }

    public void Trigger()
    {
        if (_launchCoroutine == null)
        {
            _launchCoroutine = StartCoroutine(LaunchCoroutine());
        }
    }

    private IEnumerator LaunchCoroutine()
    {
        yield return _launchWait;
        _shouldAddForce = true;
        RumbleManager.Instance.RumbleByParams(_launchRumble);
        SoundFXManager.Instance.PlayRandomSFXInArray("LauncherStart",transform.position,0.3f);
        yield return _addForceDuration;
        _shouldAddForce = false;
        
        
        _launchCoroutine = null;
    }
}
