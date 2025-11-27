using UnityEngine;

[CreateAssetMenu(fileName = "Sound Params", menuName = "Animation Event/Sound Params")] 
public class SoundParams : AnimationEventParams
{
    public float volume;
    public string clipName;
}