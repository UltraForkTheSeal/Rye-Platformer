using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConductorOnRail : MonoBehaviour
{
    [SerializeField] private Transform _leftBound;
    [SerializeField] private Transform _rightBound;
    [SerializeField] private float _moveSpeed = 1f;
    
    private Vector3 _currentTarget;
    private Coroutine _moveCoroutine;

    public void Move()
    {
        if (_moveCoroutine == null)
        {
            _moveCoroutine = StartCoroutine(MoveCoroutine());
        }
    }

    private void SetTargetByDistance()
    {
        float leftDistance = Vector3.Distance(_leftBound.position, transform.position);
        float rightDistance = Vector3.Distance(_rightBound.position, transform.position);
        //目标选择距离更大的一边
        _currentTarget = leftDistance < rightDistance ? _rightBound.position : _leftBound.position;     
    }
    
    private IEnumerator MoveCoroutine()
    {
        SetTargetByDistance();
        
        while (Vector3.Distance(transform.position, _currentTarget) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position,_currentTarget,_moveSpeed * Time.deltaTime);
            yield return null;
        }        
        transform.position = _currentTarget;
        _moveCoroutine = null;
    }
}
