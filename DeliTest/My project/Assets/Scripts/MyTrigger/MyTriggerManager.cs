using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTriggerManager : Singleton<MyTriggerManager>
{
    public MyTriggerMoveCube prefabMove;
    public MyTriggerRotateCube prefabRotate;

    public List<MyTriggerMoveCube> busyMoves = new();
    public List<MyTriggerRotateCube> busyRotates = new();

}
