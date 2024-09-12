using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : Singleton<PathFinder>
{
    [SerializeField]
    CenterPoint sta;
    [SerializeField]
    CenterPoint des;

    private void Update()
    {
        SearchPath();
    }
    public void SetStart(CenterPoint cp)
    {
        sta = cp;
    }
    public void SetDestination(CenterPoint cp)
    {
        if(Player.Instance.tarCenter != null && Player.Instance.tarCenter != cp)
            Player.Instance.SetCenter(Player.Instance.tarCenter);
        des = cp;
    }
    void SearchPath()
    {
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
        while (visiting.Count > 0)
        {
            CenterPoint current = visiting[0];
            visiting.RemoveAt(0);
            if (current == des)
            {
                DrawPath();
                return;
            }
            foreach (var next in current.nextPoints)
            {
                if (next.visited)
                    continue;
                next.lastPointInPath = current;
                visiting.Add(next);
                next.visited = true;
            }
        }
        des = null;

    }
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
