using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTrigger : MonoBehaviour, IAttached
{
    #region Trigger
    public List<Cube> effectCubes = new();
    public virtual void DoTrigger()
    {
        Debug.Log(name + "触发");
    }
    #endregion

    #region interface
    [SerializeField]
    CenterPoint curCenter;
    public CenterPoint GetCurCenter()
    {
        return curCenter;
    }
    public void SetCurCenter(CenterPoint centerPoint)
    {
        curCenter.myTriggers.Remove(this);
        curCenter = centerPoint;
        curCenter.myTriggers.Add(this);
    }

    public void ArriveTarCenter(CenterPoint center)
    {
        if (curCenter != null)
        {
            curCenter.myTriggers.Remove(this);
            curCenter = center;
            curCenter.myTriggers.Add(this);
        }
        else
        {
            curCenter = center;
            curCenter.myTriggers.Add(this);
            gameObject.SetActive(true);
            transform.parent = center.cube.attached;
            transform.localPosition = center.transform.localPosition * 0.85f;
        }

    }
    #endregion
}
