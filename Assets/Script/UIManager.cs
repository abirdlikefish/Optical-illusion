using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    LevelSelection levelSelection;
    public void Init()
    {
        levelSelection = GameObject.Find("LevelSelection").GetComponent<LevelSelection>();
        levelSelection.Init();
    }

    public void AddLevel(int index)
    {
        levelSelection.AddOption(index);
    }
    public void RemoveLevel(int index)
    {
        levelSelection.RemoveOption(index);
    }
}


