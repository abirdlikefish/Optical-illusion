using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SaveAll : MonoBehaviour
{
    Button button;
    private void Awake()
    {
        button = transform.GetComponent<Button>();
        button.onClick.AddListener(Clicked);
    }

    public void Clicked()
    {
        Command_normal.SaveData_All();
    }
}