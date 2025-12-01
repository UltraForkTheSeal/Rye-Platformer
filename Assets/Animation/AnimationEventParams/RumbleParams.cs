using UnityEngine;

[CreateAssetMenu(fileName = "RumbleParams", menuName = "Animation Event/Rumble Params")] 
public class RumbleParams : AnimationEventParams
{
    [Range(0f,1f)] public float lowFrequency;
    [Range(0f,1f)] public float highFrequency;
    public float duration;
}
