using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.ShaderData;


public class LevelManager : Singleton<LevelManager>, ISave
{
    public LevelData curLevel;
    public bool pass = false;
    public void LoadData()
    {
        if(JsonIO.ReadCurSheet<LevelData>("Mapdata","0") == null)
        {
            Debug.LogError("load failed");
        }
        curLevel = JsonIO.ReadCurSheet<LevelData>("Mapdata", "0");
        for (int i=0;i<CubeCombiner.Instance.transform.childCount;i++)
        {
            CubeCombiner.Instance.transform.GetChild(i).gameObject.SetActive(false);
        }
        CubeCombiner.Instance.cubes = curLevel.cubes;
        for (int i = 0; i < curLevel.cubes.Count; i++)
        {
            Instantiate(curLevel.cubes[i], CubeCombiner.Instance.transform);
        }
    }

    public void SaveData()
    {
        curLevel.cubes = CubeCombiner.Instance.cubes;
        JsonIO.WriteCurSheet("Mapdata", "0", curLevel);
    }
    public void PassCurLevel()
    {
        pass = true;
        UIManager.Instance.PassCurLevel();
    }

    public void TryLoadDeltaLevel(int delta)
    {
        if(delta == 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else
        {
            if (SceneManager.GetActiveScene().buildIndex + delta >= 0 && SceneManager.GetActiveScene().buildIndex + delta < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + delta);
        }
    }
}

    [Serializable]
public class LevelData
{
    public int levelId;
    public string levelName;
    public string levelDescription = "666";
    public CenterPoint initCenter;
    public CenterPoint endCenter;
    public List<Cube> cubes;
    //public List<MyTrigger> myTriggers;
}