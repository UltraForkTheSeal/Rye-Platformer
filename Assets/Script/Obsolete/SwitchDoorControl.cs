using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDoorControl : MonoBehaviour
{
    [SerializeField] private DoorBehaviour _door;
    [SerializeField] private PullSwitchBehaviour _switch;

    private void OnEnable()
    {
        _switch.SwitchPulled += _door.MotorMoveUp;
        _switch.SwitchReleased += _door.MoveDown;
    }

    private void OnDisable()
    {
        _switch.SwitchPulled -= _door.MotorMoveUp;
        _switch.SwitchReleased -= _door.MoveDown;
    }
}
