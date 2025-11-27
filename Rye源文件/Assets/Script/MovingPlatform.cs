using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform _leftBound;
    [SerializeField] private Transform _rightBound;
    [SerializeField] private Transform _platformTransform;
    [SerializeField] private float _speed;
    
    private bool _isFirstMove = true;
    [SerializeField] private bool _autoReverse = false;
    [SerializeField] private float _waitBeforeReverse = 2f;
    [SerializeField] private float _waitBeforeFirstMove = 2f;
    private WaitForSeconds _reverseWait;
    private WaitForSeconds _firstMoveWait;
    private Vector3 _currentTarget;

    private Vector3 _startPos;
    
    private Coroutine _moveCoroutine;
    
    [SerializeField] private UnityEvent _onMoveStarted;

    private void Awake()
    {
        _reverseWait = new WaitForSeconds(_waitBeforeReverse);
        _firstMoveWait = new WaitForSeconds(_waitBeforeFirstMove);
    }

    private void Start()
    {
        _startPos = _platformTransform.position;
    }

    private void SetTargetByDistance()
    {
        float leftDistance = Vector3.Distance(_leftBound.position, _platformTransform.position);
        float rightDistance = Vector3.Distance(_rightBound.position, _platformTransform.position);
        //目标选择距离更大的一边
        _currentTarget = leftDistance < rightDistance ? _rightBound.position : _leftBound.position;     
    }

    private IEnumerator MoveCoroutine()
    {
        SetTargetByDistance();
        if (_isFirstMove)
        {
            _isFirstMove = false;
            yield return _firstMoveWait;
        }
        
        _onMoveStarted?.Invoke();
        
        while (Vector3.Distance(_platformTransform.position, _currentTarget) > 0.01f)
        {
            _platformTransform.position = Vector3.MoveTowards(_platformTransform.position,_currentTarget,_speed * Time.deltaTime);
            yield return null;
        }
        _platformTransform.position = _currentTarget;
        _moveCoroutine = null;

        if (_autoReverse)
        {
            yield return _reverseWait;
            _moveCoroutine = StartCoroutine(MoveCoroutine());
        }
    }
    
    public void Trigger()
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
            
        }
        _moveCoroutine = StartCoroutine(MoveCoroutine());
    }

    public void ReturnToStartPos()
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
        }

        _moveCoroutine = StartCoroutine(ReturnToStartCoroutine());
    }

    private IEnumerator ReturnToStartCoroutine()
    {
        while (Vector3.Distance(_platformTransform.position, _startPos) > 0.01f)
        {
            _platformTransform.position = Vector3.MoveTowards(_platformTransform.position,_startPos,_speed * Time.deltaTime);
            yield return null;
        }
        _platformTransform.position = _startPos;
        _moveCoroutine = null;
    }
}
