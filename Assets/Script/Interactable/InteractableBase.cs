using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableBase : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Collider2D _interactableCollider;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Transform _followTransform;
    [SerializeField] private float _moveDuration = 0.5f;
    public bool IsBeingPicked {get; private set;}
    public UnityEvent _onPicked;
    public UnityEvent _onDropped;
    
    
    // 切换sorting order
    // 举起interactable 位置
    // 显示交互提示按钮

    public void SetUpInteractable(Transform followTarget)
    {
        _followTransform = followTarget;
        IsBeingPicked = true;
        _onPicked?.Invoke();
        
        // 防止碰撞
        _interactableCollider.enabled = false;
        
        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        // 举起时，物体遮挡玩家
        _spriteRenderer.sortingOrder = 1;
        
        StartCoroutine(SmoothMove());
    }

    public void UpdateInteractable()
    {
        transform.position = _followTransform.position;
    }

    public void DropInteractable()
    {
        _followTransform = null;
        IsBeingPicked = false;
        _onDropped?.Invoke();
        
        _interactableCollider.enabled = true;
        // 通过重力 自然下落
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        _spriteRenderer.sortingOrder = -1;
    }

    private IEnumerator SmoothMove()
    {
        float t = 0f;
        Vector3 startPos = transform.position;
        while (t < _moveDuration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, _followTransform.position, t / _moveDuration);
            
            yield return null;
        }
        
        transform.position = _followTransform.position;
    }
}
