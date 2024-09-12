using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(transform.position + new Vector3(-1, -1, -1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
