using UnityEngine;

[CreateAssetMenu(fileName = "Multi Sound Params", menuName = "Animation Event/Multi Sound Params")] 
public class MultipleSoundParams : AnimationEventParams
{
    public float volume;
    public string clipsArrayName;
}