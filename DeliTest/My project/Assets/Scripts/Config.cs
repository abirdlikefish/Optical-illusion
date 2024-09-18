using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : Singleton<Config>
{
    [Header("EditorMode")]
    //显示可能路线
    public bool showPossiblePath = true;
    [Header("Near Judge")]
    //面中心点靠近时的屏幕坐标距离
    public float centerNearDistance = 15f;
    //判断两立方体是否相邻的世界坐标距离
    public float cubeNearDistance = 0.1f;
    [Header("Obstacle Judge")]
    //障碍检测时，给正方体的面采样（n*n网格）的缩小倍率
    public float obSampleScale = 0.8f;
    //上述n的值
    public int obSampleCount = 5;

    [Header("Player")]
    public float moveSpeed = 0.08f;
    public float ballRotateSpeed = 4f;

    [Header("Rotater")]
    //按住鼠标右键旋转的速度
    public float mouseRotateSpeed = 1.0f;
    //自动吸附旋转的速度
    public float magnetRotateSpeed = 1.0f;
    //玩家移动时是否可以通过鼠标旋转
    public bool canRotateWhilePlayerMove = true;

    public void OnValidate()
    {
        for(int i=0;i<CubeCombiner.Instance?.transform.childCount; i++)
        {
            CubeCombiner.Instance.transform.GetChild(i).GetComponent<Cube>().MyOnValidate();
        }
    }
}
