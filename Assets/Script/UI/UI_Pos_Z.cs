using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Pos_Z : MonoBehaviour
{
    InputField inputField;
    public int Pos_z()
    {
        return int.Parse(inputField.text);
    }
    void Awake()
    {
        inputField = transform.GetComponent<InputField>();
    }
}
