using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility 
{
    public string AbilityName { get; }
    public bool HasUnlocked { get; set; }
    //触发该能力对应的输入
    public void InputCallback();
    public void UseAbility();
}
