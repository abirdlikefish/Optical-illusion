using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player> , IAttached
{
    public CenterPoint tarCenter;
    public CenterPoint curCenter;
    public bool hasStoppedWhenTrigger = false;
    #region state
    enum STATE
    {
        IDLE,
        MOVING,
        STOPPEDWHENTRIGGER,
    }
    [SerializeField]
    STATE state;
    public bool IsBusy()
    {
        return state != STATE.IDLE;
    }
    void ChangeState(STATE newState)
    {
        state = newState;
    }
    #endregion
    void Start()
    {
        Init();
    }

    void Update()
    {
        if (UIManager.Instance.IsUIBusy)
            return;
        switch (state)
        {
            case STATE.IDLE:
                TryMoveToDes();
                break;
            case STATE.MOVING:
                MoveToDes();
                break;
            default:break;
        }
        
        
    }
    
    public CenterPoint GetCurCenter()
    {
        return curCenter;
    }
    public void SetCurCenter(CenterPoint centerPoint)
    {
        tarCenter = curCenter = centerPoint;
    }
    void Init()
    {
        transform.position = LevelManager.Instance.curLevel.staCenter.CenterToWorldPos(gameObject);
        ArriveTarCenter(LevelManager.Instance.curLevel.staCenter);
        ChangeState(STATE.MOVING);
    }
    public void ArriveTarCenter(CenterPoint center)
    {
        if (center == null)
        {
            if(tarCenter != curCenter)
                tarCenter.OnPlayerEnter();
            curCenter = tarCenter;
            GetComponent<MeshRenderer>().material.renderQueue = 3000;
            ChangeState(STATE.IDLE);
            return;
        }
        if (tarCenter != null && tarCenter != curCenter)
        {
            if (tarCenter.myTriggers.Count != 0 && !hasStoppedWhenTrigger)
            {
                hasStoppedWhenTrigger = true;
                tarCenter.OnPlayerEnter();
                return;
            }
            hasStoppedWhenTrigger = false;
        }
        curCenter = tarCenter;
        tarCenter = center;
        transform.parent = tarCenter.cube.attached;
    }
    
    void TryMoveToDes()
    {
        if (tarCenter.nextPointInPath != null)
        {
            ChangeState(STATE.MOVING);
        }
        
    }
    void MoveToDes()
    {
        if (MyTriggerManager.Instance.busyRotates.Count != 0)
            return;
        if (IsNearTarCenter())
        {
            if (!tarCenter.nextPoints.Contains(tarCenter.nextPointInPath))
                tarCenter.nextPointInPath = null;
            ArriveTarCenter(tarCenter.nextPointInPath);
            return;
        }
        transform.position = Vector3.MoveTowards
            (
            transform.position, tarCenter.CenterToWorldPos(gameObject),
            Vector3.Magnitude(tarCenter.transform.position - curCenter.transform.position) * Config.Instance.moveSpeed * Time.deltaTime
            );
        transform.Rotate(Vector3.Cross(transform.localPosition, tarCenter.CenterToWorldPos(gameObject)) * Config.Instance.ballRotateSpeed);
        //移动时修改渲染顺序,render queue
        GetComponent<MeshRenderer>().material.renderQueue = 3100;
    }
    bool IsNearTarCenter()
    {
        return Vector3.Distance(transform.position, tarCenter.CenterToWorldPos(gameObject)) < 0.001f;
    }
}
