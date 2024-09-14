using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMode : Mode
{
    public override void Init()
    {
        base.Init();
        modeName = ModeName.PlayMode;
    }
    public override void EnterMode()
    {
        base.EnterMode();
        
    }
    public override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0) && !isLocked)
        {
            Command_normal.MouseClicked(false);
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            Command_normal.ChangeMode(ModeName.EditMode);
        }
    }

    public override void ExitMode()
    {
        
        base.ExitMode();
    }
}
