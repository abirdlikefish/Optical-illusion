using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;
public class Observe<T>
{
    List<Component> m_coms;
    Func<T> m_getValue;
    Action<T> m_setValue;

    public Observe(Func<T> getValue, Action<T> setValue, List<Component> coms)
    {
        m_getValue = getValue;
        m_setValue = setValue;
        m_coms = coms;
    }

    public T Value
    {
        get => m_getValue();
        set
        {
            if (!value.Equals(m_getValue()))
            {
                m_setValue(value);
                OnValueChange();
            }
        }
    }

    void OnValueChange()
    {
        foreach(var m_com in m_coms)
            EditorUtility.SetDirty(m_com);
    }
}


[ExecuteInEditMode]
public class MyEditor : EditorWindow
{
    public static bool showAllCubeName = true;
    bool showAllCenterPoints = false;
    List<Cube> selectedCubes = new();
    List<CenterPoint> selectedCenterPoints = new();
    List<MyTrigger> selectedTriggers = new();
    Color lineColor = new(0x39 / 255f, 0xC5 / 255f, 0xBB / 255f);
    [MenuItem("Window/MyEditor")]
    public static void ShowWindow()
    {
        GetWindow<MyEditor>("MyEditor");
    }
    private void Update()
    {
        if (Application.isPlaying)
            return;
        Repaint();
    }
    void GetSelectedObjects()
    {
        foreach (var go in Selection.gameObjects)
        {
            Cube cube = go.GetComponent<Cube>();
            CenterPoint centerPoint = go.GetComponent<CenterPoint>();
            MyTrigger trigger = go.GetComponent<MyTrigger>();
            if (cube != null)
                selectedCubes.Add(cube);
            if (centerPoint != null)
                selectedCenterPoints.Add(centerPoint);
            if (trigger != null)
                selectedTriggers.Add(trigger);
        }
    }
    bool IsCubePureColor()
    {
        Cube.COLOR firstColor = (Cube.COLOR)(-1);
        foreach (var cube in selectedCubes)
        {
            if((int)firstColor == -1)
            {
                firstColor = cube.color;
                continue;
            }
            if (cube.color != firstColor)
                return false;
        }
        if ((int)firstColor == -1)
            return false;
        return true;
    }
    bool SelectedPlayer()
    {
        foreach (var it in Selection.gameObjects)
        {
            if (it.GetComponent<Player>())
                return true;
        }
        return false;
    }
    
