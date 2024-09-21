using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTriggerRotateCube : MyTrigger
{
    public override void DoTrigger()
    {
        base.DoTrigger();
        foreach (var effectCube in effectCubes)
        {
            effectCube.rotateTriggered = true;
        }
    }
}
