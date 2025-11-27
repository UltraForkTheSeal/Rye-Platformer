using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem _doubleJumpEffect;
    [SerializeField] private GameObject[] _lights;
    
    public void TriggerDoubleJumpEffect()
    {
        _doubleJumpEffect.Play();
    }

    public void SetPlayerLight(bool on)
    {
        foreach (var playerLight in _lights)
        {
            playerLight.SetActive(on);
        }
    }
    
}
