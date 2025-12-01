using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class Conductor : MonoBehaviour {
    [Header("所有挂在本导体上的触点")]
    public List<ContactPoint> contactPoints;

    [Header("材质设置")]
    public SpriteRenderer rend;
    Material material;
    public Color onColor = Color.green;
    public Color offColor = Color.red;


    private void Awake()
    {
        if (rend == null) rend = GetComponentInChildren<SpriteRenderer>();
        material = rend.material;

        // 1) 如果编辑器里没手动挂点，就自动搜集同一父物体下的所有 ContactPoint
        if (contactPoints == null || contactPoints.Count == 0)
        {
            contactPoints = GetComponentsInChildren<ContactPoint>().ToList();
        }

        // 2) 订阅每个触点的状态变更事件
        foreach (var cp in contactPoints)
        {
            cp.OnPoweredChanged += HandleContactChanged;
        }

        UpdateAppearance();
    }
    private void Start()
    {
        // 3) 初始化外观
        Invoke("UpdateAppearance", 0.5f);
    }

    private void HandleContactChanged(bool _) {
        // 任何一个触点状态变化，重新更新外观
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        // 只要有一个通电，就算通电
        bool anyPowered = contactPoints.Any(cp => cp.isPowered);
        //rend.material = anyPowered ? onMat : offMat;
        //rend.color = anyPowered ? Color.green : Color.red; // 可选：通电时变色
                                                           // 可选：同时触发粒子、音效
        material.SetFloat("_GlowIntensity", anyPowered ? 1f : 0f); // 假设有个着色器属性控制通电效果
    }

    private void OnDestroy() {
        // 记得取消订阅，防止内存泄漏
        foreach (var cp in contactPoints) {
            cp.OnPoweredChanged -= HandleContactChanged;
        }
    }
}
