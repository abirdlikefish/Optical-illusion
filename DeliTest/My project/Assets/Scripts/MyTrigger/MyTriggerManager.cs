using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTriggerManager : Singleton<MyTriggerManager>
{
    public MyTriggerMoveCube prefabMove;
    public MyTriggerRotateCube prefabRotate;

    public HashSet<MyTriggerMoveCube> busyMoves = new();
    public HashSet<MyTriggerRotateCube> busyRotates = new();

}
