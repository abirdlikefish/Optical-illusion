using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSphere : MonoBehaviour, IAttached
{
    public CenterPoint curCenter;
    public CenterPoint GetCurCenter()
    {
        return curCenter;
    }
    public void InitTransform(CenterPoint desCenter)
    {
        transform.position = desCenter.CenterToPlayerPos();
        transform.parent = desCenter.cube.attached;
        SetCurCenter(desCenter);
    }
    public void SetCurCenter(CenterPoint centerPoint)
    {
        curCenter = centerPoint;
    }
    public float minScale = 0.2f;
    public float maxScale = 0.4f;

    // Update is called once per frame
    void Update()
    {
        //随时间的三角函数改变scale
        float scale = minScale + (maxScale - minScale) * (1 + Mathf.Sin(Time.time * 2)) / 2;
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
