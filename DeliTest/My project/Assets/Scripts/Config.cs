using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : Singleton<Config>
{
    

    [Header("Near Judge")]
    //面中心点靠近时的屏幕坐标距离
    public float centerNearDistance = 15f;
    public float cubeNearDistance = 0.1f;
    [Header("Obstacle Judge")]
    //障碍检测时，面中心向顶点位置移动的距离，1代表移到顶点处
    public float CenterObstacleMultiplier = 0.8f;

    [Header("Obstacle Info")]
    public float ObInfoTime = 1f;
    public float CenterObstacleScale = 0.1f;

    [Header("Player")]
    public float moveSpeed = 0.08f;
    public float ballRotateSpeed = 4f;

    [Header("Rotater")]
    public float mouseRotateSpeed = 1.0f;
    public float magnetRotateSpeed = 1.0f;

    public void OnValidate()
    {
        for(int i=0;i<CubeCombiner.Instance.transform.childCount; i++)
        {
            CubeCombiner.Instance.transform.GetChild(i).GetComponent<Cube>().MyOnValidate();
        }
    }
}
