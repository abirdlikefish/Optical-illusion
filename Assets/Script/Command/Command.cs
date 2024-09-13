using System.Collections;
using System.Collections.Generic;
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
        return saveManager.SaveDataByIndex(index);
    }
    public static bool SaveData_All()
    {
        return saveManager.SaveData_All();
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
        bool flag = saveManager.LoadDataByIndex(index);
        if(flag)
        {
            uiManager.AddLevel(index);
        }
        return flag;
    }
    public static bool WriteCubeListToLevelData(int index)
    {
        saveManager.CleanLevelData(index);
        return cubeManager.WriteCubeListToLevelData(index);
    }
    public static bool WriteCubeToLevelData(int index , int x , int y , int z)
    {
        return saveManager.WriteCubeToLevelData(index, x, y, z);
    }
    public static bool WriteBegPosToLevelData(int index , int x , int y , int z)
    {
        return saveManager.WriteBegPosToLevelData(index, x, y, z);
    }

    public static int CreateNewLevel()
    {
        return saveManager.CreateLevelData();
    }

    public static bool UseLevelData(int index)
    {
        if(saveManager.IsIndexExist(index) == false)
        {
            return false;
        }
        cameraGridManager.CleanCameraGrid();
        cubeManager.CleanCube_All();
        playerManager.CleanPlayer();
        gameManager.SetLock(false);
        gameManager.levelIndex = index;
        saveManager.InitMap(index);
        cubeManager.DrawCameraGrid();
        saveManager.SetBeginPosition(index);
        FindPath();
        return true;
    }
    public static bool FindPath()
    {
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
        if(cameraGridManager.IsPassable(pos) == false)
        {
            Debug.Log("set begCube failed , pos = " + pos);
            return false;
        }
        cubeManager.SetBegCube(pos);
        playerManager.InitPlayer(pos);
        return true;
    }
    public static bool AddCube(Vector3Int pos)
    {
        Cube midCube = cubeManager.AddCube(pos);
        if(midCube == null)
        {
            return false;
        }
        return true;
    }
    public static bool AddCube()
    {
        Vector3Int pos = uiManager.InputPos();
        return AddCube(pos);
    }
    public static void DrawCube2CameraGrid(Cube cube , Vector2Int pos , int depth)
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
    public static bool MouseClicked()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10000, LayerMask.GetMask("Cube")))
        {
            Vector3Int pos ;
            pos = cubeManager.SetEndCube(hit.collider.GetComponent<Cube>());
            return PlayerMove2Target(pos);
        }
        return false;
    }
    public static void Arrive()
    {
        gameManager.SetLock(false);
        FindPath();
    }
}
