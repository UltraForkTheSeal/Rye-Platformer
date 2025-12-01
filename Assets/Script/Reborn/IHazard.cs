using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHazard
{
    public Action PlayerKilled { get; set; }
}
