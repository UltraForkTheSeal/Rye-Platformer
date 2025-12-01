using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Collider2D))]
public class DeathLight : MonoBehaviour
{
    [Header("Light Settings")]
    public Light2D[] lights;
    public Volume volume;
    public float duration = 1f;
    public float maxIntensity = 10f;
    public AnimationCurve lightCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve volumeCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private float[] defaultIntensities;
    private Coroutine lightCoroutine;

    void Awake()
    {
        if (lights == null || lights.Length == 0)
        {
            Debug.LogWarning("No lights assigned to DeathLight.");
            enabled = false;
            return;
        }

        defaultIntensities = new float[lights.Length];
        for (int i = 0; i < lights.Length; i++)
            defaultIntensities[i] = lights[i] ? lights[i].intensity : 0f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (lightCoroutine != null)
                StopCoroutine(lightCoroutine);
            lightCoroutine = StartCoroutine(Lights());
        }
    }

    IEnumerator Lights()
    {
        float timer = 0f;
        ChromaticAberration chromaticAberration = null;
        PaniniProjection paniniProjection = null;
        LensDistortion lensDistortion = null;

        if (volume != null && volume.profile != null)
        {
            volume.profile.TryGet(out chromaticAberration);
            volume.profile.TryGet(out paniniProjection);
            volume.profile.TryGet(out lensDistortion);
        }

        while (timer < duration)
        {
            float t = timer / duration;
            float lightRate = lightCurve.Evaluate(t);
            float volumeRate = volumeCurve.Evaluate(t);

            for (int i = 0; i < lights.Length; i++)
            {
                if (lights[i])
                    lights[i].intensity = defaultIntensities[i] + Mathf.Lerp(0, maxIntensity, lightRate);
            }

            if (chromaticAberration != null)
                chromaticAberration.intensity.value = Mathf.Lerp(0, 1, volumeRate);
            if (paniniProjection != null)
                paniniProjection.distance.value = Mathf.Lerp(0, 1, volumeRate);
            if (lensDistortion != null)
                lensDistortion.intensity.value = Mathf.Lerp(0, 1, volumeRate);

            timer += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i])
                lights[i].intensity = defaultIntensities[i];
        }

        if (chromaticAberration != null)
            chromaticAberration.intensity.value = 0f;
        if (paniniProjection != null)
            paniniProjection.distance.value = 0f;
        if (lensDistortion != null)
            lensDistortion.intensity.value = 0f;

        lightCoroutine = null;
    }
}
