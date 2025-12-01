using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public Action Died;
    public Action Respawned;
    private Rigidbody2D _rigidbody2D;
    private CharacterControl _character;
    private Teleport _teleport;

    private Transform _currentSpawnTransform;
    private int _rebornID = 0;

    [SerializeField] private float _respawnWaitDuration = 1f;
    private WaitForSeconds _respawnWait;
    private bool _duringRespawn;
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _character = GetComponent<CharacterControl>();
        _teleport = GetComponent<Teleport>();
        _respawnWait = new WaitForSeconds(_respawnWaitDuration);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IHazard hazard))
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Launcher"))
        {
            foreach (var contact in other.contacts)
            {
                // 与平台下方碰撞
                if (contact.normal.y < 0f && _character.OnGround)
                {
                    Die();
                    break;
                }
            }
        }
    }

    private void Die()
    {
        if (_duringRespawn)
        {
            return;
        }

        _duringRespawn = true;
        Died?.Invoke();
        StartCoroutine(RespawnCoroutine());
    }
    
    public void Respawn()
    {
        //复活时需要执行的逻辑
        transform.position = _currentSpawnTransform.position;
        _rigidbody2D.velocity = Vector2.zero;
        
        _character.DeathResetParams();
        if (_teleport.enableTeleport)
        {
            _teleport.ResetTeleportFlag();
        }
    }
    
    public void UpdateCheckPoint(int newCheckPointID,Transform newCheckPoint)
    {
        if (newCheckPointID > this._rebornID)
        {
            _currentSpawnTransform = newCheckPoint;
            _rebornID = newCheckPointID;
        }
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return _respawnWait;
        Respawned?.Invoke();
        _duringRespawn = false;
    }
    
}
