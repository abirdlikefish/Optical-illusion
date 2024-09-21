using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class MyTriggerMoveCube : MyTrigger
{
    public override void DoTrigger()
    {
        base.DoTrigger();
        foreach (var effectCube in effectCubes)
            effectCube.moveTriggered = !effectCube.moveTriggered;
    }
}
