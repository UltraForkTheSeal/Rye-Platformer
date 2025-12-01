using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Sound Effect Library", fileName = "Sound Library")]
public class SoundLibrary : ScriptableObject
{
    [SerializeField]
    private List<SoundData> _soundDataList;
    private Dictionary<string, AudioClip> _soundDict;
    [SerializeField] private List<MultipleSoundData> _multipleSoundDataList;
    private Dictionary<string, AudioClip[]> _multipleSoundDict;
    
    public void Initialize()
    {
        _soundDict = new Dictionary<string, AudioClip>();
        foreach (var data in _soundDataList)
        {
            if(!_soundDict.ContainsKey(data.soundName))
            {
                _soundDict.Add(data.soundName,data.clip);
            }
        }

        _multipleSoundDict = new Dictionary<string, AudioClip[]>();
        foreach (var data in _multipleSoundDataList)
        {
            if(!_multipleSoundDict.ContainsKey(data.soundsName))
            {
                _multipleSoundDict.Add(data.soundsName,data.clips);
            }
        }
    }
    
    public AudioClip GetClipFromName(string clipName)
    {
        return _soundDict.GetValueOrDefault(clipName);
    }
    
    public AudioClip[] GetClipArrayFromName(string clipName)
    {
        return _multipleSoundDict.GetValueOrDefault(clipName);
    }
    
}


[Serializable]
public struct SoundData
{
    public string soundName;
    public AudioClip clip;
}

[Serializable]
public struct MultipleSoundData
{
    public string soundsName;
    public AudioClip[] clips;
}