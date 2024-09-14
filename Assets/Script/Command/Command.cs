using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;


public class Command
{
    protected static GameManager gameManager;
    protected static CameraGridManager cameraGridManager;
    protected static CubeManager cubeManager;
    protected static SaveManager saveManager;
    protected static PlayerManager playerManager;
    protected static UIManager uiManager;
    public static void Init(GameManager gameManager , CameraGridManager cameraGridManager, CubeManager cubeManager, SaveManager saveManager , PlayerManager playerManager , UIManager uiManager)
    {
        Command.gameManager = gameManager;
        Command.cameraGridManager = cameraGridManager;
        Command.cubeManager = cubeManager;
        Command.saveManager = saveManager;
        Command.playerManager = playerManager;
        Command.uiManager = uiManager;
    }
}

public class Command_normal : Command
{
    public static bool SaveDataByIndex(int index)
    {
        saveManager.CreateLevelDataByIndex(index);
        cubeManager.WriteCubeListToLevelData(index);
        return saveManager.SaveDataByIndex(index);
    }
    public static bool SaveCurrentLevelData()
    {
        return SaveDataByIndex(gameManager.levelIndex);
    }

    public static int LoadData_All()
    {
        return saveManager.LoadData_All();
    }
    public static bool LoadDataByIndex(int index)
    {
        if(saveManager.LoadDataByIndex(index))
        {
            uiManager.AddLevel(index);
            return true;
        }
        return false;
    }
    
    // public static bool WriteCubeListToLevelData(int index)
    // {
    //     saveManager.CleanLevelData(index);
    //     return cubeManager.WriteCubeListToLevelData(index);
    // }
    public static bool WriteCubeToLevelData(int index , int x , int y , int z)
    {
        return saveManager.WriteCubeToLevelData(index, x, y, z);
    }
    public static bool WriteCubeRotatableToLevelData(int index , Vector3Int pos , int[] len , int[] towards)
    {
        return saveManager.WriteCubeRotatableToLevelData(index , pos , len , towards);
    }
    public static bool WriteCubeMovableToLevelData(int index , Vector3Int pos , int[] len , int moveDir , int maxMoveDistance)
    {
        return saveManager.WriteCubeMovableToLevelData(index , pos , len , moveDir , maxMoveDistance);
    }
    public static bool WriteBegPosToLevelData(int index , int x , int y , int z)
    {
        return saveManager.WriteBegPosToLevelData(index, x, y, z);
    }


    public static int CreateNewLevel()
    {
        int index = saveManager.CreateLevelData();
        if(index != -1)
            UseLevelData(index);
        else
            Debug.LogWarning("Could not create level");
        uiManager.AddLevel(index);
        return index;
    }

    public static bool UseLevelData(int index)
    {
        if(saveManager.IsIndexExist(index) == false)
        {
            return false;
        }
        cameraGridManager.CleanCameraGrid();
        cubeManager.CleanCube_All();
        gameManager.SetLock(false);
        gameManager.levelIndex = index;
        saveManager.InitMap(index);

        saveManager.SetBeginPosition(index);

        RefreshCameraGrid();
        return true;
    }
    public static bool FindPath()
    {
        if(playerManager.pos == null)
            Debug.LogError("no player position");
        cameraGridManager.FindPath(playerManager.pos);
        return true;
    }
    public static void ChangeMode(Mode.ModeName modeName)
    {
        cameraGridManager.CleanCameraGrid();
        cubeManager.CleanCube_All();
        playerManager.CleanPlayer();
        
        gameManager.ChangeMode(modeName);
        uiManager.UseUI(modeName);
    }
    public static bool SetBeginPosition(Vector3Int pos)
    {
        cubeManager.SetBegCube(pos);
        playerManager.InitPlayer(pos);
        return true;
    }
    public static bool SetBeginPosition()
    {
        return SetBeginPosition(uiManager.GetInputPos());
    }
    public static bool AddCube(Vector3Int pos)
    {
        return cubeManager.AddCube(pos);
    }
    public static bool AddCube()
    {
        Vector3Int pos = uiManager.GetInputPos();
        return AddCube(pos);
    }
    public static bool AddCube_Rotatable(Vector3Int pos , int[] len , int[] towards)
    {
        return cubeManager.AddCube_Rotatable(pos, len, towards);
    }
    public static bool AddCube_Rotatable()
    {
        int[] len = new int[6]{ uiManager.GetParament(0), uiManager.GetParament(1) , uiManager.GetParament(2) , 
                                uiManager.GetParament(3) , uiManager.GetParament(4) , uiManager.GetParament(5)};
        int[] midTowards = new int[4]{4 , uiManager.GetParament(6) , uiManager.GetParament(7) , uiManager.GetParament(8)};
        return AddCube_Rotatable(uiManager.GetInputPos() , len , midTowards);
    }

