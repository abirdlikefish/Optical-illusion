using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Pos_Y : MonoBehaviour
{
    InputField inputField;
    public int Pos_y{get {return int.Parse(inputField.text);} set {inputField.text = value.ToString();} }
    void Awake()
    {
        inputField = transform.GetComponent<InputField>();
    }
}
