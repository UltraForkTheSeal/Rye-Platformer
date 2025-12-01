using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBox : MonoBehaviour
{
    public float mirrorY;
    //GameObject mirror;
    [SerializeField] GameObject mirror;

    // Start is called before the first frame update
    void Start()
    {

        mirror = Instantiate(mirror, transform.position, Quaternion.identity);
        var MB = mirror.GetComponent<MirrorBox>();
        if (MB != null)
        {
            Destroy(MB);
        }
        // 删除除 SpriteRenderer 和 Collider 相关组件以外的所有组件
        RemoveUnwantedComponents(mirror);

        mirror.GetComponent<Rigidbody2D>().isKinematic = true;
        mirror.transform.position = new Vector3(transform.position.x, 2 * mirrorY - transform.position.y, transform.position.z);
        mirror.transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.x, transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        mirror.transform.position = new Vector3(transform.position.x, 2 * mirrorY - transform.position.y, transform.position.z);
    }
    void RemoveUnwantedComponents(GameObject obj)
    {
        Component[] components = obj.GetComponents<Component>();

        foreach (Component comp in components)
        {
            // 保留 Transform
            if (comp is Transform)
                continue;

            // 保留 SpriteRenderer
            if (comp is SpriteRenderer)
                continue;

            // 保留 Collider2D / Collider
            if (comp is Collider2D)
                continue;
            // if (comp is PlatformEffector2D)
            //     continue;

            // 删除其它组件
            Destroy(comp);
        }
    }

}
