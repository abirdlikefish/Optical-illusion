using System.Collections;
using System.Collections.Generic;
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
        bool busy = false;
        for (int i = 0; i < effectCubes.Count; i++)
        {
            //将localRotation每一帧逐渐线性改变
            effectCubes[i].transform.Find("Attached").transform.localRotation = Quaternion.RotateTowards(effectCubes[i].transform.Find("Attached").transform.localRotation, targets[i], rotationSpeed * Time.deltaTime);

            if (effectCubes[i].transform.Find("Attached").transform.localRotation != targets[i])
            {
                busy = true;
            }
        }
        if (busy)
        {
            AddBusy(this);
        }
        else
        {
            RemoveBusy(this);
        }
    }
}
