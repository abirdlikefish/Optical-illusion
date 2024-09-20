using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
[Serializable]

public class CenterPoint : MonoBehaviour
{
    [Header("显示所有中心点")]
    public bool showAllCenterPoints = false;
    [Header("隐藏所有中心点")]
    public bool hideAllCenterPoints = false;
    [Header("设为起点")]
    public bool setSta = false;
    [Header("设为终点")]
    public bool setDes = false;
    [Header("创建平移按钮")]
    public bool createMoveButton = false;
    [Header("创建旋转按钮")]
    public bool createRotateButton = false;

    #region HideInSpector
    [HideInInspector]
    public Cube cube;
    [HideInInspector]
    public int axisId => transform.localPosition.x != 0 ? 0 : transform.localPosition.y != 0 ? 1 : 2;
    HashSet<CenterPoint> overlapPoints = new();
    public HashSet<CenterPoint> nextPoints = new();
    [HideInInspector]
    public bool visited;
    [HideInInspector]
    public CenterPoint lastPointInPath;
    [HideInInspector]
    public CenterPoint nextPointInPath;
    [HideInInspector]
    public List<MyTrigger> myTriggers = new();
    #endregion
    public void OnPlayerEnter()
    {
        foreach (var myTrigger in myTriggers)
        {
            myTrigger.DoTrigger();
        }
        if (LevelManager.Instance.curLevel.desCenter == this)
            LevelManager.Instance.PassCurLevel();
    }
    public bool IsVisible => transform.position.z <= cube.transform.position.z + 0.05f;
    public bool IsNotVisible => transform.position.z >= cube.transform.position.z - 0.05f;


    private void OnValidate()
    {
#if UNITY_EDITOR
        if (showAllCenterPoints)
        {
            showAllCenterPoints = false;
            foreach (var cube in CubeCombiner.Instance.cubes)
            {
                foreach (var centerPoint in cube.centerPoints)
                {
                    centerPoint.GetComponent<MeshRenderer>().enabled = true;
                }
            }
            return;
        }
        if (hideAllCenterPoints)
        {
            hideAllCenterPoints = false;
            foreach (var cube in CubeCombiner.Instance.cubes)
            {
                foreach (var centerPoint in cube.centerPoints)
                {
                    centerPoint.GetComponent<MeshRenderer>().enabled = false;
                }
            }
            return;
        }
        if (setSta)
        {
            setSta = false;
            LevelManager.Instance.curLevel.staCenter = this;
            EditorUtility.SetDirty(LevelManager.Instance);
            return;
        }
        if(setDes)
        {
            setDes = false;
            LevelManager.Instance.curLevel.desCenter = this;
            EditorUtility.SetDirty(LevelManager.Instance);
            return;
        }
        if(createMoveButton)
        {
            createMoveButton = false;
            
            MyTriggerMoveCube g = Instantiate(MyTriggerManager.Instance.prefabMove, MyTriggerManager.Instance.transform);
            g.ArriveTarCenter(this);
            return;
        }
        if (createRotateButton)
        {
            createRotateButton = false;

            MyTriggerRotateCube g = Instantiate(MyTriggerManager.Instance.prefabRotate, MyTriggerManager.Instance.transform);
            g.ArriveTarCenter(this);
            return;
        }
#endif
    }
    public void ClearInvalidTrigger()
    {
        for (int i = 0; i < myTriggers.Count; i++)
        {
            MyTrigger myTrigger = myTriggers[i];
            if (myTrigger == null)
            {
                myTriggers.Remove(myTrigger);
            }
        }
    }
    public void Awake()
    {
        cube = transform.parent.GetComponent<Cube>();
        name = cube.name + "::" + name;
    }
    private void Update()
    {
        ShowPossiblePath();
    }
    void ShowPossiblePath()
    {
        foreach (var np in nextPoints)
        {
            if (Config.Instance.showPossiblePath)
                Debug.DrawLine(transform.position, np.transform.position, Color.red);
        }
    }
    public void AddOverlapPoint(CenterPoint thatPoint)
    {
        if (!overlapPoints.Contains(thatPoint))
        {
            overlapPoints.Add(thatPoint);
            thatPoint.AddOverlapPoint(this);
        }
        foreach (var centerPoint in cube.centerPoints)
        {
            if (CubeCombiner.Instance.IsCenterSameAxis(this, centerPoint))
                continue;
            centerPoint.AddNextPoint(thatPoint.cube.GetSameDeltaCenterPoint(centerPoint));
        }
    }
    public void ClearOverlapPoint(CenterPoint thatPoint)
    {
        if (!overlapPoints.Contains(thatPoint))
            return;
        overlapPoints.Remove(thatPoint);
        foreach (var centerPoint in cube.centerPoints)
        {
            if (CubeCombiner.Instance.IsCenterSameAxis(this, centerPoint))
                continue;
            centerPoint.ClearNextPoint(thatPoint.cube.GetSameDeltaCenterPoint(centerPoint));
        }
    }
    public void AddNextPoint(CenterPoint thatPoint)
    {
        if (IsNotVisible || Obstacled(this) || thatPoint.IsNotVisible || Obstacled(thatPoint))
        {
            ClearNextPoint(thatPoint);

            return;
        }

        if (!nextPoints.Contains(thatPoint))
        {
            nextPoints.Add(thatPoint);
            thatPoint.AddNextPoint(this);
        }
    }
    public void ClearNextPoint(CenterPoint thatPoint)
    {
        if (!nextPoints.Contains(thatPoint))
            return;
        nextPoints.Remove(thatPoint);
        thatPoint.ClearNextPoint(this);
    }

