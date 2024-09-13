using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Pos_X : MonoBehaviour
{
    InputField inputField;
    public int Pos_x()
    {
        Debug.Log("hello world!");
        Debug.Log(inputField.text);
        return int.Parse(inputField.text);
    }
    void Awake()
    {
        inputField = transform.GetComponent<InputField>();
        if(inputField == null)
        {
            Debug.Log("Input field is null");
        }
    }
    void Start()
    {

    }
}
