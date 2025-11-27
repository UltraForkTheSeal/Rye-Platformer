using UnityEngine;
using Cinemachine;

[SaveDuringPlay]
[AddComponentMenu("Cinemachine/Extensions/Dynamic Zoom Extension")]
public class DynamicZoomExtension : CinemachineExtension
{
    [Header("Target Settings")]
    public Transform player;

    [Header("Deadzone Settings")]
    [Tooltip("Normalized deadzone (0-1) where 1 = screen edge")]
    [Range(0.1f, 0.9f)]
    public float deadzone = 0.7f;


    [Tooltip("Damping factor for zoom changes")]
    public float zoomDamping = 2f;

    private float _initialFOV;
    private float _targetFOV;
    private float _currentFOV;
    private float _cameraDistance;

    protected override void Awake()
    {
        base.Awake();

        // 获取初始FOV值
        if (TryGetComponent<CinemachineVirtualCamera>(out var vcam))
        {
            _initialFOV = vcam.m_Lens.FieldOfView;
            _targetFOV = _initialFOV;
            _currentFOV = _initialFOV;
        }
    }

        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage,
            ref CameraState state,
            float deltaTime)
        {
        if (stage != CinemachineCore.Stage.Aim || player == null)
        {
            state.Lens.FieldOfView = _currentFOV;
            return;
        }
            // 计算相机到玩家的距离（深度）
            _cameraDistance = Mathf.Abs(state.RawPosition.z - player.position.z);
            if (_cameraDistance < 0.01f) _cameraDistance = 0.01f; // 防止除零错误

            // 核心计算：根据玩家Y位置和deadzone计算目标FOV
            float playerY = player.position.y;
            float deadzoneHeight = _cameraDistance * Mathf.Tan(_initialFOV * 0.5f * Mathf.Deg2Rad) * deadzone;
            //float softzoneHeight = _cameraDistance * Mathf.Tan(_initialFOV * 0.5f * Mathf.Deg2Rad) * softzone;

            if (Mathf.Abs(playerY) > deadzoneHeight)
            {
                // 当玩家超出deadzone时计算新FOV
                _targetFOV = 2f * Mathf.Atan(Mathf.Abs(playerY) / (_cameraDistance * deadzone)) * Mathf.Rad2Deg;
            }
            else
            {
                // 玩家在安全区域内，使用初始FOV
                _targetFOV = _initialFOV;
            }

        
        _currentFOV = Mathf.Lerp(_currentFOV, _targetFOV, deltaTime * zoomDamping);

            // 应用平滑的FOV变化
        state.Lens.FieldOfView = _currentFOV;
        }
}