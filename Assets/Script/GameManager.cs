using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject prefab_cube;
    public GameObject prefab_cubeRotatable;
    public GameObject prefab_cubeMovable;
    public GameObject prefab_player;
    public Material material_black;
    public Material material_white;
    public Material material_red;


    CameraGridManager cameraGridManager;
    CubeManager cubeManager;
    SaveManager saveManager;
    PlayerManager playerManager;
    CommandManager commandManager;
    UIManager uiManager;
    CameraManager cameraManager;

    Mode currentMode;
    Mode playMode;
    Mode editMode;

    void Awake()
    {

        CreateManagers();
        InitManagers();
    }
    void CreateManagers()
    {
        cameraGridManager = new CameraGridManager();        
        cubeManager = new CubeManager();
        saveManager = new SaveManager();
        commandManager = new CommandManager();
        uiManager = new UIManager();
        playerManager = new PlayerManager();

        playMode = new PlayMode();
        editMode = new EditMode();

        cameraManager = Camera.main.transform.GetComponent<CameraManager>();

    }
    void InitManagers()
    {
        cameraGridManager.Init();
        cubeManager.Init(prefab_cube , prefab_cubeRotatable , prefab_cubeMovable);
        saveManager.Init();
        commandManager.Init(this , cameraGridManager, cubeManager, saveManager , playerManager , uiManager , cameraManager);
        uiManager.Init();
        playerManager.Init(prefab_player);
        BaseCube.InitMaterials(material_black , material_white , material_red);

        playMode.Init();
        editMode.Init();

        cameraManager.Init(0);
    }

    void Start()
    {
        int num = Command_normal.LoadData_All();
        Debug.Log("load " + num +" levels ");
        
        Command_normal.ChangeMode(Mode.ModeName.PlayMode);
    }

    void Update()
    {
        currentMode.Update();
    }

    public void ChangeMode(Mode.ModeName modeName)
    {
        if(currentMode != null)
        {
            currentMode.ExitMode();
        }
        if(modeName == Mode.ModeName.PlayMode)
        {
            currentMode = playMode;
        }
        else
        {
            currentMode = editMode;

        }
        currentMode.EnterMode();
    }
    public int levelIndex{get{return currentMode.levelIndex;}set{currentMode.levelIndex = value;}}


    public void SetLock(bool isLock)
    {
        
        currentMode.SetLock(isLock);
    }

}