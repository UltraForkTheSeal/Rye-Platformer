using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEnterTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent _onPlayerEnter;
    [SerializeField] private bool _triggerOnce = true;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _onPlayerEnter?.Invoke();
        }

        if (_triggerOnce)
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
