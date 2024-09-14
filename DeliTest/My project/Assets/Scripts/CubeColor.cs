using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeColor : Singleton<CubeColor>
{
    public SerializableDictionary<Cube.COLOR, Material> color_mar;
}
