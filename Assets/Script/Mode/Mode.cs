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
    public int levelIndex;
    public enum ModeName
    {
        PlayMode,
        EditMode
    };
    public ModeName modeName;
    public virtual void Init()
    {
    }


    public virtual void EnterMode()
    {
        SetLock(false);
        levelIndex = -1;
    }

    public virtual void Update()
    {

    }
    public virtual void ExitMode()
    {
        SetLock(true);
    }
}
