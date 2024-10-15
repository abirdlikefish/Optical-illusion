using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeCombiner : Singleton<CubeCombiner>
{
    [SerializeField]
    Transform cubeP;
    public List<Cube> cubes = new();

    public List<CenterPoint>[] centerPoints;
    public List<CenterPointPair> centerPointPairs = new();

    void Update()
    {
        centerPointPairs.Clear();
        for (int i = 0; i < 3; i++)
        {
            FindNearCenterOnAxisI(i);
        }
        foreach (var cube in cubes)
            cube.Change6SideColor();
    }
    public void CollectCubeAndCenter()
    {
        cubes.Clear();
        for (int i = 0; i < cubeP.childCount; i++)
        {
            if (cubeP.GetChild(i).gameObject.activeSelf)
                cubes.Add(cubeP.GetChild(i).GetComponent<Cube>());
        }
        centerPoints = new List<CenterPoint>[3];
        for (int i = 0; i < 3; i++)
        {
            centerPoints[i] = new List<CenterPoint>();
        }
        foreach (var cube in cubes)
        {
            foreach (var centerPoint in cube.centerPoints)
            {
                centerPoints[centerPoint.axisId].Add(centerPoint);
            }
        }
    }
    void FindNearCenterOnAxisI(int axisId)
    {
        for (int i = 0; i < centerPoints[axisId].Count; i++)
        {
            for (int j = i + 1; j < centerPoints[axisId].Count; j++)
            {
                CenterPoint p1 = centerPoints[axisId][i];
                CenterPoint p2 = centerPoints[axisId][j];
                if (p1.transform.position.z >= p2.transform.position.z + 0.2f)
                    (p1,p2) = (p2, p1);
                if (IsCubeSame(p1, p2))
                    continue;
                if (!IsCenterSameAxis(p1, p2))
                    continue;
                if (IsCubeNear(p1, p2))
                {
                    if (CenterIsAtTwoCubeNaka(p1,p2))
                    {
                        centerPointPairs.Add(new(p1, p2,canMagnet:false));
                        p1.AddOverlapPoint(p2);
                        if(!IsCubeSameColor(p1, p2))
                        {
                            p1.cube.nearCubes.Add(p2.cube);
                            p2.cube.nearCubes.Add(p1.cube);
                        }
                    }
                    continue;
                }
                p1.cube.nearCubes.Remove(p2.cube);
                p2.cube.nearCubes.Remove(p1.cube);

                if (IsCubeSameColor(p1, p2) &&
                !IsCubeSameAxis(p1.cube, p2.cube) &&
                p1.IsNotVisible &&
                p2.IsVisible &&
                p1.transform.position.z < p2.transform.position.z + 0.2f &&
                IsNearInCamera(p1.gameObject, p2.gameObject) &&
                !IsNearInCamera(p1.cube.gameObject, p2.cube.gameObject)
                )
                {
                    centerPointPairs.Add(new(p1, p2,canMagnet: true));
                    p1.AddOverlapPoint(p2);
                    continue;
                }
                p1.ClearOverlapPoint(p2);
            }
        }
    }
    bool IsCubeSame(CenterPoint p1,CenterPoint p2)
    {
        return p1.cube == p2.cube;
    }
    bool CenterIsAtTwoCubeNaka(CenterPoint p1,CenterPoint p2)
    {
        return Vector3.Magnitude(p1.cube.transform.localPosition + p1.transform.localPosition - p2.cube.transform.localPosition - p2.transform.localPosition) <= 0.01f;
    }
    bool IsCubeSameColor(CenterPoint p1,CenterPoint p2)
    {
        return p1.cube.color == p2.cube.color && p1.cube.color != Cube.COLOR.BLACK;
    }
    bool IsNearInCamera(GameObject obj1, GameObject obj2)
    {
        Vector3 screenPos1 = Camera.main.WorldToScreenPoint(obj1.transform.position);
        Vector3 screenPos2 = Camera.main.WorldToScreenPoint(obj2.transform.position);
        float distance = Vector3.Distance(screenPos1, screenPos2);
        return distance <= Config.Instance.centerNearDistance;
    }
    bool IsCubeNear(CenterPoint p1, CenterPoint p2)
    {
        return MathF.Abs(Vector3.Distance(p1.cube.transform.localPosition,p2.cube.transform.localPosition) - 1) <= Config.Instance.cubeNearDistance;
    }

    bool IsCubeSameAxis(Cube c1, Cube c2)
    {
        Vector3 deltaCube = c1.transform.localPosition - c2.transform.localPosition;
        int zeroCount = 0;
        for (int i = 0; i < 3; i++)
        {
            if (Mathf.Abs(deltaCube[i]) <= 0.01f)
                zeroCount++;
        }
        return zeroCount == 2;
    }
    public bool IsCenterSameAxis(CenterPoint p1, CenterPoint p2)
    {
        return Vector3.Dot(p1.transform.localPosition, p2.transform.localPosition) != 0;
    }

}
[Serializable]
public class CenterPointPair
{
    public CenterPoint first;
    public CenterPoint second;
    public bool canMagnet;
    public CenterPointPair(CenterPoint first, CenterPoint second, bool canMagnet)
    {
        this.first = first;
        this.second = second;
        this.canMagnet = canMagnet;
    }
}