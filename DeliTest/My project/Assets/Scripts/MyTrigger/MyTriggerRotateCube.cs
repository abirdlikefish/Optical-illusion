using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MyTriggerRotateCube : MyTrigger
{
    [SerializeField]
    //每次点击旋转Delta角度
    Vector3 delta;
    [SerializeField]
    // 旋转速度，每秒旋转的角度
    float rotationSpeed = 90f;

    List<Quaternion> targets = new();
    private void Start()
    {
        foreach (var effectCube in effectCubes)
            targets.Add(effectCube.transform.localRotation);
    }
    public override void DoTrigger()
    {
        triggered = true;
    }

    void Update()
    {
        if (UIManager.Instance.IsUIBusy)
            return;
        if (triggered)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                //根据delta算出目标角度
                targets[i] *= Quaternion.Euler(delta);
            }
            triggered = false;
        }
        MyTriggerManager.Instance.busyRotates.Add(this);
        bool busy = false;
        for (int i = 0; i < effectCubes.Count; i++)
        {
            
            //将localRotation每一帧逐渐线性改变
            effectCubes[i].attached.localRotation = Quaternion.RotateTowards(effectCubes[i].transform.Find("Attached").transform.localRotation, targets[i], rotationSpeed * Time.deltaTime);

            //判断已经与目标角度小于10度
            if (Quaternion.Angle(effectCubes[i].attached.localRotation, targets[i]) > 10)
            {
                busy = true;
            }
        }
        if (!busy)
        {
            MyTriggerManager.Instance.busyRotates.Remove(this);
            return;
        }
    }
}
