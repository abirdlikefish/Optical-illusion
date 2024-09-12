using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveManager
{
    LevelData levelData;
    CubeManager cubeManager;

    public void Init(CubeManager cubeManager)
    {
        this.cubeManager = cubeManager;
    }
    public bool LoadData()
    {
        if(File.Exists("./levelData.json"))
        {
            StreamReader sr = new StreamReader("./levelData.json");
            string jsonString = sr.ReadToEnd();
            sr.Close();

            levelData = JsonUtility.FromJson<LevelData>(jsonString);
            Debug.Log("load data \" ./levelData.json \" succeed");
            return true;
        }
        else
        {
            Debug.Log("failed to find levelData.json");

            levelData = new LevelData();
            string jsonString = JsonUtility.ToJson(levelData);
            StreamWriter sw = new StreamWriter("./levelData.json");
            sw.Write(jsonString);
            sw.Close();
            LoadData();
            return false;
        }
    }
    public bool SaveData()
    {
        string jsonString = JsonUtility.ToJson(levelData);
        StreamWriter sw = new StreamWriter("./levelData.json");
        sw.Write(jsonString);
        sw.Close();
        Debug.Log("save data succeed");
        return true;
    }
    public Player InitCube()
    {
        Debug.Log("InitCube beg");
        for(int i = 0; i < levelData.cubesPos_x.Count; i++)
        {
            //int x = levelData.cubesPos_x[i];
            //int y = levelData.cubesPos_y[i];
            Vector3Int midPos = new Vector3Int(levelData.cubesPos_x[i], levelData.cubesPos_y[i], levelData.cubesPos_z[i]);
            cubeManager.AddCube(midPos);
            //Debug.Log(i + ": pos = " + midPos);
        }
        return cubeManager.SetBegCube(new Vector3Int(levelData.begPos_x, levelData.begPos_y, levelData.begPos_z));
    }
}

//0,0,0,0,0,-4,-3,-2,-1,0,1,1,1,1,1,1
//0,-1,-2,-3,-4,-4,-4,-4,-4,-4,-4,-4,-4,-4,-4,-4
//0,0,0,0,0,-5,-5,-5,-5,-5,-5,-4,-3,-2,-1,0

    //{"begPos_x":0,"begPos_y":0,"begPos_z":0,"cubesPos_x":[0,1,2,3,4,4,4,4,4,4],"cubesPos_y":[0,0,0,0,0,0,0,0,0,1],"cubesPos_z":[0,0,0,0,0,1,2,3,4,4]}