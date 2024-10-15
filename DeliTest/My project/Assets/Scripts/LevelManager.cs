using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : Singleton<LevelManager>
{
    public LevelData curLevel;
    public bool pass;
    public void Initialize()
    {
        pass = false;
        EditHelper.HideAllCenterPoints();
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
    public CenterPoint staCenter;
    public CenterPoint desCenter;
    public List<Cube> cubes;
    //public List<MyTrigger> myTriggers;
}