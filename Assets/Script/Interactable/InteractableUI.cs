using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableUI : MonoBehaviour
{
    [SerializeField] private ButtonAffect _visuals;
    private bool _playerInRange;

    public void InteractablePicked()
    {
        _visuals.StartProgressOut();
    }

    public void InteractableDropped()
    {
        if (_playerInRange)
        {
            _visuals.StartProgressIn();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;
            _visuals.StartProgressIn();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        _playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
            _visuals.StartProgressOut();
        }
    }
}
