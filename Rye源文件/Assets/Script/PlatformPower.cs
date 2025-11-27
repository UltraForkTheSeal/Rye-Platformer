using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPower : MonoBehaviour
{
    public ContactPoint contactPoint;
    public MovingPlatform movingPlatform;
    public GameObject ddlForPowerUse;
    
    
    private void Update()
    {
        if (contactPoint.isPowered)
        {
            movingPlatform.Trigger();
            ddlForPowerUse.gameObject.SetActive(false);
        }
    }
}
