using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    void Start()
    {
        transform.LookAt(transform.position + new Vector3(-1, -1, -1));
    }
    void Update()
    {
        
    }
}
