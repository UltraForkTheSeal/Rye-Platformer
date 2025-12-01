using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Teleport : MonoBehaviour
{
    public bool enableTeleport = false;
    [SerializeField] private float _teleportYOrigin = 0f;
    [SerializeField] private CharacterControl _player;
    public int AboveTeleportPlaneFlag { get; private set; }

    [SerializeField] private UnityEvent _onTeleported;
    [SerializeField] private RumbleParams _teleportRumble;

    private void Awake()
    {
        AboveTeleportPlaneFlag = 1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(-100f, _teleportYOrigin, 0), new Vector3(100f, _teleportYOrigin, 0));
    }

    public void TeleportPlayer()
    {
        if (!enableTeleport) return;


        AboveTeleportPlaneFlag = -AboveTeleportPlaneFlag;

        // 位置镜像
        float yDistance = Mathf.Abs(_player.transform.position.y - _teleportYOrigin);
        _player.transform.position = new Vector3(_player.transform.position.x,
            _teleportYOrigin + AboveTeleportPlaneFlag * yDistance, _player.transform.position.z);

        // 修改gravityScale
        _player.FlipGravity();

        _onTeleported?.Invoke();
        RumbleManager.Instance.RumbleByParams(_teleportRumble);
        SoundFXManager.Instance.PlaySFX("Teleport", transform.position, 0.5f);
    }

    public void ResetTeleportFlag()
    {
        if (AboveTeleportPlaneFlag == -1)
        {
            TeleportPlayer();   
        }
        
        AboveTeleportPlaneFlag = 1;
    }

}
