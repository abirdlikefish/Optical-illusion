using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Pos_Z : MonoBehaviour
{
    InputField inputField;
    public int Pos_z{get {return int.Parse(inputField.text);} set {inputField.text = value.ToString();} }
    void Awake()
    {
        inputField = transform.GetComponent<InputField>();
        Pos_z = 0;
    }
}