    public static bool AddCube_Movable(Vector3Int pos , int[] len , int moveDir , int maxMoveDistance)
    {
        return cubeManager.AddCube_Movable(pos, len, moveDir , maxMoveDistance);
    }
    public static bool AddCube_Movable()
    {
        int[] len = new int[6]{ uiManager.GetParament(0), uiManager.GetParament(1) , uiManager.GetParament(2) , 
                                uiManager.GetParament(3) , uiManager.GetParament(4) , uiManager.GetParament(5)};
        return AddCube_Movable(uiManager.GetInputPos() , len , uiManager.GetParament(6) , uiManager.GetParament(7));
    }

    public static bool DeleteCube(Vector3Int pos)
    {
        return cubeManager.DeleteCube(pos);
    }
    public static bool DeleteCube()
    {
        Vector3Int pos = uiManager.GetInputPos();
        return DeleteCube(pos);
    }

    public static void DrawCube2CameraGrid(BaseCube cube , Vector3Int pos , int depth)
    {
        cameraGridManager.DrawGridFromCube(cube, pos, depth);
    }
    public static void AddTargetToPlayer(Vector3Int pos)
    {
        playerManager.AddTargetToPlayer(pos);
    }
    public static bool PlayerMove2Target(Vector3Int pos)
    {
        if(cameraGridManager.IsPassable(pos) && cameraGridManager.IsVisited(pos))
        {
            cameraGridManager.SetTargetsToPlayer(pos);
            gameManager.SetLock(true);
            playerManager.MoveBegin();
            return true;
        }
        else
        {
            return false;
        }
    }

    //used in play mode
    public static bool MouseClicked(bool isRig)
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10000, LayerMask.GetMask("BaseCube")))
        {
            BaseCube baseCube = hit.collider.GetComponent<BaseCube>();
            if(isRig)
            {
                if(baseCube is Cube_Rotatable)
                {
                    (baseCube as Cube_Rotatable).Rotate();
                    RefreshCameraGrid();
                }
                else if(baseCube is Cube_Movable)
                {
                    Debug.Log("begin move");
                    (baseCube as Cube_Movable).Move();
                    RefreshCameraGrid();
                }
            }
            else
            {
                if(cubeManager.SetEndCube(baseCube))
                {
                    return PlayerMove2Target((baseCube as Cube).pos);
                }
            }
        }
        return false;
    }

    public static void RefreshCameraGrid()
    {
        cameraGridManager.CleanCameraGrid();
        cubeManager.DrawCameraGrid();
        FindPath();
    }

    // used in edit mode
    public static bool MouseSelected()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10000, LayerMask.GetMask("BaseCube")))
        {
                Vector3Int pos ;
                pos = hit.collider.GetComponent<BaseCube>().pos;
                uiManager.SetInputPos(pos);
                return true;
        }
        return false;

    }
    public static void Arrive()
    {
        gameManager.SetLock(false);
        FindPath();
    }
}
