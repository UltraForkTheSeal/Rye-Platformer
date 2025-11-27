using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientArea : MonoBehaviour
{
    private CharacterControl _player;
    private AudioSource _audioSource;
    private Collider2D _collider;
    
    [SerializeField] private float _maxDistance = 15f;
    
    private void Awake()
    {
        _player = FindObjectOfType<CharacterControl>();
        _audioSource = GetComponentInChildren<AudioSource>();
        _collider = GetComponent<Collider2D>();
        
        _audioSource.enabled = false;
        _audioSource.loop = true;
        _audioSource.spatialBlend = 1;

        if (_maxDistance >= _audioSource.minDistance)
        {
            _audioSource.maxDistance = _maxDistance;
        }
    }

    private void OnValidate()
    {
        if (_audioSource)
        {
            if (_maxDistance >= _audioSource.minDistance)
            {
                _audioSource.maxDistance = _maxDistance;
            }
        }
    }

    private void Update()
    {
        // // 大于maxDistance 禁用，否则启用
        // 忽略z轴
        _audioSource.enabled = Vector2.Distance(_player.transform.position, _audioSource.transform.position) <= _maxDistance;
        
        _audioSource.transform.position = _collider.ClosestPoint(_player.transform.position);
    }
}
