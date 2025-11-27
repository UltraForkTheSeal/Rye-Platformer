using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastInput : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    private void OnEnable()
    {
        _inputReader.TogglePlayerActions(true);
    }

    private void OnDisable()
    {
        _inputReader.TogglePlayerActions(false);
    }
}
