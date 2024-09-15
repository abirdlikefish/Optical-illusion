using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;

public class Cube_Rotatable : BaseCube
{
    public GameObject prefab_subCube;
    List<GameObject>subCubes = new List<GameObject>();
    public static Vector3Int[] staDir = new Vector3Int[6] {Vector3Int.up , Vector3Int.down , Vector3Int.left , Vector3Int.right , Vector3Int.forward , Vector3Int.back}; 
    // public static int[,] dir = new int[6,6]{{5,4,2,3,0,1} , {4,5,2,3,1,0} , {0,1,5,4,2,3} , {0,1,4,5,3,2} , {0,1,2,3,4,5} , {0,1,3,2,5,4}}; 
    public int[] len = new int[6];

    // public int[] towards;
    // public int towardsNum;
    // public int towardIndex;

    public int axisIndex;
    public int possibleAngle;
    public int currentAngle;

    public Vector3Int GetDirVector(int dir)
    {
        // if(dir / 2 == axisIndex / 2) return staDir[dir];
        // if(currentAngle == 0)   return staDir[dir];
        // if(currentAngle == 1) return Vector3Int.RoundToInt(Vector3.Cross(staDir[axisIndex] , staDir[dir]));
        // if(currentAngle == 2)   return staDir[dir];
        // if(currentAngle == 3) return Vector3Int.RoundToInt(Vector3.Cross(staDir[dir] , staDir[axisIndex] ));
        // Debug.Log("Error current angle: " + currentAngle);
        // return Vector3Int.zero;

        return GetDirVector(dir , axisIndex , currentAngle);
    }
    public static Vector3Int GetDirVector(int dir , int axisIndex , int currentAngle)
    {
        if(dir / 2 == axisIndex / 2) return staDir[dir];
        if(currentAngle == 0)   return staDir[dir];
        if(currentAngle == 1) return Vector3Int.RoundToInt(Vector3.Cross(staDir[axisIndex] , staDir[dir]));
        if(currentAngle == 2)   return -staDir[dir];
        if(currentAngle == 3) return Vector3Int.RoundToInt(Vector3.Cross(staDir[dir] , staDir[axisIndex] ));
        Debug.Log("Error current angle: " + currentAngle);
        return Vector3Int.zero;
    }
    public bool Init(CubeManager cubeManager , Vector3Int pos , int[] len , int axisIndex , int possibleAngle)
    {
        this.cubeManager = cubeManager;

        this.pos = pos;
        transform.position = pos;
        this.len = len;
        this.axisIndex = axisIndex;
        this.possibleAngle = possibleAngle;
        if(possibleAngle % 2 != 1)
        {
            this.name = this.name + "_Error";
            Debug.Log("init cubeRotatable failed , cubeName = " + this.name + " possibleAngle = " + possibleAngle);

            return false;
        }
        currentAngle = 0;
        SetPosition();
        SetCubeMatrix(this);
        return true;
    }
    // public void Init(CubeManager cubeManager , Vector3Int pos , int[] len , int[] towards)
    // {
    //     this.cubeManager = cubeManager;

    //     this.pos = pos;
    //     transform.position = pos;
    //     this.len = len;
    //     this.towards = towards;
    //     towardIndex = 0;
    //     towardsNum = 0; 
    //     for(int i = 0 ; i < 4 ; i ++)
    //     {
    //         if(towards[i] == -1 )
    //             break;
    //         towardsNum ++;
    //     }
    //     SetPosition();
    //     SetCubeMatrix(this);
    // }

    public void Rotate()
    {
        SetCubeMatrix(null);

        do
        {
            currentAngle ++;
            if(currentAngle == 4)
            currentAngle = 0;
        }
        while(((possibleAngle >> currentAngle) & 1) == 0);

        // towardIndex ++;
        // if(towardIndex == towardsNum)
        //     towardIndex = 0;
        
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
                Vector3Int midPos = pos + j * GetDirVector(i);
                // Vector3Int midPos = pos + j * staDir[dir[towards[towardIndex],i]];
                Command_normal.DrawCube2CameraGrid(this ,midPos , midPos.x + midPos.y + midPos.z);
            }
        }
    }

    public static bool IsPlacable(CubeManager cubeManager , Vector3Int pos , int[] len , int axisIndex , int possibleAngle)
    {
        if(cubeManager.SearchCubeMatrix(pos) != null)   return false;
        for(int i = 0 ; i < 6 ; i ++)
        {
            for(int j = 1 ; j <= len[i] ; j++)
            {
                Vector3Int midPos = pos + j * GetDirVector(i , axisIndex , 0);
                // if(cubeManager.SearchCubeMatrix(pos + j * staDir[dir[4,i]]) != null)    return false;
                if(cubeManager.SearchCubeMatrix(midPos))    return false;
            }
        }
        return true;
    }

    private void SetPosition()
    {
        transform.position = pos;
        // transform.LookAt(transform.position + staDir[towards[towardIndex]]);

        CleanSubCube();
        for(int i = 0 ; i < 6 ; i ++)
        {
            for(int j = 1 ; j <= len[i] ; j++)
            {
                Vector3Int midPos = pos + j * GetDirVector(i);
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
    private void SetCubeMatrix(Cube_Rotatable cube_Rotatable)
    {
        cubeManager.SetCubeMatrix(pos , cube_Rotatable);
        for(int i = 0 ; i < 6 ; i ++)
        {
            for(int j = 1 ; j <= len[i] ; j++)
            {
                Vector3Int midPos = pos + j * GetDirVector(i);
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
