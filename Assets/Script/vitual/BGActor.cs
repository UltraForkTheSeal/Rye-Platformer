
using System.Collections.Generic;
using UnityEngine;

public class BGActor : MonoBehaviour
{
    public List<GameObject> BGObject; // 背景体
    public bool ActiveOrDisactive = false; // 是否激活背景
    // Start is called before the first frame update

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter");
        if (collision.CompareTag("Player"))
        {
            foreach (var bg in BGObject)
            {
                bg.SetActive(ActiveOrDisactive);
            }
        }
    }
}
