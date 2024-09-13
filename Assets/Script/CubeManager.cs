using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager
{
    List<Cube> cubes = new List<Cube>();
    Cube[,,] cube_matrix = new Cube[100, 100, 100];

    GameObject prefab_cube;

    Cube endCube;
    Cube begCube;
    Cube SearchCubeMatrix(Vector3Int pos)
    {
        pos += new Vector3Int(50, 50, 50);
        return cube_matrix[pos.x, pos.y, pos.z];
    }
    void SetCubeMatrix(Vector3Int pos , Cube midCube)
    {
        pos += new Vector3Int(50, 50, 50);
        cube_matrix[pos.x, pos.y, pos.z] = midCube;
    }
    public void Init(GameObject prefab_cube)
    {
        this.prefab_cube = prefab_cube;
    }

    public Cube AddCube(Vector3Int pos)
    {
        if (SearchCubeMatrix(pos) != null)
        {
            Debug.Log("error : two cube in on position");
            return SearchCubeMatrix(pos);
        }
        Cube midCube= GameObject.Instantiate(prefab_cube, pos , Quaternion.identity).GetComponent<Cube>();
        midCube.Init(pos);
        SetCubeMatrix(pos, midCube);
        this.cubes.Add(midCube);
        return midCube;
    }
    public void DrawCameraGrid()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            Command_normal.DrawCube2CameraGrid(cubes[i], cubes[i].cameraGridPos, cubes[i].depth);
        }
    }

    public void SetBegCube(Vector3Int pos)
    {
        Vector2Int midPos = CameraGridManager.CubePos2CameraGridPos(pos);
        this.begCube = SearchCubeMatrix(pos);
        begCube.SetBeg();

    }
    public Vector3Int SetEndCube(Cube endCube)
    {
            if (this.endCube != null)
            {
                this.endCube.Reset();
            }
            this.endCube = endCube;
            endCube.SetEnd();
            return endCube.pos;
    }

    public void CleanCube(Vector3Int pos)
    {
        if (SearchCubeMatrix(pos) == null) return;
        Cube midCube = SearchCubeMatrix(pos);
        cubes.Remove(midCube);
        SetCubeMatrix(pos, null);
        
        GameObject.Destroy(midCube.gameObject);

    }
    
    public void CleanCube_All()
    {
        for(int i = 0; i < cubes.Count; i ++)
        {
            SetCubeMatrix(cubes[i].pos, null);
            GameObject.Destroy(cubes[i].gameObject);
        }
        cubes.Clear();
    }

    public bool WriteCubeListToLevelData(int index)
    {
        for(int i = 0 ; i < cubes.Count ; i++)
        {
            Command_normal.WriteCubeToLevelData(index , cubes[i].pos.x , cubes[i].pos.y , cubes[i].pos.z );
        }
        Command_normal.WriteBegPosToLevelData(index , begCube.pos.x , begCube.pos.y , begCube.pos.z);
        return true;
    }
}
