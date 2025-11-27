using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SliderJoint2D))]
public class SliderJointEditor : MonoBehaviour
{
    [SerializeField] private SliderJoint2D _slider;
    [SerializeField] private float _angle;
    private Vector3 _anchorWorldPos;
    
    private void Awake()
    {
        _slider = GetComponent<SliderJoint2D>();
    }

    private void Start()
    {
        _slider.autoConfigureAngle = false;
        _slider.angle = _angle;
        _slider.connectedAnchor = transform.position;

        _anchorWorldPos = transform.position;
    }

    private void OnValidate()
    {
        _slider.connectedAnchor = transform.position;
    }
    
    private void OnDrawGizmos()
    {
        Vector3 startPos;
        Vector3 dir = Quaternion.Euler(0, 0, _angle) * Vector3.right;
        
        if (Application.isPlaying)
        {
            startPos = _anchorWorldPos;
        }
        else
        {
            startPos = transform.position;
        }
        
        Vector3 cubeSize = Vector3.one * 0.5f;
        
        //EditorMode
        JointTranslationLimits2D limit = _slider.limits;
        Gizmos.color = Color.red;      //下限
        Gizmos.DrawCube(startPos + (-dir) * limit.max,cubeSize);
        Gizmos.color = Color.blue;      //上限
        Gizmos.DrawCube(startPos + (-dir) * limit.min,cubeSize);
    }
    
}
