using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    SpriteRenderer[] _spriteRenderer;
    public Color startColor;
    public Color endColor;
    public float duration;
    public float delay;
    public AnimationCurve curve;
    float _timer;

    // Start is called before the first frame update
    void Awake()
    {
        _spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        GetComponent<Collider2D>().enabled = true;

    }

    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(Fading());
            GetComponent<Collider2D>().enabled = false;
        }
    }



    IEnumerator Fading()
    {
        _timer = 0;
        while (_timer < delay + duration)
        {
            float rate = Mathf.Min(Mathf.Max((curve.Evaluate((_timer - delay) / duration)), 0), 1);
            for(int i = 0; i<_spriteRenderer.Length; i++)
                _spriteRenderer[i].color = Color.Lerp(startColor, endColor, rate);
            _timer += Time.deltaTime;
            yield return null; 
        }
        for (int i = 0; i < _spriteRenderer.Length; i++)
            _spriteRenderer[i].color = endColor;
        this.enabled = false;
        yield return null;   
    }
}
