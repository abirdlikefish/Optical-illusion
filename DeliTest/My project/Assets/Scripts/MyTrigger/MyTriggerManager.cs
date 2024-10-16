using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTriggerManager : Singleton<MyTriggerManager>
{
    public MyTriggerMoveCube prefabMove;
    public MyTriggerRotateCube prefabRotate;

    public HashSet<Cube> busyMoves = new();
    public HashSet<Cube> busyRotates = new();

}
