using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        EditHelper.HideAllCenterPoints();
        EditHelper.CollectAllCube_RefreshColorAndName_ClearInvalidTrigger();
        foreach (var cube in CubeCombiner.Instance.cubes)
        {
            cube.InitMoveAndRotate();
        }
        LevelManager.Instance.Initialize();
        UIManager.Instance.Initialize();
        Player.Instance.Initialize();
        PathFinder.Instance.Initialize();
    }
}
