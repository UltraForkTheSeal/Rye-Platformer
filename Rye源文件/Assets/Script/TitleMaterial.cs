using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMaterial : MonoBehaviour
{
    // public bool isProgressIn;
    // bool pre;
    Material mat;
    float progress = 0f;
    public float durationFadeIn = 0.5f;
    public float durationFadeOut = 0.2f;
    private Coroutine currentCoroutine;
    
    [SerializeField] private float _showDuration = 5f;
    private WaitForSeconds _showWait;

    private void Awake()
    {
        _showWait = new WaitForSeconds(_showDuration);
    }

    private void OnValidate()
    {
        _showWait = new WaitForSeconds(_showDuration);
    }

    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
        if (mat == null)
        {
            Debug.LogError("No material found on the GameObject.");
            return;
        }

        // Initialize the shader property
        if (mat.HasProperty("_Progress"))
        {
            mat.SetFloat("_Progress", 0f);
        }
        else
        {
            Debug.LogError("Shader does not have a _Progress property.");
        }
    }


    public void StartProgressIn()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        if (isActiveAndEnabled)
        {
            currentCoroutine = StartCoroutine(ProgressInCoroutine());
        }
    }

    public void StartProgressOut()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        if (isActiveAndEnabled)
        {
            currentCoroutine = StartCoroutine(ProgressOutCoroutine());
        }
    }
    private IEnumerator ProgressInCoroutine()
    {
        while (progress < 1f)
        {
            progress += Time.deltaTime / durationFadeIn; // Adjust speed as needed
            mat.SetFloat("_Progress", progress);
            yield return null;
        }
        // Ensure progress is set to 1 after completion
        mat.SetFloat("_Progress", 1f);

        StartCoroutine(ShowForDuration());
    }

    private IEnumerator ProgressOutCoroutine()
    {
        while (progress > 0f)
        {
            progress -= Time.deltaTime / durationFadeOut; // Adjust speed as needed
            mat.SetFloat("_Progress", progress);
            yield return null;
        }
        // Ensure progress is set to 0 after completion
        mat.SetFloat("_Progress", 0f);
    }

    private IEnumerator ShowForDuration()
    {
        yield return _showWait;
        StartCoroutine(ProgressOutCoroutine());
    }
    
    void OnDestroy()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
    }
}