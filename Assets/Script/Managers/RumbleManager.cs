using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleManager : Singleton<RumbleManager>
{
    [SerializeField] private InputReader _inputReader;
    private Coroutine _stopRumbleCoroutine;
    
    public void Rumble(float lowFreq, float highFreq, float duration)
    {
        if (_inputReader.CurrentInputDevice is Gamepad)
        {
            Gamepad gamepad = Gamepad.current;

            if (gamepad != null)
            {
                gamepad.SetMotorSpeeds(lowFreq, highFreq);
            
                _stopRumbleCoroutine = StartCoroutine(StopAfterDuration(duration,gamepad));
            }
        }
    }

    public void RumbleByParams(RumbleParams rumbleParams)
    {
        Rumble(rumbleParams.lowFrequency, rumbleParams.highFrequency, rumbleParams.duration);
    }
    
    private IEnumerator StopAfterDuration(float duration, Gamepad pad)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            yield return null;
        }

        _stopRumbleCoroutine = null;
        pad.SetMotorSpeeds(0f,0f);
    }


}
