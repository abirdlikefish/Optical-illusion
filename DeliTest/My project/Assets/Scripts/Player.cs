using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player> , IAttached
{
    public CenterPoint curCenter;
    public CenterPoint lastCenter;
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
        if (UIManager.Instance.IsUIBusy)
            return;
        MoveToDes();
    }
    public CenterPoint GetCurCenter()
    {
        return curCenter;
    }
    void Init()
    {
        SetCenter(LevelManager.Instance.curLevel.staCenter);
        transform.position = LevelManager.Instance.curLevel.staCenter.CenterToWorldPos(gameObject);
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
    
    void MoveToDes()
    {
        if (IsNearCurCenter())
        {
            if (lastCenter != curCenter)
                curCenter.OnPlayerEnter();
            if (!curCenter.nextPoints.Contains(curCenter.nextPointInPath))
                curCenter.nextPointInPath = null;
            
            SetCenter(curCenter.nextPointInPath);
            return;
        }
        state = STATE.MOVING;
        transform.position = Vector3.MoveTowards
            (
            transform.position, curCenter.CenterToWorldPos(gameObject),
            Vector3.Magnitude(curCenter.transform.position - lastCenter.transform.position) * Config.Instance.moveSpeed * Time.deltaTime
            );
        transform.Rotate(Vector3.Cross(transform.localPosition, curCenter.CenterToWorldPos(gameObject)) * Config.Instance.ballRotateSpeed);
        //移动时修改渲染顺序,render queue
        GetComponent<MeshRenderer>().material.renderQueue = 3100;

    }

    bool IsNearCurCenter()
    {
        return Vector3.Distance(transform.position, curCenter.CenterToWorldPos(gameObject)) < 0.001f;
    }
}
