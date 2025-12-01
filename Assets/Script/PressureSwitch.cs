using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressureSwitch : MonoBehaviour
{
    [SerializeField] private float _triggerVelocityYThreshold = -0.1f;
    [SerializeField] private float _enterYOffset = 0.1f;
    [SerializeField] private float _triggerYOffset = 0.3f;
    private Transform _buttonTransform;
    private Vector3 _startLocalPos;
    
    [SerializeField] private UnityEvent onSwitchTriggered;
    
    private void Awake()
    {
        _buttonTransform = transform.GetChild(0);
    }

    private void Start()
    {
        _startLocalPos = _buttonTransform.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out CharacterControl player))
        {
            //速度到达threshold 触发开关
            if (player.Velocity.y <= _triggerVelocityYThreshold)
            {
                onSwitchTriggered?.Invoke();
                SoundFXManager.Instance.PlaySFX("SwitchOn",transform.position,0.2f);
               _buttonTransform.Translate(Vector3.down * _triggerYOffset);
            }
            //未到达触发阈值，只播放动画
            else
            {
                //SoundFXManager.Instance.PlaySFX("SwitchOff",transform.position,0.2f);
               _buttonTransform.Translate(Vector3.down * _enterYOffset);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out CharacterControl player))
        {
            SoundFXManager.Instance.PlaySFX("SwitchOff",transform.position,0.2f);
            _buttonTransform.localPosition = _startLocalPos;
        }
    }
}
