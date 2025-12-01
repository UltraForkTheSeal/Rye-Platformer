using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnUI : MonoBehaviour
{
    private PlayerLife _playerLife;
    [SerializeField] private Grad _fadeOut;
    [SerializeField] private Grad _fadeIn;

    private void Awake()
    {
        _playerLife = FindObjectOfType<PlayerLife>();
    }

    private void OnEnable()
    {
        _playerLife.Died += PlayerLife_OnDied;
        _playerLife.Respawned += Player_OnRespawned;
    }

    private void OnDisable()
    {
        _playerLife.Died -= PlayerLife_OnDied;
        _playerLife.Respawned -= Player_OnRespawned;
    }

    private void PlayerLife_OnDied()
    {
        _fadeOut.enabled = true;
    }
    
    private void Player_OnRespawned()
    {
        _fadeIn.enabled = true;
    }
    

    private void Start()
    {
        _fadeOut.enabled = false;
        _fadeIn.enabled = false;
    }
    
    
}
