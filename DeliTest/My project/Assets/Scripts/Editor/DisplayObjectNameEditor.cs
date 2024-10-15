using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Cube))] // ����Ը����������Ϊ�ض�����
public class DisplayObjectNameEditor : Editor
{

    private static GUIStyle labelStyle;

    // ������ʱ��ʼ�� GUIStyle
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
                // ��ȡ��ǰѡ�е�����
                if (!cube)
                    continue;
                Transform tr = cube.transform;

                // ������ʾλ����������Ϸ�
                Vector3 position = tr.position; // �������Ϸ� 1.5 ����λ��ʾ

                // ������������
                Handles.Label(position, tr.name, labelStyle);
            }
        }
        SceneView.RepaintAll();
    }
}
