using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTrigger : Busy
{
    public List<Cube> effectCubes = new();

    [SerializeField]
    protected bool triggered = false;
    public virtual void DoTrigger()
    {
        triggered = !triggered;
    }
}
