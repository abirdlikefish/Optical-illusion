using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager
{
    List<Cube> cubes = new List<Cube>();
    List<Cube_Rotatable> cube_Rotatables = new List<Cube_Rotatable>();
    BaseCube[,,] cube_matrix = new BaseCube[100, 100, 100];

    GameObject prefab_cube;
    GameObject prefab_cubeRotatable;

    Cube endCube;
    Cube begCube;
    public BaseCube SearchCubeMatrix(Vector3Int pos)
    {
        pos += new Vector3Int(50, 50, 50);
        return cube_matrix[pos.x, pos.y, pos.z];
    }
    public void SetCubeMatrix(Vector3Int pos , BaseCube midCube)
    {
        pos += new Vector3Int(50, 50, 50);
        cube_matrix[pos.x, pos.y, pos.z] = midCube;
    }
    public void Init(GameObject prefab_cube , GameObject prefab_cubeRotatable)
    {
        this.prefab_cube = prefab_cube;
        this.prefab_cubeRotatable = prefab_cubeRotatable;
    }

    public bool AddCube(Vector3Int pos)
    {
        if (SearchCubeMatrix(pos) != null)
        {
            Debug.Log("error : two cube in on position");
            return false;
        }
        Cube midCube= GameObject.Instantiate(prefab_cube).GetComponent<Cube>();
        midCube.Init(this , pos);
        // SetCubeMatrix(pos, midCube);
        this.cubes.Add(midCube);
        // return midCube;
        return true;
    }
    public bool AddCube_Rotatable(Vector3Int pos , int len , Vector3Int[] towards)
    {
        for(int i = 0 ; i < len ; i ++)
        {
            if( SearchCubeMatrix(pos + i * towards[0]) != null)
            {
                Debug.Log("error : cannot add cube_rotatable in " + (pos + i * towards[0]));
                Debug.Log( "object name is " + SearchCubeMatrix(pos + i * towards[0]).gameObject.name);
                return false;
            }
        }
        Cube_Rotatable midCube= GameObject.Instantiate(prefab_cubeRotatable).GetComponent<Cube_Rotatable>();
        midCube.Init(this , pos , len , towards);
        this.cube_Rotatables.Add(midCube);
        return true;
    }
    public void DrawCameraGrid()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            Command_normal.DrawCube2CameraGrid(cubes[i] , cubes[i].pos, cubes[i].depth);
        }
        for (int i = 0 ; i < cube_Rotatables.Count ; i ++)
        {
            cube_Rotatables[i].DrawCameraGrid();
        }
    }

    public void SetBegCube(Vector3Int pos)
    {
        if (this.begCube != null)
        {
            this.begCube.Reset();
        }
        if(SearchCubeMatrix(pos) is Cube_Rotatable)
        {
            Debug.Log("Cube_Rotatable cannot be begCube");
            return ;
        }
        this.begCube = SearchCubeMatrix(pos) as Cube;
        begCube.SetBeg();

    }
    public bool SetEndCube(BaseCube endCube)
    {
        if(endCube is Cube == false)
            return false;
        if (this.endCube != null)
        {
            this.endCube.Reset();
        }
        this.endCube = endCube as Cube;
        endCube.SetEnd();
        return true;
    }

    public bool DeleteCube(Vector3Int pos)
    {
        if (SearchCubeMatrix(pos) == null)
        {
            Debug.Log("No Cube found");
            return false;
        }
        if (begCube.pos == pos)
        {
            Debug.Log("Can't delete begCube");
            return false;
        }

        BaseCube midCube = SearchCubeMatrix(pos);
        if(midCube is Cube_Rotatable)
        {
            cube_Rotatables.Remove(midCube as Cube_Rotatable);

            (midCube as Cube_Rotatable).Remove();
        }
        else if(midCube is Cube)
        {
            cubes.Remove(midCube as Cube);

            SetCubeMatrix(pos, null);
            GameObject.Destroy(midCube.gameObject);
        }
        return true;
    }
    
    public void CleanCube_All()
    {
        for(int i = 0; i < cubes.Count; i ++)
        {
            SetCubeMatrix(cubes[i].pos, null);
            GameObject.Destroy(cubes[i].gameObject);
        }
        cubes.Clear();
        for(int i = 0 ; i < cube_Rotatables.Count; i  ++)
        {
            cube_Rotatables[i].Remove();
        }
        cube_Rotatables.Clear();
    }

    public bool WriteCubeListToLevelData(int index)
    {
        for(int i = 0 ; i < cubes.Count ; i++)
        {
            Command_normal.WriteCubeToLevelData(index , cubes[i].pos.x , cubes[i].pos.y , cubes[i].pos.z );
        }
        for(int i = 0 ; i < cube_Rotatables.Count ; i++)
        {
            Command_normal.WriteCubeRotatableToLevelData(index , cube_Rotatables[i].pos , cube_Rotatables[i].len , cube_Rotatables[i].towards);
        }
        Command_normal.WriteBegPosToLevelData(index , begCube.pos.x , begCube.pos.y , begCube.pos.z);
        return true;
    }
}