using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditHelper : MonoBehaviour
{
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

    //public static void RefreshAllTriggerDelta()
    //{
    //    foreach (var cube in CubeCombiner.Instance.cubes)
    //    {
    //        cube.RefreshTriggerDelta();
    //    }
    //}
}