    bool Obstacled(CenterPoint centerPoint)
    {
        //已知正方体的中心坐标和某个面的中心点坐标，求这个面的四个顶点坐标
        //正方体的局部坐标转换为世界坐标
        Vector3[] vertices = GetFaceVertices(centerPoint.cube.transform.localPosition,1,centerPoint.transform.localPosition);

        for (int i=0;i< 4; i++)
        {
            vertices[i] -= centerPoint.cube.transform.localPosition;
            vertices[i] = Vector3.Lerp(centerPoint.transform.localPosition, vertices[i], Config.Instance.obSampleScale);
            vertices[i] = centerPoint.cube.transform.TransformPoint(vertices[i]);
        }
        for (int i = 0; i < Config.Instance.obSampleCount; i++)
        {
            for (int j = 0; j < Config.Instance.obSampleCount; j++)
            {
                Vector3 samplePoint = Vector3.Lerp(Vector3.Lerp(vertices[0], vertices[1], i / (float)(Config.Instance.obSampleCount - 1)), Vector3.Lerp(vertices[3], vertices[2], i / (float)(Config.Instance.obSampleCount - 1)), j / (float)(Config.Instance.obSampleCount - 1));
                RaycastHit[] raycastHits = Physics.RaycastAll(samplePoint + Vector3.forward * 0.05f, -Vector3.forward, 1000f);
                foreach (var raycastHit in raycastHits)
                {
                    if (raycastHit.collider.transform == centerPoint.cube.transform)
                        continue;
                    return true;
                }
            }
        }
        return false;
    }
    public Vector3 CenterToWorldPos(GameObject go)
    {
        return transform.position + (transform.position - cube.transform.position) * go.transform.lossyScale.x;
    }
    Vector3[] GetFaceVertices(Vector3 cubeCenter, float size, Vector3 faceCenter)
    {
        faceCenter = faceCenter + cubeCenter;
        Vector3[] vertices = new Vector3[4];
        float halfSize = size / 2;
        // 根据面中心点确定面
        if (Mathf.Abs(faceCenter.y - (cubeCenter.y + halfSize)) < 0.01f) // 上面
        {
            vertices[0] = new Vector3(cubeCenter.x - halfSize, cubeCenter.y + halfSize, cubeCenter.z - halfSize);
            vertices[1] = new Vector3(cubeCenter.x + halfSize, cubeCenter.y + halfSize, cubeCenter.z - halfSize);
            vertices[2] = new Vector3(cubeCenter.x + halfSize, cubeCenter.y + halfSize, cubeCenter.z + halfSize);
            vertices[3] = new Vector3(cubeCenter.x - halfSize, cubeCenter.y + halfSize, cubeCenter.z + halfSize);
        }
        else if (Mathf.Abs(faceCenter.y - (cubeCenter.y - halfSize)) < 0.01f) // 下面
        {
            vertices[0] = new Vector3(cubeCenter.x - halfSize, cubeCenter.y - halfSize, cubeCenter.z - halfSize);
            vertices[1] = new Vector3(cubeCenter.x + halfSize, cubeCenter.y - halfSize, cubeCenter.z - halfSize);
            vertices[2] = new Vector3(cubeCenter.x + halfSize, cubeCenter.y - halfSize, cubeCenter.z + halfSize);
            vertices[3] = new Vector3(cubeCenter.x - halfSize, cubeCenter.y - halfSize, cubeCenter.z + halfSize);
        }
        else if (Mathf.Abs(faceCenter.x - (cubeCenter.x + halfSize)) < 0.01f) // 右面
        {
            vertices[0] = new Vector3(cubeCenter.x + halfSize, cubeCenter.y - halfSize, cubeCenter.z - halfSize);
            vertices[1] = new Vector3(cubeCenter.x + halfSize, cubeCenter.y - halfSize, cubeCenter.z + halfSize);
            vertices[2] = new Vector3(cubeCenter.x + halfSize, cubeCenter.y + halfSize, cubeCenter.z + halfSize);
            vertices[3] = new Vector3(cubeCenter.x + halfSize, cubeCenter.y + halfSize, cubeCenter.z - halfSize);
        }
        else if (Mathf.Abs(faceCenter.x - (cubeCenter.x - halfSize)) < 0.01f) // 左面
        {
            vertices[0] = new Vector3(cubeCenter.x - halfSize, cubeCenter.y - halfSize, cubeCenter.z - halfSize);
            vertices[1] = new Vector3(cubeCenter.x - halfSize, cubeCenter.y - halfSize, cubeCenter.z + halfSize);
            vertices[2] = new Vector3(cubeCenter.x - halfSize, cubeCenter.y + halfSize, cubeCenter.z + halfSize);
            vertices[3] = new Vector3(cubeCenter.x - halfSize, cubeCenter.y + halfSize, cubeCenter.z - halfSize);
        }
        else if (Mathf.Abs(faceCenter.z - (cubeCenter.z + halfSize)) < 0.01f) // 前面
        {
            vertices[0] = new Vector3(cubeCenter.x - halfSize, cubeCenter.y - halfSize, cubeCenter.z + halfSize);
            vertices[1] = new Vector3(cubeCenter.x + halfSize, cubeCenter.y - halfSize, cubeCenter.z + halfSize);
            vertices[2] = new Vector3(cubeCenter.x + halfSize, cubeCenter.y + halfSize, cubeCenter.z + halfSize);
            vertices[3] = new Vector3(cubeCenter.x - halfSize, cubeCenter.y + halfSize, cubeCenter.z + halfSize);
        }
        else if (Mathf.Abs(faceCenter.z - (cubeCenter.z - halfSize)) < 0.01f) // 后面
        {
            vertices[0] = new Vector3(cubeCenter.x - halfSize, cubeCenter.y - halfSize, cubeCenter.z - halfSize);
            vertices[1] = new Vector3(cubeCenter.x + halfSize, cubeCenter.y - halfSize, cubeCenter.z - halfSize);
            vertices[2] = new Vector3(cubeCenter.x + halfSize, cubeCenter.y + halfSize, cubeCenter.z - halfSize);
            vertices[3] = new Vector3(cubeCenter.x - halfSize, cubeCenter.y + halfSize, cubeCenter.z - halfSize);
        }

        return vertices;
    }
}
