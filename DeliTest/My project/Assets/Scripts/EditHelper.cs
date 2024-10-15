using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditHelper : MonoBehaviour
{
    public static void CollectAllCube_RefreshColorAndName_ClearInvalidTrigger()
    {
        CubeCombiner.Instance.CollectCubeAndCenter();
        foreach (var cube in CubeCombiner.Instance.cubes)
        {
            cube.RefreshColorAndName();
            foreach(var centerPoint in cube.centerPoints)
            {
                centerPoint.ClearInValidTrigger();
            }
        }
    }
    public static void AllCubeMagnetPos()
    {
        foreach (var cube in CubeCombiner.Instance.cubes)
            cube.MagnetPos();
    }
    public static void ShowAllCenterPoints()
    {
        foreach (var cube in CubeCombiner.Instance.cubes)
        {
            foreach (var centerPoint in cube.centerPoints)
            {
                centerPoint.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }
    public static void HideAllCenterPoints()
    {
        foreach (var cube in CubeCombiner.Instance.cubes)
        {
            foreach (var centerPoint in cube.centerPoints)
            {
                centerPoint.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
