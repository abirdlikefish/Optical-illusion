using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public int begPos_x;
    public int begPos_y;
    public int begPos_z;
    public List<int> cubesPos_x;
    public List<int> cubesPos_y;
    public List<int> cubesPos_z;
    public LevelData()
    {
        cubesPos_x = new List<int>();
        cubesPos_y = new List<int>();
        cubesPos_z = new List<int>();
        cubesPos_x.Add(0);
        cubesPos_y.Add(0);
        cubesPos_z.Add(0);
    }
}
