using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Threading;

public class SaveManager
{
    LevelData[] levelDataList;
    static string filePath = "./LevelData/levelData_";
    public void Init()
    {
        levelDataList = new LevelData[100];
        // Debug.Log("saveManager init");
    }
    public int LoadData_All()
    {
        int num = 0;
        for(int i = 0 ; i < 100 ; i ++)
        {
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
        // Debug.Log(index);
            levelDataList[index] = midLevelData;
            Debug.Log("load data \" " + midPath + " \" succeed");
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SaveData_All()
    {
        for(int i = 0 ; i < 100 ; i++)
        {
            // SaveDataByIndex(i);
            Command_normal.SaveDataByIndex(i);
        }
        return true;
    }
    public bool SaveDataByIndex(int index)
    {
        if(levelDataList[index] == null)
        {
            return false;
        }
        Command_normal.WriteCubeListToLevelData(index);
        string jsonString = JsonUtility.ToJson(levelDataList[index]);
        string midPath = filePath + index;
        StreamWriter sw = new StreamWriter(midPath);
        sw.Write(jsonString);
        sw.Close();
        Debug.Log("save data \" " + midPath + " \" succeed");
        return true;
    }
    public void CleanLevelData(int index)
    {
        levelDataList[index].cubesPos_x.Clear();
        levelDataList[index].cubesPos_y.Clear();
        levelDataList[index].cubesPos_z.Clear();
    }
    public bool WriteCubeToLevelData(int index , int x , int y , int z )
    {
        levelDataList[index].cubesPos_x.Add(x);
        levelDataList[index].cubesPos_y.Add(y);
        levelDataList[index].cubesPos_z.Add(z);
        return true;
    }
    public bool WriteBegPosToLevelData(int index , int x , int y , int z)
    {
        levelDataList[index].begPos_x = x;
        levelDataList[index].begPos_y = y;
        levelDataList[index].begPos_z = z;
        return true;
    }

    public int CreateLevelDataByIndex(int index)
    {
        levelDataList[index] = new LevelData();
        // SaveDataByIndex(index);
        Debug.Log("create data " + index + " succeed");
        Command_normal.UseLevelData(index);
        return index;
    }
    public int CreateLevelData()
    {
        for(int i = 0 ; i < 100 ; i++)
        {
            if(levelDataList[i] == null)
                return CreateLevelDataByIndex(i);
        }
        return -1;
    }
    public bool InitMap(int index)
    {
        // Debug.Log("InitCube beg");
        for(int i = 0; i < levelDataList[index].cubesPos_x.Count; i++)
        {
            Vector3Int midPos = new Vector3Int(levelDataList[index].cubesPos_x[i], levelDataList[index].cubesPos_y[i], levelDataList[index].cubesPos_z[i]);
            Command_normal.AddCube(midPos);
        }
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