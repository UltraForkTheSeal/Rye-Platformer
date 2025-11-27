using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SceneChangeFade : MonoBehaviour
{
    [SerializeField] private GameObject _fadeImage;
    private Material _fadeMaterial;

    [SerializeField] private float _fadeInDuration = 2f;
    [SerializeField] private float _fadeOutDuration = 2f;

    [SerializeField] private UnityEvent _onFadeInStarted;
    [SerializeField] private UnityEvent _onFadeInEnded;
    
    private void Awake()
    {
        _fadeMaterial = _fadeImage.GetComponent<Image>().material;    
    }

    private IEnumerator Start()
    {
        _onFadeInStarted?.Invoke();
        // 等待该协程结束
        yield return StartCoroutine(FadeCoroutine(1,_fadeInDuration));
        _onFadeInEnded?.Invoke();
    }

    private IEnumerator FadeCoroutine(int shouldReverse, float duration)
    {
        _fadeMaterial.SetInt("_IsReverse",shouldReverse);
        
        float t = 0f;
        while (t < _fadeInDuration)
        {
            t += Time.deltaTime;
            _fadeMaterial.SetFloat("_Progress",Mathf.Lerp(0f,1f,t / duration));
            yield return null;
        }
        
        _fadeMaterial.SetFloat("_Progress",1f);
    }
    
    public void TriggerFadeOut()
    {
        StartCoroutine(FadeCoroutine(0,_fadeOutDuration));
    }
}
