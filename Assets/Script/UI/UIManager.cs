using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager
{
    LevelSelection levelSelection;
    GameObject panel_playMode;
    GameObject panel_editMode;
    GameObject panel_allMode;

    UI_Pos_X ui_Pos_X;
    UI_Pos_Y ui_Pos_Y;
    UI_Pos_Z ui_Pos_Z;

    UI_Parament[] ui_Paraments ;

    public void Init()
    {
        levelSelection = GameObject.Find("LevelSelection").GetComponent<LevelSelection>();
        levelSelection.Init();
        panel_playMode = GameObject.Find("Panel_playMode");
        panel_editMode = GameObject.Find("Panel_editMode");
        panel_allMode = GameObject.Find("Panel_allMode");

        ui_Pos_X = GameObject.Find("Position_X").GetComponent<UI_Pos_X>();
        ui_Pos_Y = GameObject.Find("Position_Y").GetComponent<UI_Pos_Y>();
        ui_Pos_Z = GameObject.Find("Position_Z").GetComponent<UI_Pos_Z>();

        ui_Paraments = new UI_Parament[9];
        for(int i = 0 ; i < 9 ; i ++)
        {
            // Debug.Log("parament index: " + i);
            ui_Paraments[i] = GameObject.Find("Parament_" + i.ToString()).GetComponent<UI_Parament>();
        }
        

        panel_allMode.SetActive(true);
    }

    public void AddLevel(int index)
    {
        levelSelection.AddOption(index);
    }
    public void RemoveLevel(int index)
    {
        levelSelection.RemoveOption(index);
    }

    public Vector3Int GetInputPos()
    {
        return new Vector3Int(ui_Pos_X.Pos_x , ui_Pos_Y.Pos_y , ui_Pos_Z.Pos_z);
    }
    public void SetInputPos(Vector3Int pox)
    {
        ui_Pos_X.Pos_x = pox.x;
        ui_Pos_Y.Pos_y = pox.y;
        ui_Pos_Z.Pos_z = pox.z;

    }

    public Vector3Int GetParament_Vector3(int index)
    {
        return new Vector3Int(ui_Paraments[index * 3 + 0 ].Num , ui_Paraments[index * 3 + 1 ].Num , ui_Paraments[index * 3 + 2 ].Num );
    }

    public void UseUI(Mode.ModeName modeName)
    {
        if(modeName == Mode.ModeName.PlayMode)
        {
            panel_playMode.SetActive(true);
            panel_editMode.SetActive(false);
        }
        else
        {
            panel_playMode.SetActive(false);
            panel_editMode.SetActive(true);
        }
    }
}


