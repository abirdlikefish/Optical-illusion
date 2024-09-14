using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Parament : MonoBehaviour
{
    InputField inputField;
    public int Num{get {return int.Parse(inputField.text);} set {inputField.text = value.ToString();} }
    void Awake()
    {
        inputField = transform.GetComponent<InputField>();
        Num = 0;
    }
}
