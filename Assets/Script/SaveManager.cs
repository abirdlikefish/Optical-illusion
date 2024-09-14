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
            levelDataList[index] = midLevelData;
            Debug.Log("load data \" " + midPath + " \" succeed");
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SaveDataByIndex(int index)
    {
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
        levelDataList[index].Clean();
    }
    public bool WriteCubeToLevelData(int index , int x , int y , int z )
    {
        levelDataList[index].cubesPos_x.Add(x);
        levelDataList[index].cubesPos_y.Add(y);
        levelDataList[index].cubesPos_z.Add(z);
        return true;
    }
    public bool WriteCubeRotatableToLevelData(int index , Vector3Int pos , int[] len , int axisIndex , int possibleAngle )
    {
        levelDataList[index].cubeRotatablesPos_x.Add(pos.x);
        levelDataList[index].cubeRotatablesPos_y.Add(pos.y);
        levelDataList[index].cubeRotatablesPos_z.Add(pos.z);
        levelDataList[index].cubeRotatablesLen_0.Add(len[0]);
        levelDataList[index].cubeRotatablesLen_1.Add(len[1]);
        levelDataList[index].cubeRotatablesLen_2.Add(len[2]);
        levelDataList[index].cubeRotatablesLen_3.Add(len[3]);
        levelDataList[index].cubeRotatablesLen_4.Add(len[4]);
        levelDataList[index].cubeRotatablesLen_5.Add(len[5]);
        levelDataList[index].cubeRotatablesAxisIndex.Add(axisIndex);
        levelDataList[index].cubeRotatablesPossibleAngle.Add(possibleAngle);
        // levelDataList[index].cubeRotatablesTowards_0.Add(towards[0]);
        // levelDataList[index].cubeRotatablesTowards_1.Add(towards[1]);
        // levelDataList[index].cubeRotatablesTowards_2.Add(towards[2]);
        // levelDataList[index].cubeRotatablesTowards_3.Add(towards[3]);
        return true;
    }
    public bool WriteCubeMovableToLevelData(int index , Vector3Int pos , int[] len , int moveDir , int maxMoveDistance )
    {
        levelDataList[index].cubeMovablesPos_x.Add(pos.x);
        levelDataList[index].cubeMovablesPos_y.Add(pos.y);
        levelDataList[index].cubeMovablesPos_z.Add(pos.z);
        levelDataList[index].cubeMovablesLen_0.Add(len[0]);
        levelDataList[index].cubeMovablesLen_1.Add(len[1]);
        levelDataList[index].cubeMovablesLen_2.Add(len[2]);
        levelDataList[index].cubeMovablesLen_3.Add(len[3]);
        levelDataList[index].cubeMovablesLen_4.Add(len[4]);
        levelDataList[index].cubeMovablesLen_5.Add(len[5]);
        levelDataList[index].cubeMovablesMoveDir.Add(moveDir);
        levelDataList[index].cubeMovablesMaxMoveDistance.Add(maxMoveDistance);
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
        if(levelDataList[index] == null)
        {
            levelDataList[index] = new LevelData();
        }
        else
        {
            CleanLevelData(index);
        }
        Debug.Log("create " + index + " data succeed");
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
        for(int i = 0 ; i < levelDataList[index].cubeRotatablesPos_x.Count ; i ++)
        {
            Vector3Int midPos = new Vector3Int(levelDataList[index].cubeRotatablesPos_x[i] , levelDataList[index].cubeRotatablesPos_y[i] , levelDataList[index].cubeRotatablesPos_z[i]);
            int[] len = new int[6]{ levelDataList[index].cubeRotatablesLen_0[i] , levelDataList[index].cubeRotatablesLen_1[i] , levelDataList[index].cubeRotatablesLen_2[i] , 
                                    levelDataList[index].cubeRotatablesLen_3[i] , levelDataList[index].cubeRotatablesLen_4[i] , levelDataList[index].cubeRotatablesLen_5[i]};
            // int[] midTowards = new int[4]{levelDataList[index].cubeRotatablesTowards_0[i] , levelDataList[index].cubeRotatablesTowards_1[i] , levelDataList[index].cubeRotatablesTowards_2[i] , levelDataList[index].cubeRotatablesTowards_3[i]};

            // Command_normal.AddCube_Rotatable(midPos , len , midTowards);
            Command_normal.AddCube_Rotatable(midPos , len , levelDataList[index].cubeRotatablesAxisIndex[i] , levelDataList[index].cubeRotatablesPossibleAngle[i]);
        }
        for(int i = 0 ; i < levelDataList[index].cubeMovablesPos_x.Count ; i ++)
        {
            Vector3Int midPos = new Vector3Int(levelDataList[index].cubeMovablesPos_x[i] , levelDataList[index].cubeMovablesPos_y[i] , levelDataList[index].cubeMovablesPos_z[i]);
            int[] len = new int[6]{ levelDataList[index].cubeMovablesLen_0[i] , levelDataList[index].cubeMovablesLen_1[i] , levelDataList[index].cubeMovablesLen_2[i] , 
                                    levelDataList[index].cubeMovablesLen_3[i] , levelDataList[index].cubeMovablesLen_4[i] , levelDataList[index].cubeMovablesLen_5[i]};
            Command_normal.AddCube_Movable(midPos , len , levelDataList[index].cubeMovablesMoveDir[i] , levelDataList[index].cubeMovablesMaxMoveDistance[i]);
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
