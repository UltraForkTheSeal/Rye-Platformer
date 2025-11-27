using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class WaveGenerator : MonoBehaviour
{
    [Header("生成设置")]
    public GameObject wavePointPrefab; // 需要提前创建好的WavePoint预制体
    public int pointCount = 20;        // 生成点数量
    public float spacing = 0.5f;       // 点间距
    public float bottom = 2;
    

    [Header("物理参数预设")]
    public float defaultFrequency = 1.2f;
    public float defaultDamping = 0.3f;
    public float defaultNeighborInfluence = 0.6f;

    [Header("高级设置")]
    public bool generateOnStart = true;
    //public bool addColliders = true;
    //public float colliderRadius = 0.2f;
    public float actForce = 10f;
    ParticleSystem Particle;
    public GameObject ParticlePrefab;
    private Transform ParticleTransform;
    private WavePoint[] wavePoints;

    void Start()
    {
        // 清理旧点
        if (generateOnStart)
            ClearExistingPoints();
        
        if (ParticleTransform == null)
            ParticleTransform = Instantiate(ParticlePrefab, parent: transform).transform;
        if (Particle == null)
        Particle = ParticleTransform.gameObject.GetComponent<ParticleSystem>();
        Particle.Stop();
        if (generateOnStart) GenerateWavePoints();
    }

    // 生成波浪点的主方法
    public void GenerateWavePoints()
    {


        // 初始化数组
        wavePoints = new WavePoint[pointCount];

        // 循环生成点
        for (int i = 0; i < pointCount; i++)
        {
            // 计算位置
            Vector2 pos = new Vector2(transform.position.x,transform.position.y) + Vector2.right * spacing * i;
            
            // 实例化预制体
            GameObject pointObj = Instantiate(wavePointPrefab, pos, Quaternion.identity, transform);
            pointObj.name = $"WavePoint_{i}";
            
            // 获取组件
            WavePoint wp = pointObj.GetComponent<WavePoint>();
            wavePoints[i] = wp;

            // 配置物理参数
            wp.frequency = defaultFrequency;
            wp.dampingRatio = defaultDamping;
            wp.neighborInfluence = defaultNeighborInfluence;
            wp.bottom = bottom;
            wp.rect.localScale = new Vector3 (spacing/ pointObj.transform.localScale.x, wp.rect.localScale.y, wp.rect.localScale.z);
            wp.rect.localPosition = (Vector3.zero - new Vector3(0, bottom, 0)) / 2;
            wp.actForce = actForce;
            wp.Particle = Particle;
            wp.ParticleTransform = ParticleTransform;

            // 添加碰撞体（可选）
            // if (addColliders)
            // {
            //     CircleCollider2D col = pointObj.AddComponent<CircleCollider2D>();
            //     col.radius = colliderRadius;
            //     col.isTrigger = true;
            // }

            // 连接邻居（跳过第一个点）
            if(i > 0)
            {
                wp.LeftNeighbor = wavePoints[i - 1];
                wavePoints[i - 1].RightNeighbor = wp;
            }
            //修改rect

        }

        // 配置首尾点
        ConfigureEndPoints();
    }

    // 配置起点和终点的特殊属性
    void ConfigureEndPoints()
    {
        if(wavePoints.Length < 2) return;

        // 第一个点
        WavePoint first = wavePoints[0];
        first.GetComponent<Rigidbody2D>().mass = 2f; // 增加质量
        //first.tag = "WaveAnchor"; // 添加特殊标签

        // 最后一个点
        WavePoint last = wavePoints[wavePoints.Length - 1];
        last.GetComponent<Rigidbody2D>().mass = 2f;
        //last.tag = "WaveAnchor";
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(pointCount * spacing, 0, 0));
        Gizmos.DrawLine(transform.position - new Vector3(0, bottom, 0), transform.position + new Vector3(pointCount * spacing, 0, 0) - new Vector3(0, bottom, 0));

    }

    // 清除已有波浪点
    public void ClearExistingPoints()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        wavePoints = null;
    }

    // 编辑器按钮（需要在UnityEditor命名空间下）
    #if UNITY_EDITOR
    [UnityEditor.MenuItem("Wave/Create Wave Generator")]
    static void CreateWaveGenerator()
    {
        GameObject generator = new GameObject("WaveGenerator");
        generator.AddComponent<WaveGenerator>();
    }

    [ContextMenu("立即生成波浪点")]
    void EditorGenerate()
    {
        GenerateWavePoints();
    }

    [ContextMenu("清除所有波浪点")]
    void EditorClear()
    {
        ClearExistingPoints();
    }
    #endif
}