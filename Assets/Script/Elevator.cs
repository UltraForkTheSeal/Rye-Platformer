using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Elevator : MonoBehaviour
{
    [SerializeField] private float _targetHeight;
    [SerializeField] private float _moveTime;
    private Vector3 _bottomPos;
    private Vector3 _upperPos;
    private Vector3 _smoothSpeed;

    [SerializeField] private UnityEvent _onMoveDownStarted;
    private bool _moveDownTriggered;
    
    
    private void Start()
    {
        _bottomPos = transform.position;
        _upperPos = _bottomPos + Vector3.up * _targetHeight;
    }

    private void OnValidate()
    {
        _upperPos = _bottomPos + Vector3.up * _targetHeight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_upperPos, 0.5f);
    }

    public void TriggerMoveUp()
    {
        StopAllCoroutines();
        StartCoroutine(MoveCoroutine(_upperPos));
    }

    public void TriggerMoveDown()
    {
        if (!_moveDownTriggered)
        {
            _moveDownTriggered = true;
            StopAllCoroutines();
            _onMoveDownStarted?.Invoke();   
            StartCoroutine(MoveCoroutine(_bottomPos));
        }
    }
    
    private IEnumerator MoveCoroutine(Vector3 targetPos)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _smoothSpeed, _moveTime);
            yield return null;
        }
        
        transform.position = targetPos;
    }
    
}
