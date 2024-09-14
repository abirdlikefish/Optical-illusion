using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CubeCombiner : Singleton<CubeCombiner>
{
    [SerializeField]
    float nearDistance = 2f;
    public static float nearTime = 0.2f;
    [SerializeField]
    Transform cubeP;
    [SerializeField]
    List<Cube> cubes = new();

    public List<CenterPoint> centerPoints = new();
    public List<CenterPointPair> centerPointPairs = new();
    private void Awake()
    {
        CollectCubeAndCenter();
    }
    void Update()
    {
        FindNearCenter();
    }
    private void OnTransformChildrenChanged()
    {
        CollectCubeAndCenter();
    }
    void CollectCubeAndCenter()
    {
        cubes.Clear();
        for (int i = 0; i < cubeP.childCount; i++)
        {
            if (cubeP.GetChild(i).gameObject.activeSelf)
                cubes.Add(cubeP.GetChild(i).GetComponent<Cube>());
        }
        centerPoints.Clear();
        foreach (var cube in cubes)
        {
            foreach (var centerPoint in cube.centerPoints)
            {
                centerPoints.Add(centerPoint);
            }
        }
    }

    void FindNearCenter()
    {
        for (int i = 0; i < centerPoints.Count; i++)
        {
            for (int j = 0; j < centerPoints.Count; j++)
            {
                if (i == j)
                    continue;
                CenterPoint p1 = centerPoints[i];
                CenterPoint p2 = centerPoints[j];
                if (IsCubeSameColor(p1, p2) &&
                    IsCenterSameAxis(p1, p2) &&
                    !IsCubeSameAxis(p1.cube, p2.cube) &&
                    centerPoints[i].IsNotVisible &&
                    centerPoints[j].IsVisible &&
                    centerPoints[i].transform.position.z < centerPoints[j].transform.position.z + 0.2f &&
                    !IsNearInCamera(p1.cube.gameObject,p2.cube.gameObject) &&
                    IsNearInCamera(p1.gameObject, p2.gameObject)
                    )
                {
                    IsCubeSameAxis(p1.cube, p2.cube);
                    if (!centerPointPairs.Exists(x => x.first == p1 && x.second == p2))
                        centerPointPairs.Add(new(p1, p2));
                    p1.AddOverlapPoint(p2);
                }
                else if (
                    IsCubeNear(p1, p2) &&
                    !IsNearInCamera(p1.cube.gameObject, p2.cube.gameObject) &&
                    IsNearInCamera(p1.gameObject, p2.gameObject))
                {
                    p1.cube.ChangeColor(p2.cube);
                    p2.cube.ChangeColor(p1.cube);
                    p1.AddOverlapPoint(p2);
                }
                else
                {
                    centerPointPairs.RemoveAll(x => x.first == p1 && x.second == p2);
                    p1.ClearOverlapPoint(p2);
                }
            }
        }
    }
    bool IsCubeSameColor(CenterPoint p1,CenterPoint p2)
    {
        return p1.cube.color == p2.cube.color;
    }
    bool IsNearInCamera(GameObject obj1, GameObject obj2)
    {
        Vector3 screenPos1 = Camera.main.WorldToScreenPoint(obj1.transform.position);
        Vector3 screenPos2 = Camera.main.WorldToScreenPoint(obj2.transform.position);
        float distance = Vector3.Distance(screenPos1, screenPos2);
        return distance <= nearDistance;
    }
    public bool IsCubeNear(CenterPoint p1, CenterPoint p2)
    {
        return Vector3.Magnitude(p1.cube.transform.localPosition - p2.cube.transform.localPosition) == 1;
    }

    bool IsCubeSameAxis(Cube c1, Cube c2)
    {
        return Vector3.Dot(c1.transform.localPosition,c2.transform.localPosition) != 0;
    }
    bool IsCenterSameAxis(CenterPoint p1, CenterPoint p2)
    {
        return Vector3.Dot(p1.delta, p2.delta) != 0;
    }
    bool IsCenterInSamePlane(CenterPoint p1, CenterPoint p2)
    {
        return Vector3.Dot(Rotater.Instance.GetInitDelta(p1, p2), p1.delta - p2.delta) == 0;
    }

    

}
[Serializable]
public class CenterPointPair
{
    public CenterPoint first;
    public CenterPoint second;

    public CenterPointPair(CenterPoint first, CenterPoint second)
    {
        this.first = first;
        this.second = second;
    }
}