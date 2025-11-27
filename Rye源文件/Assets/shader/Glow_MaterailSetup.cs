using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glow_MaterailSetup : MonoBehaviour
{
    Material Glow_material;
    SpriteRenderer spriteRenderer;
    public Sprite GlowMap;
    public bool UseCollider = false;
    public float duration = 0.5f;
    [Range(0, 1)]
    public float initialIntensity = 1f;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Glow_material = spriteRenderer.material;
        //Debug.Log(Glow_material.name);

        Glow_material.SetTexture("_GlowTex", GlowMap.texture);
        if (UseCollider)
        {
            Glow_material.SetFloat("_GlowIntensity", 0f);
        }
        else
        {
            Glow_material.SetFloat("_GlowIntensity", initialIntensity);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && UseCollider)
        {
            StartCoroutine(TurnGlow());
            UseCollider = false;
        }
    }
    IEnumerator TurnGlow()
    {
        float progress = 0f;
        while (progress < 1f)
        {
            progress += Time.deltaTime / duration;
            Glow_material.SetFloat("_GlowIntensity", progress);
            yield return null;
        }
        // Ensure progress is set to 1 after completion
        Glow_material.SetFloat("_GlowIntensity", 1f);
    }
}
