using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPoint : MonoBehaviour
{
    [HideInInspector]
    public Cube cube;
    [SerializeField]
    List<CenterPoint> overlapPoints = new();
    public List<CenterPoint> nextPoints = new();
    public bool visited;
    public CenterPoint lastPointInPath;
    public CenterPoint nextPointInPath;
    //public List<Vector3> obstacledPoints = new();

    public List<MyTrigger> myTriggers = new();
    public void OnPlayerEnter()
    {
        foreach (var myTrigger in myTriggers)
        {
            myTrigger.DoTrigger();
        }
        if (LevelManager.Instance.curLevel.endCenter == this)
            LevelManager.Instance.PassCurLevel();
    }
    public bool IsVisible => transform.position.z <= cube.transform.position.z + 0.05f;
    public bool IsNotVisible => transform.position.z >= cube.transform.position.z - 0.05f;
    //public bool IsObstacled => obstacledPoints.Count != 0;
    public bool NoNext => nextPoints.Count == 0;
    public void Awake()
    {
        cube = transform.parent.GetComponent<Cube>();
        name = cube.name + "::" + name;
        if (myTriggers.Count != 0)
            GetComponent<MeshRenderer>().enabled = true;
    }
    private void Update()
    {
        foreach(var np in nextPoints)
        {
            if(Input.GetKey(KeyCode.Alpha3))
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
        foreach(var centerPoint in cube.centerPoints)
        {
            if(CubeCombiner.Instance.IsCenterSameAxis(this,centerPoint))
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
            //if (CubeCombiner.Instance.IsCenterSameAxis(this, centerPoint))
            //    continue;
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
        bool ret = false;
        Vector3[] vertices = GetFaceVertices(centerPoint.cube.transform.localPosition,1,centerPoint.transform.localPosition);
        Vector3[] init_vertices = new Vector3[4];
        for (int i=0;i< 4; i++)
        {
            vertices[i] -= centerPoint.cube.transform.localPosition;
            init_vertices[i] = centerPoint.cube.transform.TransformPoint(vertices[i]);
            vertices[i] = Vector3.Lerp(centerPoint.transform.localPosition, vertices[i], Config.Instance.CenterObstacleMultiplier);
            vertices[i] = centerPoint.cube.transform.TransformPoint(vertices[i]);
        }
        for (int i = 0; i < 4; i++)
        {
            RaycastHit[] raycastHits = Physics.RaycastAll(vertices[i] + Vector3.forward * 0.05f, -Vector3.forward,1000f);
            
            foreach (var raycastHit in raycastHits)
            {
                if (raycastHit.collider.transform == centerPoint.cube.transform)
                    continue;
                ret = true;
                //return true;
            }
            if (centerPoint.name == "BLUE3::-y")
            {
                //将未遮挡顶点的位置画出来
                Debug.DrawLine(init_vertices[i], vertices[i], Color.red);
                Debug.Log(i + " th :" + raycastHits.Length);
                
            }
        }
        return ret;
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
