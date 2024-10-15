using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player> , IAttached
{
    public CenterPoint tarCenter;
    public CenterPoint curCenter;
    #region state
    enum STATE
    {
        IDLE,
        MOVING,
        ARRIVED,
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
    void Update()
    {
        if (UIManager.Instance.IsUIBusy)
            return;
        switch (state)
        {
            case STATE.IDLE:
                TryIdle();
                break;
            case STATE.MOVING:
                TryMoving();
                break;
            case STATE.ARRIVED:
                TryArrived();
                break;
            default:
                break;
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
    public void Initialize()
    {
        ArriveTarCenter(LevelManager.Instance.curLevel.staCenter);
    }
    public void ArriveTarCenter(CenterPoint nextTar)
    {
        if (curCenter == null || tarCenter == null)//关卡初始化时的null
        {
            tarCenter = nextTar;
            transform.position = LevelManager.Instance.curLevel.staCenter.CenterToPlayerPos();
        }
        ChangeState(STATE.ARRIVED);
        TryArrived();
    }
    
    void TryIdle()
    {
        if (tarCenter.nextPointInPath != null)
        {
            ChangeState(STATE.MOVING);
        }
    }
    void TryMoving()
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
            transform.position, tarCenter.CenterToPlayerPos(),
            Vector3.Magnitude(tarCenter.CenterToPlayerPos() - curCenter.CenterToPlayerPos()) * Config.Instance.moveSpeed * Time.deltaTime
            );
        transform.Rotate(Vector3.Cross(transform.localPosition, tarCenter.CenterToPlayerPos()) * Config.Instance.ballRotateSpeed);
        //移动时修改渲染顺序,render queue
        GetComponent<MeshRenderer>().material.renderQueue = 3100;
    }

    void TryArrived()
    {
        if (!Application.isPlaying)
            return;
        transform.parent = tarCenter.cube.attached;
        if (curCenter != tarCenter)
            tarCenter.OnPlayerEnter();
        curCenter = tarCenter;
        if(tarCenter.nextPointInPath == null)
        {
            GetComponent<MeshRenderer>().material.renderQueue = 3000;
            ChangeState(STATE.IDLE);
            return;
        }
        tarCenter = tarCenter.nextPointInPath;
        
        ChangeState(STATE.MOVING);
    }
    bool IsNearTarCenter()
    {
        return Vector3.Distance(transform.position, tarCenter.CenterToPlayerPos()) < 0.001f;
    }
}
