using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleShowHide : MonoBehaviour
{
    [SerializeField] private TitleMaterial _title;
    private bool _hasShown;
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_hasShown)
            {
                _hasShown = true;
                _title.StartProgressIn();
            }
        }
    }
}
