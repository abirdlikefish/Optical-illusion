using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube_Rotatable : BaseCube
{
    // public Vector3Int pos;
    public int len;
    public Vector3Int[] towards;
    public int towardsNum;
    public int towardIndex;
    public void Init(CubeManager cubeManager , Vector3Int pos , int len , Vector3Int[] towards)
    {
        this.cubeManager = cubeManager;

        this.pos = pos;
        this.len = len;
        this.towards = towards;
        towardIndex = 0;
        towardsNum = 0;
        for(int i = 0 ; i < 3 ; i ++)
        {
            if(towards[i] == Vector3Int.zero )
                break;
            towardsNum ++;
        }
        SetPosition();
        SetCubeMatrix(this);
    }

    public void Rotate()
    {
        SetCubeMatrix(null);

        towardIndex ++;
        if(towardIndex == towardsNum)
            towardIndex = 0;
        // transform.position = pos + len * 0.5f * new Vector3(towards[towardIndex].x , towards[towardIndex].y , towards[towardIndex].z); 
        // transform.localScale = len * towards[towardIndex];
        
        SetPosition();
        
        SetCubeMatrix(this);
    }
    public void Remove()
    {
        SetCubeMatrix(null);
        Destroy(gameObject);
    }
    public void DrawCameraGrid()
    {
        for(int i = 0 ; i < len ; i ++)
        {
            Vector3Int midPos = pos + i * towards[towardIndex];
            Command_normal.DrawCube2CameraGrid(this ,midPos, midPos.x + midPos.y + midPos.z);
        }
    }

    private void SetPosition()
    {
        transform.position = pos + (len - 1) * 0.5f * new Vector3(towards[towardIndex].x , towards[towardIndex].y , towards[towardIndex].z); 
        // transform.localScale = (len - 1) * towards[towardIndex] + Vector3Int.one;
        transform.localScale = new Vector3Int(1 , 1 , len);
        transform.LookAt(transform.position + towards[towardIndex]);

    }
    private void SetCubeMatrix(Cube_Rotatable cube_Rotatable)
    {
        for(int i = 0 ; i < len ; i ++)
        {
            cubeManager.SetCubeMatrix(pos + towards[towardIndex] * i , cube_Rotatable);
        }
    }
    public override void SetBeg()
    {
        Debug.Log("can't set begin in Cube_Rotatable");
    }
    public override void SetEnd()
    {
    }
    public override void Reset()
    {
    }

}
