using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HugRumble : MonoBehaviour
{
    [SerializeField] private RumbleParams _hugRumbleParams;
    [SerializeField] private InputReader _input;

    private void OnEnable()
    {
        _input.TogglePlayerActions(true);
    }

    private void OnDisable()
    {
        _input.TogglePlayerActions(false);
    }

    public void Trigger()
    {
        RumbleManager.Instance.RumbleByParams(_hugRumbleParams);
    }
}
