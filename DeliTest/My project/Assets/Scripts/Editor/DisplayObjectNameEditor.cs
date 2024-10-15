using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Cube))] // 你可以根据需求更改为特定类型
public class DisplayObjectNameEditor : Editor
{

    private static GUIStyle labelStyle;

    // 在启用时初始化 GUIStyle
    void OnEnable()
    {
        labelStyle = new GUIStyle()
        {
            fontSize = 15,
            fontStyle = FontStyle.Bold,
            normal = new GUIStyleState { textColor = Color.white },
            alignment = TextAnchor.MiddleCenter

        };
        SceneView.duringSceneGui += OnSceneGUI;
    }
    private static void OnSceneGUI(SceneView sceneView)
    {
        if(MyEditor.showAllCubeName)
        {
            foreach(var cube in CubeCombiner.Instance.cubes)
            {
                // 获取当前选中的物体
                if (!cube)
                    continue;
                Transform tr = cube.transform;

                // 设置显示位置在物体的上方
                Vector3 position = tr.position; // 在物体上方 1.5 个单位显示

                // 绘制物体名称
                Handles.Label(position, tr.name, labelStyle);
            }
        }
        SceneView.RepaintAll();
    }
}
