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
        else if (Input.GetMouseButtonDown(0))
        {
            Command_normal.MouseSelected();
        }
        else if(Input.GetKeyDown(KeyCode.Delete))
        {
            Command_normal.DeleteCube();
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            Command_normal.AddCube();
        }
    }

    public override void ExitMode()
    {
        
        base.ExitMode();
    }

}
