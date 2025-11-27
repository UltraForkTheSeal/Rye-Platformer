using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class WavePoint : MonoBehaviour
{
    [Header("频率控制")]
    
    [Range(0.1f, 5f)] 
    public float frequency = 1f;      // 震动频率（Hz）
    [Range(0f, 1f)]
    public float dampingRatio = 0.2f; // 阻尼比例（0=无阻尼，1=临界阻尼）

    [Header("邻居连接")]
    public Transform rect;
    public WavePoint LeftNeighbor;
    public WavePoint RightNeighbor;

    [Header("高级设置")]
    public float maxForce = 15f;
    public float neighborInfluence = 0.6f; // 邻居影响力
    public float bottom;
    public float actForce = 10f;
    public CircleCollider2D col;

    private Rigidbody2D rb;
    public ParticleSystem Particle;
    public Transform ParticleTransform;
    private Vector2 restPosition;
    private float stiffness;
    private float criticalDamping;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        restPosition = transform.position;
        rb.gravityScale = 0;
        rect = transform.GetChild(0);
        UpdateFrequencyParameters();
        UpdateRect();
    }
    private void Update()
    {
        UpdateRect();
    }

    void FixedUpdate()
    {
        ApplySpringForce();
        ApplyNeighborForces();
        ApplyVelocityDamping();
 
    }

    void UpdateFrequencyParameters()
    {
        // 根据频率自动计算物理参数
        float angularFreq = 2 * Mathf.PI * frequency;  // ω = 2πf
        stiffness = rb.mass * angularFreq * angularFreq; // k = mω²
        criticalDamping = 2 * Mathf.Sqrt(rb.mass * stiffness); // c_c = 2√(mk)
    }

    void ApplySpringForce()
    {
        Vector2 displacement = restPosition - rb.position;
        rb.AddForce(displacement * stiffness);
    }

    void ApplyNeighborForces()
    {
        if(LeftNeighbor) if(LeftNeighbor.rb != null) ApplyNeighborForce(LeftNeighbor.rb);
        if(RightNeighbor) if(RightNeighbor.rb != null) ApplyNeighborForce(RightNeighbor.rb);
    }

    void ApplyNeighborForce(Rigidbody2D neighborRB)
    {
        Vector2 offset = neighborRB.position - rb.position;
        float force = Mathf.Clamp(offset.y * stiffness * neighborInfluence, 
                                -maxForce, maxForce);
        rb.AddForce(Vector2.up * force);
    }

    void ApplyVelocityDamping()
    {
        // 临界阻尼混合
        float dampingForce = criticalDamping * dampingRatio;
        rb.velocity *= Mathf.Clamp01(1 - dampingForce * Time.fixedDeltaTime);
    }

    public void ApplyDisturbance(float force)
    {
        rb.AddForce(Vector2.up * Mathf.Clamp(force, -maxForce, maxForce), 
                  ForceMode2D.Impulse);
    }
    void UpdateRect()
    {
        rect.localScale = new Vector3(rect.localScale.x, (transform.position.y-restPosition.y+bottom)/transform.lossyScale.y, rect.localScale.z);
        rect.localPosition = new Vector3(0, restPosition.y - transform.position.y - bottom, 0)/ 2 / transform.lossyScale.y;
        col.offset = new Vector2(0, restPosition.y - transform.position.y)/transform.lossyScale.y;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("OnTriggerEnter");
        if (collision.CompareTag("Player"))
        {   
            rb.AddForce(Vector2.up * actForce, ForceMode2D.Impulse);
            ParticleTransform.position = transform.position;
            Particle.Play();
        }
    }

    // 编辑器实时更新参数（可选）
#if UNITY_EDITOR
    void OnValidate()
    {
        if(rb == null) rb = GetComponent<Rigidbody2D>();
        UpdateFrequencyParameters();
    }
    #endif
}