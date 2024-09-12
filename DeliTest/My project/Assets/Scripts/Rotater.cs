using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField]
    float rotateSpeed = 1.0f;
    
    void Update()
    {
        HandleMouseInput();
    }
    void HandleMouseInput()
    {
        //rotate the object based on the mouse input

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        if(Input.GetMouseButton(0))
        {
            transform.Rotate(Vector3.up, -mouseX * rotateSpeed, Space.World);
            transform.Rotate(Vector3.right, mouseY * rotateSpeed, Space.World);
        }
    }
}
