using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditMode : Mode
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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Command_normal.ChangeMode(ModeName.PlayMode);
        }
    }

    public override void ExitMode()
    {
        
        base.ExitMode();
    }

}
