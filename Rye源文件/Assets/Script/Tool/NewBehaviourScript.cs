using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR


public class SceneScalerWindow : EditorWindow
{

    private float referenceZ = 0;
    private float CamerareferenceZ = 0;

    private float baseScale = 1.0f;
    private float scaleFactor = 0.1f;
    private bool realtimeUpdate = false;
    [MenuItem("Tools/场景缩放工具")]
    static void Init()
    {
        var window = GetWindow<SceneScalerWindow>();
        window.titleContent = new GUIContent("场景缩放器");
        window.Show();
    }

    void OnGUI()
    {

        // 在OnGUI内添加：
        GUILayout.Label("缩放设置", EditorStyles.boldLabel);
        referenceZ = EditorGUILayout.FloatField("参考Z位置", referenceZ);
        CamerareferenceZ = EditorGUILayout.FloatField("相机参考Z位置", CamerareferenceZ);
        baseScale = EditorGUILayout.FloatField("基础缩放", baseScale);
        scaleFactor = EditorGUILayout.Slider("缩放系数", scaleFactor, 0, 1);
        realtimeUpdate = EditorGUILayout.Toggle("实时更新", realtimeUpdate);

        if (GUILayout.Button("应用缩放"))
        {
            ApplyScaling();
        }
    }
    void ApplyScaling()
    {
        if (Selection.gameObjects.Length == 0)
        {
            Debug.LogWarning("未选中任何物体！");
            return;
        }

        foreach (GameObject obj in Selection.gameObjects)
        {
            float zDistance = obj.transform.position.z - referenceZ;
            float calculatedScale = (zDistance - CamerareferenceZ)* scaleFactor /(referenceZ - CamerareferenceZ);

            // 保持其他轴不变，仅修改XY缩放（根据需求调整）
            obj.transform.localScale = new Vector3(
                calculatedScale,
                calculatedScale,
                obj.transform.localScale.z
            );
        }
    }
    private void OnEnable()
    {
        EditorApplication.update += OnEditorUpdate;
    }

    private void OnDisable()
    {
        EditorApplication.update -= OnEditorUpdate;
    }

    void OnEditorUpdate()
    {
        if (realtimeUpdate && Selection.activeGameObject != null)
        {
            ApplyScaling();
        }
    }
}

#endif