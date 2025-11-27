using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Collider2D))]
public class ContactPoint : MonoBehaviour
{
    // 用 List<EnergySource> 替代 HashSet<EnergySource>
    private List<EnergySource> activeSources = new List<EnergySource>();
    // 只要有一个源，算通电
    public bool isPowered => activeSources.Count > 0;

    // 邻居列表（初始化 + 动态碰撞维护）
    public List<ContactPoint> neighbors = new List<ContactPoint>();

    // 通／断电事件
    public event Action<bool> OnPoweredChanged;
    //初始化能量源
    [SerializeField] private EnergySource startWithSource;

    private void Awake()
    {
        //InitializeNeighborsFromParent();
        if(startWithSource != null)
        InitializeWithSource(startWithSource);

        // 如果一开始就有源，触发一次“通电”事件与广播
        if (isPowered)
        {
            OnPoweredChanged?.Invoke(true);
            Propagate(true, new HashSet<ContactPoint> { this });
        }
    }


    /// <summary>
    /// 添加或移除单一路径的 source
    /// </summary>
    public void SetPowered(bool on, EnergySource src)
    {
        bool wasPowered = isPowered;

        if (on)
        {
            // 添加新源（如果已在列表中则忽略）
            if (!activeSources.Contains(src))
            {
                activeSources.Add(src);
                // 从 0→1，触发事件与广播
                if (!wasPowered)
                {
                    OnPoweredChanged?.Invoke(true);
                    Propagate(true, new HashSet<ContactPoint> { this });
                }
            }
        }
        else
        {
            // 移除断电源
            if (activeSources.Remove(src))
            {
                // 从 1→0，触发断电事件与广播
                if (wasPowered && !isPowered)
                {
                    OnPoweredChanged?.Invoke(false);
                    Propagate(false, new HashSet<ContactPoint> { this });
                }
            }
        }
    }

    /// <summary>
    /// 广播当前状态到所有邻居，用 visited 防环
    /// </summary>
    private void Propagate(bool powerOn, HashSet<ContactPoint> visited)
    {
        
        foreach (var nb in neighbors)
        {
            if (visited.Contains(nb)) continue;
            visited.Add(nb);

            // 传播时，用当前点最后一个 source（或对方最后一个）作为参数
            EnergySource useSrc = null;
            if (powerOn)
            {
                // 本节点最新加入的 source
                useSrc = activeSources[activeSources.Count - 1];
            }
            else
            {
                // 断电时，对方如果还有 source，就给它最后一个；否则 null
                if (nb.activeSources.Count > 0)
                    useSrc = nb.activeSources[nb.activeSources.Count - 1];
            }

            nb.SetPowered(powerOn, useSrc);
            nb.Propagate(powerOn, visited);
        }
    }

    // /// <summary>
    // /// 初始化：收集同一父物体下所有触点，并同步它们的多路 source
    // /// </summary>
    // private void InitializeNeighborsFromParent()
    // {
    //     Transform parent = transform.parent;
    //     if (parent != null)
    //     {
    //         var allPoints = parent.GetComponentsInChildren<ContactPoint>();
    //         for (int i = 0; i < allPoints.Length; i++)
    //         {
    //             var cp = allPoints[i];
    //             if (cp != this && !neighbors.Contains(cp))
    //             {
    //                 neighbors.Add(cp);
    //             }
    //         }
    //     }

    //     // 同步兄弟节点上已有的所有 source
    //     for (int i = 0; i < neighbors.Count; i++)
    //     {
    //         var nb = neighbors[i];
    //         for (int j = 0; j < nb.activeSources.Count; j++)
    //         {
    //             SetPowered(true, nb.activeSources[j]);
    //         }
    //     }
    // }
    /// <summary>
    /// 手动将自己注册为某个 EnergySource 的根节点，并立即触发一次通电广播
    /// </summary>
    public void InitializeWithSource(EnergySource src)
    {
        // 1) 把自己加入该源的 connectedPoints 列表（可选，看你要不要让源也记录）  
        if (!src.connectedPoints.Contains(this))
        {
            src.connectedPoints.Add(this);
        }
        // 2) 直接调用 SetPowered 添加源并广播
        SetPowered(true, src);
    }

    // 2D 碰撞检测，动态添加／移除邻居并传播状态
    private void OnTriggerEnter2D(Collider2D other)
    {
        var otherCP = other.GetComponent<ContactPoint>();
        if (otherCP != null && !neighbors.Contains(otherCP))
        {
            neighbors.Add(otherCP);
            otherCP.neighbors.Add(this);

            // 动态导通：若任一已通电，就向对方传播
            if (isPowered)
            {
                otherCP.SetPowered(true, activeSources[activeSources.Count - 1]);
            }
            else if (otherCP.isPowered)
            {
                SetPowered(true, otherCP.activeSources[otherCP.activeSources.Count - 1]);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var otherCP = other.GetComponent<ContactPoint>();
        if (otherCP == null || !neighbors.Contains(otherCP)) return;

        // 1. 移除双向连接
        neighbors.Remove(otherCP);
        otherCP.neighbors.Remove(this);

        // 2. 对自己，检查每个源是否还能到根节点；不能到的就断开
        var mySourcesCopy = new List<EnergySource>(activeSources);
        foreach (var src in mySourcesCopy)
        {
            if (!IsReachableFromSource(src))
            {
                SetPowered(false, src);
            }
        }

        // 3. 对对方同样操作
        var otherSourcesCopy = new List<EnergySource>(otherCP.activeSources);
        foreach (var src in otherSourcesCopy)
        {
            if (!otherCP.IsReachableFromSource(src))
            {
                otherCP.SetPowered(false, src);
            }
        }
    }

    private bool IsReachableFromSource(EnergySource src)
    {
        var visited = new HashSet<ContactPoint>();
        var queue = new Queue<ContactPoint>();
        queue.Enqueue(this);
        visited.Add(this);

        while (queue.Count > 0)
        {
            var cp = queue.Dequeue();
            // 如果 cp 是 src 的一个根节点，则可达
            if (src.connectedPoints.Contains(cp)) return true;
            // 否则继续沿邻居遍历
            foreach (var nb in cp.neighbors)
            {
                if (!visited.Contains(nb))
                {
                    visited.Add(nb);
                    queue.Enqueue(nb);
                }
            }
        }
        return false;
    }
}
