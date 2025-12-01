using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFov : MonoBehaviour
{
    bool FovOrDepth = false;
    public Transform player;
    public CinemachineVirtualCamera virtualCamera;
    CinemachineFramingTransposer framingTransposer;

    [Header("FOV调整参数")]
    public float defaultFOV = 5f;      // 默认正交Size
    public float maxFOV = 8f;          // 最大正交Size
    public float minXBoundary = -5f;   // 玩家左边界
    public float maxXBoundary = 5f;    // 玩家右边界
    public float adjustmentSpeed = 2f; // 调整速度
    // Start is called before the first frame update
    void Start()
    {
        if (virtualCamera == null)
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (player == null)
            player = virtualCamera.LookAt;

    }

    // Update is called once per frame
    void Update()
    {

        if (player == null) return;

        // 计算玩家在边界范围内的偏移比例 (0-1)
        float normalizedOffset = CalculatePlayerOffset();

        // 根据偏移量计算目标FOV
        float targetFOV = Mathf.Lerp(defaultFOV, maxFOV, normalizedOffset);

        // 平滑过渡到目标FOV
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(
            virtualCamera.m_Lens.OrthographicSize,
            targetFOV,
            adjustmentSpeed * Time.deltaTime
        );
    }
    private float CalculatePlayerOffset()
    {
        float playerX = player.position.x;
        float offset = 0f;

        // 计算左侧超出
        if (playerX < minXBoundary)
        {
            offset = Mathf.Abs(minXBoundary - playerX) / (minXBoundary - virtualCamera.transform.position.x);
        }
        // 计算右侧超出
        else if (playerX > maxXBoundary)
        {
            offset = Mathf.Abs(playerX - maxXBoundary) / (virtualCamera.transform.position.x - maxXBoundary);
        }

        return Mathf.Clamp01(offset);
    }
}
