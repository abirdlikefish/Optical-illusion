using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager
{
    List<Cube> cubes = new List<Cube>();
    Cube[,,] cube_matrix = new Cube[100, 100, 100];

    CameraGridManager cameraGridManager;
    GameObject prefab_cube;
    GameObject prefab_player;

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
    public void Init(CameraGridManager cameraGridManager , GameObject prefab_cube , GameObject prefab_player)
    {
        this.cameraGridManager = cameraGridManager;
        this.prefab_cube = prefab_cube;
        this.prefab_player = prefab_player;
    }
    //public void AddCube(Cube cube)
    //{
    //    cube.Init();
    //    this.cubes.Add(cube);
    //}
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
        cameraGridManager.ResetCameraGrid();
        for (int i = 0; i < cubes.Count; i++)
        {
            cameraGridManager.DrawGrid_L(cubes[i], cubes[i].cameraGridPos, cubes[i].depth, true);
            cameraGridManager.DrawGrid_R(cubes[i], cubes[i].cameraGridPos, cubes[i].depth, true);

            cameraGridManager.DrawGrid_L(cubes[i], cubes[i].cameraGridPos + new Vector2Int(1, 1), cubes[i].depth, false);
            cameraGridManager.DrawGrid_R(cubes[i], cubes[i].cameraGridPos + new Vector2Int(1, 1), cubes[i].depth, false);

            cameraGridManager.DrawGrid_L(cubes[i], cubes[i].cameraGridPos + new Vector2Int(0, 1), cubes[i].depth, false);
            cameraGridManager.DrawGrid_R(cubes[i], cubes[i].cameraGridPos + new Vector2Int(1, 0), cubes[i].depth, false);
        }
    }
    //public void SetBegCube(Cube begCube)
    //{
    //    this.begCube = begCube;
    //    begCube.SetBeg();
    //}
    public Player SetBegCube(Vector3Int pos)
    {
        Vector2Int midPos = CameraGridManager.CubePos2CameraGridPos(pos);
        if(cameraGridManager.IsPassable(midPos) == false)
        {
            Debug.Log("set begCube failed");
        }
        //SetBegCube(AddCube(pos));
        this.begCube = SearchCubeMatrix(pos);
        begCube.SetBeg();


        //Player player;
        Player player = GameObject.Instantiate(prefab_player).GetComponent<Player>();
        player.Init(pos , cameraGridManager);
        return player;
    }
    public bool SetEndCube(Cube endCube , Player player)
    {
        if(cameraGridManager.IsPassable(endCube.cameraGridPos) == false)
        {
            Debug.Log("failed to set endCube");
            return false;
        }
        else
        {
            bool isPassable = cameraGridManager.SetTarget(endCube.cameraGridPos, CameraGridManager.CubePos2CameraGridPos(player.pos), player);
            if (isPassable == false)
                return false;
            if (this.endCube != null)
            {
                this.endCube.Reset();
            }
            this.endCube = endCube;
            endCube.SetEnd();
            return true;
        }
    }

}
