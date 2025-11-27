using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceType : MonoBehaviour
{
    public enum Surface
    {
        Default,
        Concrete,
        Glass,
        Wood,
        Grass,
        Metal,
        Sand,
        Cloth,
        Water
    }

    [SerializeField] private Surface _currentSurface;
    public Surface CurrentSurface => _currentSurface;
}
