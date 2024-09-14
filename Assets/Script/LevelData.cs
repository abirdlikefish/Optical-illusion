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
    public List<int> cubeRotatablesLen_0 = new List<int>();
    public List<int> cubeRotatablesLen_1 = new List<int>();
    public List<int> cubeRotatablesLen_2 = new List<int>();
    public List<int> cubeRotatablesLen_3 = new List<int>();
    public List<int> cubeRotatablesLen_4 = new List<int>();
    public List<int> cubeRotatablesLen_5 = new List<int>();
    public List<int> cubeRotatablesTowards_0 = new List<int>();
    public List<int> cubeRotatablesTowards_1 = new List<int>();
    public List<int> cubeRotatablesTowards_2 = new List<int>();
    public List<int> cubeRotatablesTowards_3 = new List<int>();

    
    public List<int> cubeMovablesPos_x = new List<int>();
    public List<int> cubeMovablesPos_y = new List<int>();
    public List<int> cubeMovablesPos_z = new List<int>();
    public List<int> cubeMovablesLen_0 = new List<int>();
    public List<int> cubeMovablesLen_1 = new List<int>();
    public List<int> cubeMovablesLen_2 = new List<int>();
    public List<int> cubeMovablesLen_3 = new List<int>();
    public List<int> cubeMovablesLen_4 = new List<int>();
    public List<int> cubeMovablesLen_5 = new List<int>();
    public List<int> cubeMovablesMoveDir = new List<int>();
    public List<int> cubeMovablesMaxMoveDistance = new List<int>();
    public LevelData()
    {
        begPos_x = 0;
        begPos_y = 0;
        begPos_z = 0;
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
        cubeRotatablesLen_0.Clear();
        cubeRotatablesLen_1.Clear();
        cubeRotatablesLen_2.Clear();
        cubeRotatablesLen_3.Clear();
        cubeRotatablesLen_4.Clear();
        cubeRotatablesLen_5.Clear();
        cubeRotatablesTowards_0.Clear();
        cubeRotatablesTowards_1.Clear();
        cubeRotatablesTowards_2.Clear();
        cubeRotatablesTowards_3.Clear();
        
        cubeMovablesPos_x.Clear();
        cubeMovablesPos_y.Clear();
        cubeMovablesPos_z.Clear();
        cubeMovablesLen_0.Clear();
        cubeMovablesLen_1.Clear();
        cubeMovablesLen_2.Clear();
        cubeMovablesLen_3.Clear();
        cubeMovablesLen_4.Clear();
        cubeMovablesLen_5.Clear();
        cubeMovablesMoveDir.Clear();
        cubeMovablesMaxMoveDistance.Clear();

    }
}
