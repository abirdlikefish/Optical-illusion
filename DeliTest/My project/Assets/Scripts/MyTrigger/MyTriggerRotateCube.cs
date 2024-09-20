using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTriggerRotateCube : MyTrigger
{
    [SerializeField]
    //每次点击旋转Delta角度
    Vector3 delta;
    [SerializeField]
    // 旋转速度，每秒旋转的角度
    float rotationSpeed = 90f;
    bool hasResetCenter = false;
    [SerializeField]
    List<Vector3> targets = new();
    private void Start()
    {
        foreach (var effectCube in effectCubes)
        {
            targets.Add(effectCube.transform.eulerAngles);
            targets[^1] = new Vector3(Mathf.Round(targets[^1].x), Mathf.Round(targets[^1].y), Mathf.Round(targets[^1].z));
        }
    }
    void Update()
    {
        if (UIManager.Instance.IsUIBusy)
            return;
        if (triggered)
        {
            hasResetCenter = false;
            for (int i = 0; i < targets.Count; i++)
            {
                targets[i] = targets[i] + delta;
                targets[i] = new Vector3(Mathf.Round(targets[i].x), Mathf.Round(targets[i].y), Mathf.Round(targets[i].z));
            }
            triggered = false;
        }
        MyTriggerManager.Instance.busyRotates.Add(this);
        bool busy = false;
        for (int i = 0; i < effectCubes.Count; i++)
        {
            effectCubes[i].attached.localRotation = Quaternion.RotateTowards(effectCubes[i].attached.localRotation,Quaternion.Euler(targets[i]), rotationSpeed * Time.deltaTime);
            if (Quaternion.Angle(effectCubes[i].attached.localRotation, Quaternion.Euler(targets[i])) > 1)
            {
                busy = true;
            }
        }
        if (!busy)
        {
            MyTriggerManager.Instance.busyRotates.Remove(this);
            if (hasResetCenter)
                return;
            hasResetCenter = true;
            for (int i = 0; i < effectCubes.Count; i++)
            {
                for (int j = 0; j < effectCubes[i].attached.childCount; j++)
                {
                    effectCubes[i].attached.localRotation = Quaternion.Euler(targets[i]);
                    GameObject g = effectCubes[i].attached.GetChild(j).gameObject;
                    if (!g.activeSelf || g.GetComponent<IAttached>() == null)
                        continue;
                    g.GetComponent<IAttached>().SetCurCenter(effectCubes[i].GetNearestCenterByRotate(g.transform));
                }

            }
        }
        
    }
}
