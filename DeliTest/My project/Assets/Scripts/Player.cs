using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public CenterPoint tarCenter;
    public CenterPoint curCenter;
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
    // Start is called before the first frame update
    void Start()
    {
        state = STATE.IDLE;
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToDes();
    }
    void Init()
    {
        transform.parent = initCenter.transform;
        transform.localPosition = CenterToLocalPos(initCenter);
        SetCenter(initCenter);
    }
    public void SetCenter(CenterPoint center)
    {
        curCenter = center;
        tarCenter = curCenter.nextPointInPath;
        
        PathFinder.Instance.SetStart(curCenter);
    }
    Vector3 CenterToLocalPos(CenterPoint center)
    {
        return Vector3.Normalize(center.delta) * transform.localScale.x / 2;
    }
    void MoveToDes()
    {
        tarCenter = curCenter.nextPointInPath;
        if (tarCenter != null)
        {
            state = STATE.MOVING;
            transform.parent = curCenter.nextPointInPath.transform;
            //匀速移动到下一站
            transform.localPosition = Vector3.MoveTowards
                (
                transform.localPosition, CenterToLocalPos(tarCenter),
                Vector3.Magnitude(tarCenter.transform.position - curCenter.transform.position) * moveSpeed
                );
            transform.Rotate(Vector3.Cross(transform.localPosition, CenterToLocalPos(tarCenter)) * rotateSpeed);



            if (IsNearNextCenter())
                SetCenter(tarCenter);
        }
        else
        {
            state = STATE.IDLE;
        }

    }
    bool IsNearNextCenter()
    {
        return Vector3.Distance(transform.localPosition, CenterToLocalPos(tarCenter)) < 0.001f;
    }
}
