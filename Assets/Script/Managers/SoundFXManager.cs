using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundFXManager : Singleton<SoundFXManager>
{
    [SerializeField] private SoundLibrary _soundLibrary;
    [SerializeField] private AudioSource _SFXSourcePrefab;
    private CharacterControl _character;
    
    public override void Awake()
    {
        base.Awake();
        _soundLibrary.Initialize();
        _character = FindObjectOfType<CharacterControl>();
    }
    
    public void PlaySFX(string clipName, Vector3 spawnPosition, float volume)
    {
        //生成 audioSource,并根据参数调整音量等属性
        AudioSource source = Instantiate(_SFXSourcePrefab, spawnPosition, Quaternion.identity);
        //通过SoundLibrary 获取AudioClip
        source.clip = _soundLibrary.GetClipFromName(clipName);
        source.volume = volume;
        
        //播放，并在音效结束后销毁
        source.Play();
        float clipLength = source.clip.length;
        Destroy(source.gameObject, clipLength);
    }
    
    public void PlayRandomSFXInArray(string arrayName, Vector3 spawnPosition, float volume)
    {
        //通过SoundLibrary 获取AudioClipArray
        AudioClip[] clips = _soundLibrary.GetClipArrayFromName(arrayName); 
        int index = Random.Range(0, clips.Length);
        
        //生成 audioSource,并根据参数调整音量等属性
        AudioSource source = Instantiate(_SFXSourcePrefab, spawnPosition, Quaternion.identity);
        source.clip = clips[index];
        source.volume = volume;
        
        //播放，并在音效结束后销毁
        source.Play();
        float clipLength = clips[index].length;
        Destroy(source.gameObject, clipLength);
    }

}
