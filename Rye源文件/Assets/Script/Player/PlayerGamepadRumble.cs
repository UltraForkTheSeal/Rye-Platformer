using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGamepadRumble : MonoBehaviour
{
    public void Rumble(RumbleParams rumbleParams)
    {
        RumbleManager.Instance.Rumble(rumbleParams.lowFrequency,rumbleParams.highFrequency,rumbleParams.duration);
    }
}
