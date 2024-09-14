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
    public List<int> cubesPos_x = new List<int>();
    public List<int> cubesPos_y = new List<int>();
    public List<int> cubesPos_z = new List<int>();

    public List<int> cubeRotatablesPos_x = new List<int>();
    public List<int> cubeRotatablesPos_y = new List<int>();
    public List<int> cubeRotatablesPos_z = new List<int>();
    public List<int> cubeRotatablesLen = new List<int>();
    public List<int> cubeRotatablesTowards_0_x = new List<int>();
    public List<int> cubeRotatablesTowards_0_y = new List<int>();
    public List<int> cubeRotatablesTowards_0_z = new List<int>();
    public List<int> cubeRotatablesTowards_1_x = new List<int>();
    public List<int> cubeRotatablesTowards_1_y = new List<int>();
    public List<int> cubeRotatablesTowards_1_z = new List<int>();
    public List<int> cubeRotatablesTowards_2_x = new List<int>();
    public List<int> cubeRotatablesTowards_2_y = new List<int>();
    public List<int> cubeRotatablesTowards_2_z = new List<int>();
    public LevelData()
    {
        // cubesPos_x = new List<int>();
        // cubesPos_y = new List<int>();
        // cubesPos_z = new List<int>();
        cubesPos_x.Add(0);
        cubesPos_y.Add(0);
        cubesPos_z.Add(0);
    }
    public void Clean()
    {
        cubesPos_x.Clear();
        cubesPos_y.Clear();
        cubesPos_z.Clear();
        cubeRotatablesPos_x.Clear();
        cubeRotatablesPos_y.Clear();
        cubeRotatablesPos_z.Clear();
        cubeRotatablesLen.Clear();
        cubeRotatablesTowards_0_x.Clear();
        cubeRotatablesTowards_0_y.Clear();
        cubeRotatablesTowards_0_z.Clear();
        cubeRotatablesTowards_1_x.Clear();
        cubeRotatablesTowards_1_y.Clear();
        cubeRotatablesTowards_1_z.Clear();
        cubeRotatablesTowards_2_x.Clear();
        cubeRotatablesTowards_2_y.Clear();
        cubeRotatablesTowards_2_z.Clear();

    }
}
