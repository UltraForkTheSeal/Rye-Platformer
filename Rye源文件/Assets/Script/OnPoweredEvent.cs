using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnPoweredEvent : MonoBehaviour
{
    [SerializeField] private ContactPoint _wire;
    [SerializeField] private UnityEvent _onWirePowered;
    [SerializeField] private UnityEvent _onPowerCut;
    
    private void OnEnable()
    {
        _wire.OnPoweredChanged += Wire_OnPoweredChanged;
    }

    private void OnDisable()
    {
        _wire.OnPoweredChanged -= Wire_OnPoweredChanged;
    }

    private void Wire_OnPoweredChanged(bool isPowered)
    {
        if (isPowered)
        {
            _onWirePowered?.Invoke();
        }
        else
        {
            _onPowerCut?.Invoke();
        }
    }
}
