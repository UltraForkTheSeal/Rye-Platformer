using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOnFall : MonoBehaviour
{
    public ContactPoint contactPoint;
    public Rigidbody2D m_rigidbody2D;
    public bool isFalling;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        contactPoint.OnPoweredChanged += HandleContactChanged;
        isFalling = false;
    }
    private void HandleContactChanged(bool _) {
        // 任何一个触点状态变化，重新更新外观
        if (contactPoint.isPowered)
            if (isFalling != true)
            {
                //触发
                m_rigidbody2D.gravityScale = 1;
                isFalling = true;
            }
    }

    private void OnDestroy() {
        contactPoint.OnPoweredChanged -= HandleContactChanged;
    }
}
