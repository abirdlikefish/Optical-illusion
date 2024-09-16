using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : Singleton<PathFinder>
{
    [SerializeField]
    CenterPoint sta;
    [SerializeField]
    CenterPoint des;
    public GameObject obstacledInfo;
    public void SetDestinations(CenterPoint[] cps)
    {
        foreach (var cp in cps)
        {
            des = cp;
            if (SearchPath())
                return;
        }
        //ShowObInfo();
    }
    bool SearchPath()
    {
        sta = Player.Instance.curCenter;
        if (des == null)
            return false;
        if (sta.IsNotVisible || sta.IsObstacled)
            return false;
        foreach (var cp in CubeCombiner.Instance.centerPoints)
        {
            cp.visited = false;
            cp.lastPointInPath = null; 
            cp.nextPointInPath = null;
        }
        List<CenterPoint> visiting = new();
        visiting.Add(sta);
        sta.visited = true;
        sta.lastPointInPath = null;
        CenterPoint current = null;
        while (visiting.Count > 0)
        {
            current = visiting[0];
            visiting.RemoveAt(0);
            if (current == des)
            {
                DrawPath();
                return true;
            }
            foreach (var next in current.nextPoints)
            {
                if (next.visited || next.IsObstacled)
                    continue;
                next.lastPointInPath = current;
                visiting.Add(next);
                next.visited = true;
            }
        }
        return false;
    }
    //void ShowObInfo()
    //{
    //    foreach (var ob in temp_obstaclePoints)
    //    {
    //        GameObject g = Instantiate(obstacledInfo);
    //        g.transform.parent = lastInQueue.transform.parent;
    //        g.transform.position = ob;
    //        g.transform.localScale = new Vector3(1, 1, 1) * Config.Instance.CenterObstacleScale;
    //        g.SetActive(true);
    //    }
    //    Debug.Log("NPF from " + lastInQueue.name);
    //}
    void DrawPath()
    {
        CenterPoint currentInPath = des;
        while (currentInPath != sta)
        {
            Debug.DrawLine(currentInPath.transform.position, currentInPath.lastPointInPath.transform.position, Color.green);
            currentInPath.lastPointInPath.nextPointInPath = currentInPath;
            currentInPath = currentInPath.lastPointInPath;
        }
    }
}
