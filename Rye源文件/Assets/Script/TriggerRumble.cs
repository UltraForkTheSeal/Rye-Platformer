using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRumble : MonoBehaviour
{
    public void Rumble(RumbleParams rumbleParams)
    {
        RumbleManager.Instance.RumbleByParams(rumbleParams);
    }
}
