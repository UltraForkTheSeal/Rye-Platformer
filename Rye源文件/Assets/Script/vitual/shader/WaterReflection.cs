using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WaterReflection : MonoBehaviour
{
    Material waterMaterial;
    SpriteRenderer spriteRenderer;
    Sprite sprite => spriteRenderer.sprite;
    Bounds bounds;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        waterMaterial = spriteRenderer.material;
        
    }

    // Update is called once per frame
    void Update()
    {
        bounds = spriteRenderer.bounds;
        float topY = bounds.max.y;  // 上边界的 Y 坐标
        //Debug.Log("top Position: " + topY);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(bounds.center.x, topY, 0));
        //Debug.Log("Screen Position: " + screenPos);
        float screenTopY = screenPos.y;  // 屏幕上的 Y 坐标（像素）
        float modifiedScreenTopY = screenTopY / Screen.height;  // 归一化到 [0, 1] 范围
        waterMaterial.SetFloat("_ReflectionLineY", modifiedScreenTopY);
    }

}
