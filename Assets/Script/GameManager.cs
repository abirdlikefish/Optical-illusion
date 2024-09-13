using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject prefab_cube;
    public GameObject prefab_player;

    CameraGridManager cameraGridManager;
    CubeManager cubeManager;
    SaveManager saveManager;
    PlayerManager playerManager;
    CommandManager commandManager;
    UIManager uiManager;

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

    }
    void InitManagers()
    {
        cameraGridManager.Init();
        cubeManager.Init(prefab_cube);
        saveManager.Init();
        commandManager.Init(this , cameraGridManager, cubeManager, saveManager , playerManager , uiManager);
        uiManager.Init();
        playerManager.Init(prefab_player);

        playMode.Init();
        editMode.Init();

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
