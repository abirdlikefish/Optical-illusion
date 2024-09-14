using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube_Movable : BaseCube
{
    public GameObject prefab_subCube;
    List<GameObject>subCubes = new List<GameObject>();
    public static Vector3Int[] staDir = new Vector3Int[6] {Vector3Int.up , Vector3Int.down , Vector3Int.left , Vector3Int.right , Vector3Int.forward , Vector3Int.back}; 
    public int[] len = new int[6];

    public int moveDir ;
    public int maxMoveDistance;
    public int moveDistance;
    public void Init(CubeManager cubeManager , Vector3Int pos , int[] len , int moveDir , int maxMoveDistance)
    {
        this.cubeManager = cubeManager;

        this.pos = pos;
        transform.position = pos;
        this.len = len;
        this.moveDir = moveDir;
        this.maxMoveDistance = maxMoveDistance;
        this.moveDistance = 0;

        SetPosition();
        SetCubeMatrix(this);
    }

    public void Move()
    {
        SetCubeMatrix(null);

        moveDistance ++;
        if(moveDistance == maxMoveDistance)
            moveDistance = 0;        
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
        Vector3Int midPos = pos + staDir[moveDir] * moveDistance;
        Command_normal.DrawCube2CameraGrid(this ,midPos , midPos.x + midPos.y + midPos.z);
        for(int i = 0 ; i < 6 ; i ++)
        {
            for(int j = 1 ; j <= len[i] ; j++)
            {
                midPos = pos + j * staDir[i] + staDir[moveDir] * moveDistance;
                Command_normal.DrawCube2CameraGrid(this ,midPos , midPos.x + midPos.y + midPos.z);
            }
        }
    }

    public static bool IsPlacable(CubeManager cubeManager , Vector3Int pos , int[] len , int moveDir , int maxMoveDistance)
    {
        if(cubeManager.SearchCubeMatrix(pos) != null)   return false;
        for(int i = 0 ; i < 6 ; i ++)
        {
            for(int j = 1 ; j <= len[i] ; j++)
            {
                Vector3Int midPos = pos + j * staDir[i] ;
                if(cubeManager.SearchCubeMatrix(midPos) != null)    return false;
            }
        }
        return true;
    }

    private void SetPosition()
    {
        transform.position = pos + staDir[moveDir] * moveDistance;

        CleanSubCube();
        for(int i = 0 ; i < 6 ; i ++)
        {
            for(int j = 1 ; j <= len[i] ; j++)
            {
                Vector3Int midPos = pos + j * staDir[i] + staDir[moveDir] * moveDistance;
                SetSubCube(midPos);
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
    private void SetCubeMatrix(Cube_Movable cube_Rotatable)
    {
        cubeManager.SetCubeMatrix(pos + staDir[moveDir] * moveDistance , cube_Rotatable);
        for(int i = 0 ; i < 6 ; i ++)
        {
            for(int j = 1 ; j <= len[i] ; j++)
            {
                Vector3Int midPos = pos + j * staDir[i] + staDir[moveDir] * moveDistance;
                cubeManager.SetCubeMatrix(midPos , cube_Rotatable);
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
