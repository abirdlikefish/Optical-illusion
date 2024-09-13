using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPoint : MonoBehaviour
{
    public Vector3Int delta;
    [HideInInspector]
    public Cube cube;
    [HideInInspector]
    public string info;
    [SerializeField]
    List<CenterPoint> overlapPoints = new();
    public List<CenterPoint> nextPoints = new();
    public bool visited;
    public CenterPoint lastPointInPath;
    public CenterPoint nextPointInPath;
    public bool IsVisible => cube.GetVisibleCenterPoint().Contains(this);
    private void Awake()
    {
        cube = transform.parent.GetComponent<Cube>();
        info = cube.name + "::" + name;
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
        //Debug.Log(info + " is near to " + thatPoint.info);
        foreach(var centerPoint in cube.centerPoints)
        {
            if (Vector3.Dot(delta, centerPoint.delta) != 0)
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
            if (Vector3.Dot(delta, centerPoint.delta) != 0)
                continue;
            centerPoint.ClearNextPoint(thatPoint.cube.GetSameDeltaCenterPoint(centerPoint));
        }
    }
    public void AddNextPoint(CenterPoint thatPoint)
    {
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
}
