
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attached : MonoBehaviour
{
    private void OnTransformChildrenChanged()
    {
        foreach(var center in transform.parent.GetComponent<Cube>().centerPoints)
        {
            center.ClearInvalidTrigger();
        }
    }
}
