using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    private Dictionary<string, PlayerAbilityBase> _abilityDict;
    
    private void Awake()
    {
        PlayerAbilityBase[] abilityArray = GetComponents<PlayerAbilityBase>();
        
        _abilityDict = new Dictionary<string, PlayerAbilityBase>();
        foreach (var ability in abilityArray)
        {
            // if (!_abilityDict.ContainsKey(ability.AbilityName))
            // {
            //     _abilityDict.Add(ability.AbilityName, ability);
            // }
            _abilityDict.TryAdd(ability.AbilityName, ability);
        }
    }
    

    public bool UnlockAbilityOfName(string abilityName)
    {
        if (_abilityDict.TryGetValue(abilityName,out PlayerAbilityBase ability))
        {
            ability.HasUnlocked = true;
            return true;
        }

        return false;
    }
    
}
