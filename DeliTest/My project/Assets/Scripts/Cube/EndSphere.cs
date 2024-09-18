using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSphere : MonoBehaviour
{
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
