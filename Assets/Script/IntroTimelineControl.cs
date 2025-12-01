using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class IntroTimelineControl : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    private bool _isPlaying;
    [SerializeField] private UnityEvent _onWakeUp;

    private void OnEnable()
    {
        _input.AnyButtonPressed += InputOn_AnyButtonPressed;
    }

    private void OnDisable()
    {
        _input.AnyButtonPressed -= InputOn_AnyButtonPressed;
    }

    private void InputOn_AnyButtonPressed()
    {
        if (!_isPlaying)
        {
            _isPlaying = true;
            _onWakeUp?.Invoke();
        }
    }
}