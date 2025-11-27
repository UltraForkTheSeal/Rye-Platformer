using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingDoor : MonoBehaviour
{
    [SerializeField] private Transform _upperPos;
    [SerializeField] private Transform _lowerPos;
    [SerializeField] private Transform _doorTransform;
    [SerializeField] private float _upSpeed;
    [SerializeField] private float _downSpeed;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private Transform _rayOrigin;
    [SerializeField] private float _rayLength = 0.2f;
    public bool isBeingPulled;
    
    private void Update()
    {
        if (!isBeingPulled)
        {
            MoveDown();
        }
    }

    private void MoveDoor(Vector3 targetPos, float speed)
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(_rayOrigin.position + Vector3.left * 0.5f, Vector2.down, _rayLength, _interactableLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(_rayOrigin.position + Vector3.right * 0.5f, Vector2.down, _rayLength, _interactableLayer);
        if (hitLeft.collider != null || hitRight.collider != null)
        {
            if (hitLeft.transform.TryGetComponent(out BoxBehaviour boxLeft))
            {
                boxLeft.Stop();
            }
            else if (hitRight.transform.TryGetComponent(out BoxBehaviour boxRight))
            {
                boxRight.Stop();
            }
            
            return;
        }
        _doorTransform.position = Vector3.MoveTowards(_doorTransform.position,targetPos,speed * Time.deltaTime);
    }

    public void MoveUp()
    {
        MoveDoor(_upperPos.position, _upSpeed);
    }

    public void MoveDown()
    {
        MoveDoor(_lowerPos.position, _downSpeed);
    }
    
}
