using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class TeleportFlipAudio : MonoBehaviour
{
    [SerializeField] private Teleport _teleport;
    [SerializeField] private float _transitionDuration = 1f;
    
    [SerializeField] private AudioMixerSnapshot _aboveSnapshot;
    [SerializeField] private AudioMixerSnapshot _belowSnapshot;

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += SceneManager_OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= SceneManager_OnSceneUnloaded;

    }

    private void SceneManager_OnSceneUnloaded(Scene unloadedScene)
    {
        if (unloadedScene == SceneManager.GetActiveScene())
        {
            _aboveSnapshot.TransitionTo(0.1f);
        }
    }


    public void FlipAudio()
    {
        if (_teleport.AboveTeleportPlaneFlag == 1)
        {
            _aboveSnapshot.TransitionTo(_transitionDuration);
        }
        else
        {
            _belowSnapshot.TransitionTo(_transitionDuration);
        }
    }

}
