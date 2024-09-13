using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMode : Mode
{
    // public override void Init(CameraGridManager cameraGridManager, CubeManager cubeManager, SaveManager saveManager)
    public override void Init()
    {
        // base.Init(cameraGridManager, cubeManager, saveManager);
        base.Init();
        modeName = ModeName.PlayMode;
    }
    public override void EnterMode()
    {
        base.EnterMode();
        
        int num = Command_normal.LoadData_All();
        Debug.Log("load " + num +" levels ");

        // cubeManager.DrawCameraGrid();
        // cameraGridManager.FindPath(player.pos);
    }
    public override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0) && !isLocked)
        {
            Command_normal.MouseClicked();
        }
    }

    public override void ExitMode()
    {
        
        base.ExitMode();
        // cameraGridManager.CleanCameraGrid();
        // cubeManager.CleanCube_All();
    }
}
