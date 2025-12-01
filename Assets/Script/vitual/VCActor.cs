using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VCActor : MonoBehaviour
{
    [SerializeField]
    GameObject AreaVirtualCamera;
    public int Priority = 1; // 设置默认优先级
    void Awake()
    {
        if (AreaVirtualCamera == null)
        {
            for (int i = 0; i < transform.childCount;i++)
            {
                if (transform.GetChild(i).gameObject.TryGetComponent<CinemachineVirtualCamera>(out var virtualCamera))
                 {
                    AreaVirtualCamera =  transform.GetChild(i).gameObject;
                    break;
                 }
            }           
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Enter Trigger " + collision.name);
        if (collision.CompareTag("Player"))
        {
            AreaVirtualCamera.GetComponent<CinemachineVirtualCamera>().Priority = Priority;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            AreaVirtualCamera.GetComponent<CinemachineVirtualCamera>().Priority = -1;
    }
}
