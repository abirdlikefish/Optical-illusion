using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player> , IAttached
{
    //public CenterPoint tarCenter;
    //public CenterPoint nextTarCenter;
    public CenterPoint curCenter;
    public CenterPoint lastCenter;
    public CenterPoint initCenter;
    
    enum STATE
    {
        IDLE,
        MOVING
    }
    STATE state;
    public bool IsBusy()
    {
        return state == STATE.MOVING;
    }
    void Start()
    {
        state = STATE.IDLE;
        Init();
        
    }

    void Update()
    {
        MoveToDes();
    }
    public CenterPoint GetCurCenter()
    {
        return curCenter;
    }
    void Init()
    {
        SetCenter(initCenter);
        transform.position = CenterToWorldPos(initCenter);
    }
    public void SetCenter(CenterPoint center)
    {
        if (center == null)
        {
            state = STATE.IDLE;
            lastCenter = curCenter;
            GetComponent<MeshRenderer>().material.renderQueue = 3000;
            return;
        }
        lastCenter = curCenter;
        curCenter = center;
        transform.parent = curCenter.transform;
        transform.parent = curCenter.cube.transform.Find("Attached");

    }
    Vector3 CenterToWorldPos(CenterPoint center)
    {
        return center.transform.position + (center.transform.position - center.cube.transform.position) * transform.lossyScale.x;
    }
    void MoveToDes()
    {
        if (IsNearCurCenter())
        {
            SetCenter(curCenter.nextPointInPath);
            return;
        }
        state = STATE.MOVING;
        transform.position = Vector3.MoveTowards
            (
            transform.position, CenterToWorldPos(curCenter),
            Vector3.Magnitude(curCenter.transform.position - lastCenter.transform.position) * Config.Instance.moveSpeed
            );
        transform.Rotate(Vector3.Cross(transform.localPosition, CenterToWorldPos(curCenter)) * Config.Instance.rotateSpeed);
        //移动时修改渲染顺序,render queue
        GetComponent<MeshRenderer>().material.renderQueue = 3100;

    }

    bool IsNearCurCenter()
    {
        return Vector3.Distance(transform.position, CenterToWorldPos(curCenter)) < 0.001f;
    }
}
