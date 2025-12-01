using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal; // 需要引用URP的Light2D
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Light2D))]
public class BreathingLight : MonoBehaviour
{
    public float speed = 1f;            // 呼吸频率
    public float intensityMultiplier = 0.5f; // 强度变化幅度
    //public Color startColor = Color.white; // 起始颜色
    //public Color endColor = Color.blue;    // 结束颜色

    private Light2D targetLight;
    private float baseIntensity;
    private float timer;
    public AnimationCurve breathingCurve;

    public bool fadeIn;
    public float fadeInDuration = 1f;
    IEnumerator fadeInCoroutine;
    private float multiplier = 1f;

    void Start()
    {
        targetLight = GetComponent<Light2D>();
        baseIntensity = targetLight.intensity;
        timer = Random.Range(0f, 1f);
    }
    void OnEnable()
    {
        fadeInCoroutine = FadeInLight();
        if (fadeIn)
        {
            StartCoroutine(fadeInCoroutine);
        }

        else
        {
            multiplier = 1f;
        }
    }
    void Update()
    {
        // 在Update中替换为：
        float curveValue = breathingCurve.Evaluate(timer * speed % 1) * intensityMultiplier;
        // 使用正弦波实现平滑呼吸效果
        //float intensityVariation = Mathf.Sin(timer * speed) * intensityMultiplier;
        targetLight.intensity = (baseIntensity + curveValue) * multiplier;
        timer += Time.deltaTime;
    }
    void OnDisable()
    {
        // 在禁用时停止协程
        StopCoroutine(fadeInCoroutine);
        // 在禁用时重置光源强度
        targetLight.intensity = baseIntensity;
    }
    IEnumerator FadeInLight()
    {
        float timer = 0f;
        while (timer < fadeInDuration)
        {
            multiplier = Mathf.Lerp(0f, 1.0f, timer / fadeInDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        multiplier = 1f; // 确保在淡入完成后将乘数设置为1
    }
}