    void OnGUI()
    {
        if (Application.isPlaying)
        {
            GUILayout.Label($"游戏运行时不可编辑", GUILayout.Width(450));
            return;
        }
        EditHelper.CollectAllCube_RefreshColorAndName_ClearInvalidTrigger();
        EditHelper.AllCubeMagnetPos();
        selectedCubes.Clear();
        selectedCenterPoints.Clear();
        selectedTriggers.Clear();
        if (Selection.activeGameObject)
        {
            GetSelectedObjects();
        }
        string selectedCubesName = "";
        foreach (var cube in selectedCubes)
        {
            selectedCubesName += cube.name + " ";
        }
        string selectedCenterPointsName = "";
        foreach (var centerPoint in selectedCenterPoints)
        {
            selectedCenterPointsName += centerPoint.cube.name + "::" +  centerPoint.name + " ";
        }
        string selectedTriggersName = "";
        foreach (var trigger in selectedTriggers)
        {
            selectedTriggersName += trigger.GetCurCenter().cube.name + "::" + trigger.GetCurCenter().name + "::" + trigger.name + " ";
        }

        Texture2D texture = new Texture2D(1, 1);
        lineColor = EditorGUILayout.ColorField("分割线颜色", lineColor);
        texture.SetPixel(0, 0, lineColor);
        texture.Apply();
        GUIStyle lineStyle = new GUIStyle();
        lineStyle.normal.background = texture;

        Tools.hidden = false;
        Tools.hidden |= SelectedPlayer() && Tools.current != Tool.Move;

        GUILayout.Box(GUIContent.none, lineStyle, GUILayout.ExpandWidth(true), GUILayout.Height(2));
        GUILayout.Label($"当前选中{selectedCubes.Count}个方块 ：{selectedCubesName}", GUILayout.Width(450));
        Tools.hidden |= selectedCubes.Count != 0 && Tools.current != Tool.Move;
        showAllCubeName = GUILayout.Toggle(showAllCubeName, "显示所有方块名称", GUILayout.Width(150));

        GUILayout.BeginHorizontal();
        {
            if (IsCubePureColor())
            {
                GUILayout.Label($"方块颜色都设为：");
                var observer = new Observe<Cube.COLOR>
                    (
                        () => selectedCubes[0].color,
                        (val) => { foreach (var cube in selectedCubes) cube.color = val; },
                        selectedCubes.Cast<Component>().ToList()
                    );
                observer.Value = (Cube.COLOR)EditorGUILayout.EnumPopup(observer.Value);
            }
        }
        GUILayout.EndHorizontal();
        
        
        if(selectedCubes.Count == 1)
        {

            var observer = new Observe<Vector3>
                (
                    () => selectedCubes[0].moveDelta,
                    (val) => selectedCubes[0].moveDelta = val,
                    new () { selectedCubes[0] }
                );
            observer.Value = EditorGUILayout.Vector3Field("这个方块的平移偏移值(来回)", observer.Value);

            var observer2 = new Observe<float>
                (
                    () => selectedCubes[0].moveSpeed,
                    (val) => selectedCubes[0].moveSpeed = val,
                    new() { selectedCubes[0] }
                );
            observer2.Value = EditorGUILayout.FloatField("这个方块的移动速度", observer2.Value);

            var observer3 = new Observe<Vector3>
                (
                    () => selectedCubes[0].rotateDelta,
                    (val) => selectedCubes[0].rotateDelta = val,
                    new() { selectedCubes[0] }
                );
            observer3.Value = EditorGUILayout.Vector3Field("这个方块的旋转偏移值(单向累积)", observer3.Value);

            var observer4 = new Observe<float>
                (
                    () => selectedCubes[0].rotateSpeed,
                    (val) => selectedCubes[0].rotateSpeed = val,
                    new() { selectedCubes[0] }
                );
            observer4.Value = EditorGUILayout.FloatField("这个方块的旋转速度", observer4.Value);
        }

        GUILayout.Box("", lineStyle, GUILayout.ExpandWidth(true), GUILayout.Height(2));
        GUILayout.Label($"当前选中{selectedCenterPoints.Count}个中心点 ：{selectedCenterPointsName}");
        Tools.hidden |= selectedCenterPoints.Count != 0;
        showAllCenterPoints = GUILayout.Toggle(showAllCenterPoints, "显示所有中心点", GUILayout.Width(150));

        GUILayout.BeginHorizontal();
        {
            GUI.enabled = selectedCenterPoints.Count == 1;
            if (GUILayout.Button("设为起点", GUILayout.Width(150)))
            {
                LevelManager.Instance.curLevel.staCenter = selectedCenterPoints[0];
                EditorUtility.SetDirty(LevelManager.Instance);
            }
            Player.Instance.Initialize();
            if (GUILayout.Button("设为终点", GUILayout.Width(150)))
            {
                LevelManager.Instance.curLevel.desCenter = selectedCenterPoints[0];
                EditorUtility.SetDirty(LevelManager.Instance);
            }
            PathFinder.Instance.Initialize();
            GUI.enabled = true;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            GUI.enabled = selectedCenterPoints.Count == 1;
            if (GUILayout.Button("创建平移按钮", GUILayout.Width(150)))
            {
                MyTriggerMoveCube g = Instantiate(MyTriggerManager.Instance.prefabMove, MyTriggerManager.Instance.transform);
                g.ArriveTarCenter(selectedCenterPoints[0]);
                showAllCenterPoints = false;
                //将选中物体变为生成的物体
                Selection.activeGameObject = g.gameObject;
            }
            if (GUILayout.Button("创建旋转按钮", GUILayout.Width(150)))
            {
                MyTriggerRotateCube g = Instantiate(MyTriggerManager.Instance.prefabRotate, MyTriggerManager.Instance.transform);
                g.ArriveTarCenter(selectedCenterPoints[0]);
                showAllCenterPoints = false;
                Selection.activeGameObject = g.gameObject;
            }
            GUI.enabled = true;
        }
        GUILayout.EndHorizontal();

        GUILayout.Box("", lineStyle, GUILayout.ExpandWidth(true), GUILayout.Height(2));
        GUILayout.Label($"当前选中{selectedTriggers.Count}个按钮 ：{selectedTriggersName}", GUILayout.Width(450));

        if (selectedTriggers.Count == 1)
        {
            Type type = selectedTriggers[0].GetType();
            string t1 = "";
            if (type == typeof(MyTriggerMoveCube))
                t1 = " 平移 的";
            else if(type == typeof(MyTriggerRotateCube))
                t1 = " 旋转 的";
            //获取按钮作用的物体的所有序号
            List<int> allIndex = new();
            foreach (var effectCube in selectedTriggers[0].effectCubes)
            {
                allIndex.Add(effectCube.Index);
            }
                
            GUILayout.Label($"这个按钮{t1}方块有");
            if(allIndex.Count == 0)
            {
                GUILayout.Label("无");
                if (GUILayout.Button("+", GUILayout.Width(30)))
                {
                    allIndex.Insert(0, 0);
                }
            }
            else
            {
                for (int i = 0; i < allIndex.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    allIndex[i] = int.Parse(GUILayout.TextField(allIndex[i].ToString(), GUILayout.Width(60)));
                    //增加，删除按钮
                    if (GUILayout.Button("+", GUILayout.Width(30)))
                    {
                        allIndex.Insert(i + 1, allIndex[i]);
                    }
                    if (GUILayout.Button("-", GUILayout.Width(30)))
                    {
                        allIndex.RemoveAt(i);
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.Label($"请填入方块序号 : 0 - {CubeCombiner.Instance.cubes.Count - 1}");
            if (GUILayout.Button("排序", GUILayout.Width(150)))
            {
                allIndex.Sort();
            }
            selectedTriggers[0].effectCubes.Clear();
            for (int i = 0; i < allIndex.Count; i++)
            {
                int index = allIndex[i];
                if (index < 0)
                    continue;
                if (CubeCombiner.Instance.transform.childCount < index + 1)
                {
                    Debug.LogError($"方块{index}不存在！");
                    continue;
                }
                selectedTriggers[0].effectCubes.Add(CubeCombiner.Instance.transform.GetChild(index).GetComponent<Cube>());
            }
        }

        GUILayout.BeginHorizontal();
        {
        }
        GUILayout.EndHorizontal();

        
        
        if (showAllCenterPoints)
            EditHelper.ShowAllCenterPoints();
        else
            EditHelper.HideAllCenterPoints();

        
    }

}
