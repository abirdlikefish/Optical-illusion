using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTrigger : Busy, IAttached
{
    private void Awake()
    {
        if (!curCenter.myTriggers.Contains(this))
        {
            Debug.Log("发现多余触发器！请销毁");
        }
    }

    private void OnDisable()
    {
        curCenter.myTriggers.Remove(this);
    }
    #region Trigger
    public List<Cube> effectCubes = new();
    [Header("初始是否激活")][SerializeField]
    protected bool triggered = false;
    public virtual void DoTrigger()
    {
        triggered = !triggered;
    }
    #endregion

    [SerializeField]
    CenterPoint curCenter;
    #region interface
    public CenterPoint GetCurCenter()
    {
        return curCenter;
    }

    public void SetCenter(CenterPoint center)
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
            transform.parent = center.cube.transform.Find("Attached");
            transform.localPosition = center.transform.localPosition * 0.85f;
        }

    }
    #endregion
    float If_0_Then_90(float x)
    {
        return x == 0 ? 90 : 0;
    }
}
