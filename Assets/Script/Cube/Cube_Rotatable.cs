using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;

public class Cube_Rotatable : BaseCube
{
    public GameObject prefab_subCube;
    List<GameObject>subCubes = new List<GameObject>();
    public static Vector3Int[] staDir = new Vector3Int[6] {Vector3Int.up , Vector3Int.down , Vector3Int.left , Vector3Int.right , Vector3Int.forward , Vector3Int.back}; 
    public static int[,] dir = new int[6,6]{{5,4,2,3,0,1} , {4,5,2,3,1,0} , {0,1,5,4,2,3} , {0,1,4,5,3,2} , {0,1,2,3,4,5} , {0,1,3,2,5,4}}; 
    public int[] len = new int[6];

    public int[] towards;
    public int towardsNum;
    public int towardIndex;
    public void Init(CubeManager cubeManager , Vector3Int pos , int[] len , int[] towards)
    {
        this.cubeManager = cubeManager;

        this.pos = pos;
        transform.position = pos;
        this.len = len;
        this.towards = towards;
        towardIndex = 0;
        towardsNum = 0; 
        for(int i = 0 ; i < 4 ; i ++)
        {
            if(towards[i] == -1 )
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
        CleanSubCube();
        Destroy(gameObject);
    }
    public void DrawCameraGrid()
    {
        Command_normal.DrawCube2CameraGrid(this , pos , pos.x + pos.y + pos.z);
        for(int i = 0 ; i < 6 ; i ++)
        {
            for(int j = 1 ; j <= len[i] ; j++)
            {
                Vector3Int midPos = pos + j * staDir[dir[towards[towardIndex],i]];
                Command_normal.DrawCube2CameraGrid(this ,midPos , midPos.x + midPos.y + midPos.z);
            }
        }
    }

    public static bool IsPlacable(CubeManager cubeManager , Vector3Int pos , int[] len , int[] towards)
    {
        if(cubeManager.SearchCubeMatrix(pos) != null)   return false;
        for(int i = 0 ; i < 6 ; i ++)
        {
            for(int j = 1 ; j <= len[i] ; j++)
            {
                if(cubeManager.SearchCubeMatrix(pos + j * staDir[dir[4,i]]) != null)    return false;
            }
        }
        return true;
    }

    private void SetPosition()
    {
        transform.position = pos;
        transform.LookAt(transform.position + staDir[towards[towardIndex]]);

        CleanSubCube();
        for(int i = 0 ; i < 6 ; i ++)
        {
            for(int j = 1 ; j <= len[i] ; j++)
            {
                SetSubCube(pos + j * staDir[dir[towards[towardIndex],i]]);
            }
        }

    }
    private void SetSubCube(Vector3Int pos)
    {
        GameObject subCube = GameObject.Instantiate(prefab_subCube);
        subCube.transform.position = pos;
        subCubes.Add(subCube);
    }
    private void CleanSubCube()
    {
        for(int i = 0 ; i < subCubes.Count ; i ++)
        {
            Destroy(subCubes[i]);
        }
        subCubes.Clear();
    }
    private void SetCubeMatrix(Cube_Rotatable cube_Rotatable)
    {
        cubeManager.SetCubeMatrix(pos , cube_Rotatable);
        for(int i = 0 ; i < 6 ; i ++)
        {
            for(int j = 1 ; j <= len[i] ; j++)
            {
                cubeManager.SetCubeMatrix(pos + j * staDir[dir[towards[towardIndex],i]] , cube_Rotatable);
            }
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
