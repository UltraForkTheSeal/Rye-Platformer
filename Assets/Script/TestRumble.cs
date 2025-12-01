using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRumble : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField,Range(0f,1f)] private float _lowFreq = 1f;
    [SerializeField,Range(0f,1f)] private float _highFreq = 1f;
    [SerializeField] private float _duration = 1f;

    private void Update()
    {
        if (_inputReader.rumblePressed)
        {
            RumbleManager.Instance.Rumble(_lowFreq,_highFreq,_duration);
        }
    }
}
