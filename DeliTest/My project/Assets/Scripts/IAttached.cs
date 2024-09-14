using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttached
{
    abstract CenterPoint GetCurCenter();
    abstract void SetCenter(CenterPoint center);
}
