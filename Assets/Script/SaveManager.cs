using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Threading;

public class SaveManager
{
    // List<LevelData> levelDataList;
    LevelData[] levelDataList;
    static string filePath = "./LevelData/levelData_";
    // public void Init(CubeManager cubeManager)
    public void Init()
    {
        // this.cubeManager = cubeManager;
        // levelDataList = new List<LevelData>(100);
        levelDataList = new LevelData[100];
        Debug.Log("saveManager init");
    }
    public int LoadData_All()
    {
        int num = 0;
        for(int i = 0 ; i < 100 ; i ++)
        {
            // LoadDataByIndex(i);
            num += Command_normal.LoadDataByIndex(i) ? 1 : 0;
        }
        return num;
    }
    public bool LoadDataByIndex(int index)
    {
        string midPath = filePath + index;
        if(File.Exists(midPath))
        {
            StreamReader sr = new StreamReader(midPath);
            string jsonString = sr.ReadToEnd();
            sr.Close();

            LevelData midLevelData = JsonUtility.FromJson<LevelData>(jsonString);
            // levelDataList.Add(midLevelData);
        Debug.Log(index);
        // Debug.Log(levelDataList.Count);
            levelDataList[index] = midLevelData;
            Debug.Log("load data \" " + midPath + " \" succeed");
            return true;
        }
        else
        {
            return false;
        }
    }
    // public bool ReLoadData()
    // {
    //     levelDataList.Clear();
    //     return LoadData();
    // }
    public bool SaveData_All()
    {
        // for(int i = 0 ; i < levelDataList.Count ; i++)
        for(int i = 0 ; i < 100 ; i++)
        {
            SaveDataByIndex(i);
        }
        return true;
    }
    public bool SaveDataByIndex(int index)
    {
        if(levelDataList[index] == null)
        {
            return false;
        }
        string jsonString = JsonUtility.ToJson(levelDataList[index]);
        string midPath = filePath + index;
        StreamWriter sw = new StreamWriter(midPath);
        sw.Write(jsonString);
        sw.Close();
        Debug.Log("save data \" " + midPath + " \" succeed");
        return true;
    }
    public bool CreateLevelDataByIndex(int index)
    {
        levelDataList[index] = new LevelData();
        SaveDataByIndex(index);
        Debug.Log("create data " + index + " succeed");
        return true;
    }
    public bool CreateLevelData()
    {
        for(int i = 0 ; i < 100 ; i++)
        {
            if(levelDataList[i] == null)
                return CreateLevelDataByIndex(i);
        }
        return false;
        // int index = levelDataList.Count;
        // return CreateLevelDataByIndex(index);
    }
    public bool InitMap(int index)
    {
        Debug.Log("InitCube beg");
        for(int i = 0; i < levelDataList[index].cubesPos_x.Count; i++)
        {
            Vector3Int midPos = new Vector3Int(levelDataList[index].cubesPos_x[i], levelDataList[index].cubesPos_y[i], levelDataList[index].cubesPos_z[i]);
            Command_normal.AddCube(midPos);
            // cubeManager.AddCube(midPos);
        }
        // Command_normal.SetBeginPosition(new Vector3Int(levelDataList[index].begPos_x, levelDataList[index].begPos_y, levelDataList[index].begPos_z));
        
        // return cubeManager.SetBegCube(new Vector3Int(levelDataList[index].begPos_x, levelDataList[index].begPos_y, levelDataList[index].begPos_z));
        return true;
    }
    public bool SetBeginPosition(int index)
    {

        return Command_normal.SetBeginPosition(new Vector3Int(levelDataList[index].begPos_x, levelDataList[index].begPos_y, levelDataList[index].begPos_z));
    }
    public bool IsIndexExist(int index)
    {
        if(levelDataList[index] == null)
            return false;
        else
            return true;
    }
}

//0,0,0,0,0,-4,-3,-2,-1,0,1,1,1,1,1,1
//0,-1,-2,-3,-4,-4,-4,-4,-4,-4,-4,-4,-4,-4,-4,-4
//0,0,0,0,0,-5,-5,-5,-5,-5,-5,-4,-3,-2,-1,0

// {"begPos_x":0,"begPos_y":0,"begPos_z":0,"cubesPos_x":[0,1,2,3,4,4,4,4,4,4],"cubesPos_y":[0,0,0,0,0,0,0,0,0,1],"cubesPos_z":[0,0,0,0,0,1,2,3,4,4]}