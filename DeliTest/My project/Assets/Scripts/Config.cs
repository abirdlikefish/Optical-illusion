using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : Singleton<Config>
{
    //障碍检测时，面中心向顶点位置移动的距离，1代表移到顶点处
    public float CenterObstacleMultiplier = 0.8f;
    public float ObInfoTime = 1f;
    public float CenterObstacleScale = 0.1f;
}
