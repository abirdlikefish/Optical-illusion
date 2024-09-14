using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCube : MonoBehaviour
{
    public static Material black;
    public static Material white;
    public static Material red;
    public Vector3Int pos;
    public static void InitMaterials(Material black , Material white , Material red)
    {
        BaseCube.black = black;
        BaseCube.white = white;
        BaseCube.red = red;
    }
    protected CubeManager cubeManager;
    public virtual void SetBeg(){}
    public virtual void SetEnd(){}
    public virtual void Reset(){}
}
