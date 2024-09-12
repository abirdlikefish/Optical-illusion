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
    private void Awake()
    {
        CollectCubeAndCenter();
    }

    // Update is called once per frame
    void Update()
    {
        FindNearCenter();
    }
    //子物体数量发生变化时
    private void OnTransformChildrenChanged()
    {
        CollectCubeAndCenter();
    }
    void CollectCubeAndCenter()
    {
        cubes.Clear();
        for (int i = 0; i < cubeP.childCount; i++)
        {
            if(cubeP.GetChild(i).gameObject.activeSelf)
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
                //if (IsSameCube(p1, p2) || IsCubeNear(p1, p2) || IsCubeSameAxis(p1,p2) || !IsCenterSameAxis(p1, p2) || !IsBetweenCube(p1, p2))
                //{
                //    continue;
                //}
                if (IsCenterSameAxis(p1, p2) &&
                    centerPoints[i].transform.position.z < centerPoints[j].transform.position.z &&
                    !centerPoints[i].isVisible &&
                    centerPoints[j].isVisible &&
                    IsNearInCamera(p1.gameObject, p2.gameObject) ||
                    IsCubeNear(p1, p2)
                    )
                {
                    p1.AddOverlapPoint(p2);
                }
                else
                {
                    p1.ClearOverlapPoint(p2);
                }
                
            }
        }
    }
    bool IsNearInCamera(GameObject obj1, GameObject obj2)
    {
        Vector3 screenPos1 = Camera.main.WorldToScreenPoint(obj1.transform.position);
        Vector3 screenPos2 = Camera.main.WorldToScreenPoint(obj2.transform.position);
        float distance = Vector3.Distance(screenPos1, screenPos2);
        //Debug.Log("Distance: " + distance);
        return distance <= nearDistance;
    }
    //bool IsSameCube(CenterPoint p1, CenterPoint p2)
    //{
    //    return p1.cube == p2.cube;
    //}
    bool IsCubeNear(CenterPoint p1, CenterPoint p2)
    {
        return Vector3.Magnitude(p1.cube.transform.localPosition - p2.cube.transform.localPosition) == 1;
    }
    ////判断正方体的连线与轴平行
    //bool IsCubeSameAxis(CenterPoint p1, CenterPoint p2)
    //{
    //    Vector3 deltaCube = p1.cube.transform.localPosition - p2.cube.transform.localPosition;
    //    int zeroCount = 0;
    //    zeroCount += deltaCube.x == 0 ? 1 : 0;
    //    zeroCount += deltaCube.y == 0 ? 1 : 0;
    //    zeroCount += deltaCube.z == 0 ? 1 : 0;
    //    return zeroCount == 2;
    //}

    bool IsCenterSameAxis(CenterPoint p1, CenterPoint p2)
    {
        return Vector3.Dot(p1.delta, p2.delta) != 0;
    }
    //bool IsBetweenCube(CenterPoint p1, CenterPoint p2)
    //{
    //    Vector3 deltaPoint = p1.delta - p2.delta;
    //    Vector3 deltaCube = p1.cube.transform.localPosition - p2.cube.transform.localPosition;
    //    return Vector3.Dot(deltaPoint, deltaCube) < 0;
    //}
}