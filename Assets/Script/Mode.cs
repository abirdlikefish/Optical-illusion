using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode
{
    protected bool isLocked;
    public void SetLock(bool flag)
    {
        isLocked = flag;
    }
    public enum ModeName
    {
        PlayMode,
        EditMode
    };
    // public CameraGridManager cameraGridManager;
    // public CubeManager cubeManager;
    // public SaveManager saveManager;
    // public Player player;
    public ModeName modeName;
    // public virtual void Init(CameraGridManager cameraGridManager, CubeManager cubeManager, SaveManager saveManager)
    public virtual void Init()
    {
        // this.cameraGridManager = cameraGridManager;
        // this.cubeManager = cubeManager;
        // this.saveManager = saveManager;
    }


    public virtual void EnterMode()
    {
        SetLock(false);
    }

    public virtual void Update()
    {

    }
    public virtual void ExitMode()
    {
        SetLock(true);
    }
}
