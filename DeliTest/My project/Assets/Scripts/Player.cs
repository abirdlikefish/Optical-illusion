using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    //public CenterPoint tarCenter;
    //public CenterPoint nextTarCenter;
    public CenterPoint curCenter;
    public CenterPoint lastCenter;
    public CenterPoint initCenter;
    public float moveSpeed = 0.1f;
    public float rotateSpeed = 1f;
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
    void Init()
    {
        SetCenter(initCenter);
        transform.localPosition = CenterToLocalPos(initCenter);
    }
    public void SetCenter(CenterPoint center)
    {
        if (center == null)
        {
            state = STATE.IDLE;
            lastCenter = curCenter;
            return;
        }
        lastCenter = curCenter;
        curCenter = center;
        transform.parent = curCenter.transform;
        

    }
    Vector3 CenterToLocalPos(CenterPoint center)
    {
        //return Vector3.Normalize(center.delta) * transform.localScale.x / 2;
        return center.transform.localPosition * transform.localScale.x;
    }
    void MoveToDes()
    {
        if (IsNearCurCenter())
        {
            SetCenter(curCenter.nextPointInPath);
            return;
        }
        state = STATE.MOVING;
        transform.localPosition = Vector3.MoveTowards
            (
            transform.localPosition, CenterToLocalPos(curCenter),
            Vector3.Magnitude(curCenter.transform.position - lastCenter.transform.position) * moveSpeed
            );
        transform.Rotate(Vector3.Cross(transform.localPosition, CenterToLocalPos(curCenter)) * rotateSpeed);
        
    }

    bool IsNearCurCenter()
    {
        return Vector3.Distance(transform.localPosition, CenterToLocalPos(curCenter)) < 0.001f;
    }
}
