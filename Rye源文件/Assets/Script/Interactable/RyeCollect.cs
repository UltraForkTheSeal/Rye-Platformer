using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
using Sequence = DG.Tweening.Sequence;

public class RyeCollect : MonoBehaviour
{
    [Header("Animation Settings")]
    public float floatHeight = 0.5f;        // 上下浮动高度
    public float floatDuration = 1.5f;      // 浮动周期
    public float stretchScale = 1.2f;       // 拉伸缩放比例
    public float stretchDuration = 0.8f;    // 拉伸动画时长
    public float collectDuration = 0.5f;    // 收集动画时长
    public float scaleDuration = 0.3f;      // 缩放动画时长
    public float trackingSpeed = 5f;        // 追踪玩家的速度
    
    [Header("Effects")]
    public ParticleSystem collectParticles; // 收集时的粒子效果
    public ParticleSystem idleParticles;    // 待机时的粒子效果
    
    [Header("Shader Settings")]
    public SpriteRenderer itemRenderer;     // 物品渲染器
    public string glowProperty = "_GlowIntensity"; // 发光强度属性名
    public float maxGlowIntensity = 3f;     // 最大发光强度
    
    private Vector3 startPosition;          // 初始位置
    private Vector3 originalScale;          // 原始缩放
    private Tween floatTween;               // 浮动动画引用
    private Tween stretchTween;             // 拉伸动画引用
    private Transform playerTarget;         // 玩家目标
    private bool isCollecting;              // 是否正在收集
    private float collectStartTime;         // 收集开始时间


    public event Action OnCollected;

    void Start()
    {
        startPosition = transform.position;
        originalScale = transform.localScale;

        isCollecting = false;           // 是否正在收集

        
        // 开始浮动和拉伸动画
        StartFloating();
        StartStretching();
        
        // 随机化初始位置
        transform.position = startPosition + new Vector3(Random.Range(-0.1f, 0.1f), 0, 0);
        
        // 开始待机粒子效果
        if (idleParticles != null)
        {
            idleParticles.Play();
        }
    }

    void Update()
    {
        // 如果正在收集且玩家目标存在，持续追踪玩家
        if (isCollecting && playerTarget != null)
        {
            TrackPlayer();
        }
    }

    void StartFloating()
    {
        // 上下浮动动画
        floatTween = transform.DOMoveY(startPosition.y + floatHeight, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(gameObject);
    }

    void StartStretching()
    {
        // 拉伸动画 - 在Y轴上拉伸，X轴上压缩
        stretchTween = transform.DOScale(
                new Vector3(originalScale.x * (1f / stretchScale), 
                            originalScale.y * stretchScale, 
                            originalScale.z), 
                stretchDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollecting)
        {
            OnCollected?.Invoke();
            
            
            
            playerTarget = other.transform;
            Collect();
        }
    }

    public void Collect()
    {
        isCollecting = true;
        collectStartTime = Time.time;
        
        // 停止所有动画
        floatTween?.Kill();
        stretchTween?.Kill();
        DOTween.Kill(transform);
        
        // 禁用碰撞体
        GetComponent<Collider2D>().enabled = false;
        
        // 立即播放收集粒子效果
        if (collectParticles != null)
        {
            collectParticles.transform.position = transform.position;
            collectParticles.transform.parent = null;
            collectParticles.Play();
            Destroy(collectParticles.gameObject, collectParticles.main.duration);
        }
        
        // 停止并隐藏待机粒子
        if (idleParticles != null)
        {
            idleParticles.Stop();
            idleParticles.gameObject.SetActive(false);
        }
        
        // 创建收集动画序列
        Sequence collectSequence = DOTween.Sequence();
        
        // 1. 先向上弹跳
        collectSequence.Append(transform.DOMoveY(transform.position.y + 0.3f, 0.2f).SetEase(Ease.OutQuad));
        
        // 2. 开始追踪玩家
        // 这里不再使用DOMove，而是在Update中持续追踪
        
        // 3. 缩小动画
        collectSequence.Join(transform.DOScale(0.01f, scaleDuration).SetEase(Ease.OutQuad));
        
        // 4. 发光效果增强
        if (itemRenderer != null && itemRenderer.material.HasProperty(glowProperty))
        {
            collectSequence.Join(DOTween.To(
                () => itemRenderer.material.GetFloat(glowProperty),
                x => itemRenderer.material.SetFloat(glowProperty, x),
                maxGlowIntensity, 0.2f));
        }
        
        // 5. 淡出效果
        if (itemRenderer != null)
        {
            collectSequence.Join(itemRenderer.DOFade(0f, collectDuration));
        }
        
        // 动画完成后的回调
        collectSequence.OnComplete(() => {
            // 销毁收集物
            Destroy(gameObject);
        });
    }

    void TrackPlayer()
    {
        // 计算收集进度（0到1）
        float progress = Mathf.Clamp01((Time.time - collectStartTime) / collectDuration);
        
        // 使用缓动函数使移动更自然
        float easedProgress = EaseInQuad(progress);
        
        // 计算新位置（向玩家移动）
        Vector3 targetPosition = playerTarget.position;
        transform.position = Vector3.Lerp(transform.position, targetPosition, 
                                         trackingSpeed * Time.deltaTime);
        
        // 当接近玩家时，加速移动
        float distance = Vector3.Distance(transform.position, targetPosition);
        if (distance < 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, 
                                                   targetPosition, 
                                                   trackingSpeed * 2 * Time.deltaTime);
        }
    }

    // 二次缓入函数
    float EaseInQuad(float t)
    {
        return t * t;
    }

    void OnDestroy()
    {
        // 确保所有动画被正确清理
        floatTween?.Kill();
        stretchTween?.Kill();
    }
}