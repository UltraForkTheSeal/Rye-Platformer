using System.Collections;

using DG.Tweening;
using UnityEngine;


public class FragilePlatform : MonoBehaviour
{
    //private bool isEnter = false;
    private Vector3 m_OriginalPosition;
    private Vector3 m_ChildPos;
    Rigidbody2D m_rigidbody2D;
    Collider2D[] collider2Ds;

    private void Start()
    {
        m_OriginalPosition = this.transform.position;
        m_ChildPos = transform.GetChild(0).transform.localPosition;
        m_rigidbody2D = this.GetComponent<Rigidbody2D>();
        m_rigidbody2D.bodyType = RigidbodyType2D.Static;
        collider2Ds = this.GetComponents<Collider2D>();
    }
    
    private void OnCollisionEnter2D(Collision2D m_collision)
    {
        Debug.Log("Fragile Platform is here");
        if (m_collision.gameObject.CompareTag("Player"))
            StartCoroutine(FPlatformFallenTimer());
    }

    IEnumerator FPlatformFallenTimer()
    {
        FPlatformFallen();
        SoundFXManager.Instance.PlaySFX("BlockFall",transform.position,0.2f);
        
        yield return new WaitForSeconds(1.5f);
        useGravity();
        StartCoroutine(BackToTransform());
    }

    private void FPlatformFallen()
    {
        this.transform.GetChild(0).DOShakePosition(1.5f, 0.03f,fadeOut:false,vibrato:20);
        //this.transform.DOShakePosition(duration:1.5f, strength:0.03f);
    }

    private void useGravity()
    {
        m_rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        m_rigidbody2D.freezeRotation = true;
        m_rigidbody2D.gravityScale = 6f;
        foreach (var i in collider2Ds)
        {
            i.enabled = false;
        }
    }

    IEnumerator BackToTransform()
    {
        yield return new WaitForSeconds(2f);
        BackToOriginalPlace();
        yield return new WaitForSeconds(1f);
        SoundFXManager.Instance.PlaySFX("BlockRestore",transform.position,0.5f);
        //rigidbody2D.velocity = new Vector2(0, 0);
        m_rigidbody2D.bodyType = RigidbodyType2D.Static;
        foreach(var i in collider2Ds)
        {
            i.enabled = true;
        }
    }

    void BackToOriginalPlace()
    { 
        //this.transform.position = m_OriginalPosition;
        transform.GetChild(0).transform.localPosition = m_ChildPos;
        this.transform.DOMove(m_OriginalPosition, 1f);
    }
}