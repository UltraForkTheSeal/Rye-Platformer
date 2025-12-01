using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwichOnOpen : MonoBehaviour
{
    Vector3 startPos;
    Vector3 endPos;
    public ContactPoint contactPoint;
    public float distance;
    public float duration;
    public float delay;
    public AnimationCurve curve;
    float _timer;
    public bool isPowered;
    Collider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        endPos = transform.position + Vector3.up * distance;
        contactPoint.OnPoweredChanged += HandleContactChanged;
        isPowered = false;
        boxCollider = GetComponent<Collider2D>();
        boxCollider.isTrigger = true;
        boxCollider.enabled = false;
    }

    // Update is called once per frame
    private void HandleContactChanged(bool _) {
        // 任何一个触点状态变化，重新更新外观
        if (contactPoint.isPowered)
            if (isPowered != true)
            {
                StartCoroutine("Opening");
                isPowered = true;
            }
    }
    IEnumerator Opening()
    {
        _timer = 0;
        while (_timer < delay + duration)
        {
            float rate = Mathf.Min(Mathf.Max((curve.Evaluate((_timer - delay) / duration)), 0), 1);
            transform.position = Vector3.Lerp(startPos, endPos, rate);
            _timer += Time.deltaTime;
            yield return null; 
        }
        transform.position = endPos;
        boxCollider.enabled = true;
        yield return null;   
    }
    private void OnDestroy() {
        contactPoint.OnPoweredChanged -= HandleContactChanged;
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     Debug.Log("Enter Trigger " + collision.name);
    //     if (collision.CompareTag("Player"))
    //         SceneManager.LoadScene("Level2");
    // }
}
