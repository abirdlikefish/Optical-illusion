using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public CenterPoint[] centerPoints = new CenterPoint[6];
    public CenterPoint GetSameDeltaCenterPoint(CenterPoint thatPoint)
    {
        foreach(var centerPoint in centerPoints)
        {
            if(centerPoint.delta == thatPoint.delta)
                return centerPoint;
        }
        return null;
    }
    //被点击时寻找最近的可视的中心点
    public void OnMouseDown()
    {
        //PathFinder.Instance.lastClickedCube?.HideAllCenterPoints();
        //PathFinder.Instance.lastClickedCube = this;
        CenterPoint nearest = GetNearestVisibleCenterPoint(Input.mousePosition);
        PathFinder.Instance.SetDestination(nearest);

        //nearest.GetComponent<MeshRenderer>().enabled = true;
        //PathFinder.Instance.SetStart(nearest);
    }
    void HideAllCenterPoints()
    {
        foreach (var centerPoint in centerPoints)
        {
            centerPoint.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    public List<CenterPoint> GetVisibleCenterPoint()
    {
        List<CenterPoint> ret = new();
        foreach (var it in centerPoints)
        {
            bool foundSameAxis = false;
            foreach (var it2 in ret)
            {
                if (Vector3.Dot(it.delta, it2.delta) != 0)
                {
                    foundSameAxis = true;
                    if (it.transform.position.z < it2.transform.position.z)
                    {
                        ret.Add(it);
                        ret.Remove(it2);
                        break;
                    }
                }
            }
            if (foundSameAxis)
                continue;
            ret.Add(it);
        }
        return ret;
    }
    public CenterPoint GetNearestVisibleCenterPoint(Vector2 pos)
    {
        List<CenterPoint> visibleCenterPoints = GetVisibleCenterPoint();
        CenterPoint nearest = null;
        float minDis = float.MaxValue;
        foreach (var centerPoint in visibleCenterPoints)
        {
            float dis = Vector2.Distance(Camera.main.WorldToScreenPoint(centerPoint.transform.position), pos);
            if (dis < minDis)
            {
                minDis = dis;
                nearest = centerPoint;
            }
        }
        return nearest;
    }
}
