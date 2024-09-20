using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttached
{
    CenterPoint GetCurCenter();
    void SetCurCenter(CenterPoint centerPoint);
}
