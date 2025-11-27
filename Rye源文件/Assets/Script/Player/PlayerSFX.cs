using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    // private CharacterControl _character;
    //
    // private void Awake()
    // {
    //     _character = GetComponentInParent<CharacterControl>();
    // }

    public void PlaySound(SoundParams soundParams)
    {
        SoundFXManager.Instance.PlaySFX(soundParams.clipName,transform.position,soundParams.volume);
    }

    public void PlaySoundInArray(MultipleSoundParams multipleSoundParams)
    {
        SoundFXManager.Instance.PlayRandomSFXInArray(multipleSoundParams.clipsArrayName,transform.position,multipleSoundParams.volume);
    }

    // public void PlayWalkSFXBySurface(AnimationEvent soundData)
    // {
    //     string surfaceName = GetArrayNameBySurface(_character.CurrentSurfaceType);
    //     SoundFXManager.Instance.PlayRandomSFXInArray(surfaceName, transform.position, soundData.floatParameter);
    // }
    //
    // private string GetArrayNameBySurface(SurfaceType.Surface surface)
    // {
    //     string result = "Walk";
    //     switch (surface)
    //     {
    //         case SurfaceType.Surface.Concrete:
    //             result = "WalkConcrete";
    //             break;
    //         case SurfaceType.Surface.Glass:
    //             result = "WalkGlass";
    //             break;
    //         case SurfaceType.Surface.Wood:
    //             result = "WalkWood";
    //             break;
    //         case SurfaceType.Surface.Grass:
    //             result = "WalkGrass";
    //             break;
    //         case SurfaceType.Surface.Metal:
    //             result = "WalkMetal";
    //             break;
    //         case SurfaceType.Surface.Sand:
    //             result = "WalkSand";
    //             break;
    //         case SurfaceType.Surface.Cloth:
    //             result = "WalkCloth";
    //             break;
    //         case SurfaceType.Surface.Water:
    //             result = "WalkWater";
    //             break;
    //     }
    //     
    //     return result;
    // }
}
