using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : Singleton<PathFinder>
{
    [SerializeField]
    CenterPoint sta;
    [SerializeField]
    CenterPoint des;
    [SerializeField]
    public EndSphere endSphere;
    private void Awake()
    {
        endSphere.InitTransform(LevelManager.Instance.curLevel.desCenter);
    }
    public void SetDestinations(CenterPoint[] cps)
    {
        foreach (var cp in cps)
        {
            des = cp;
            if (SearchPath())
            {
                //if(sta != des)
                UIManager.Instance.ShowSth("pathSucceedCircle", Input.mousePosition);
                return;
            }    
        }
        UIManager.Instance.ShowSth("pathFailCross",Input.mousePosition);
    }
    bool SearchPath()
    {
        sta = Player.Instance.tarCenter;
        if (des == null)
            return false;
        foreach (var cp in CubeCombiner.Instance.centerPoints[sta.axisId])
        {
            cp.visited = false;
            cp.lastPointInPath = null; 
            cp.nextPointInPath = null;
        }
        List<CenterPoint> visiting = new(){sta};
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
                if (next.visited)
                    continue;
                next.lastPointInPath = current;
                visiting.Add(next);
                next.visited = true;
            }
        }
        return false;
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
