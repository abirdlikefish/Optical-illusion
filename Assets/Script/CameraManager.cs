using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Vector3[] dir = new Vector3[2]{new Vector3(-1,-1,-1) , new Vector3(1,-1,-1)};
    public float changeTime;
    public float begTime;
    bool isChanging ;
    int viewIndex ;
    public void Init(int viewIndex)
    {
        // SetView(viewIndex);
        transform.position = dir[viewIndex] * -100;
        transform.LookAt(transform.position + dir[viewIndex]);
        changeTime = 2;
        isChanging = false;
        this.viewIndex = viewIndex;
    }
    void Update()
    {
        if(isChanging)
        {
            float k = (Time.time - begTime) / changeTime;
            if(k >= 1)
            {
                transform.position = dir[viewIndex] * -100;
                transform.LookAt(transform.position + dir[viewIndex]);
                return ;
            }
            Vector3 midDir = Vector3.Lerp(dir[viewIndex ^ 1], dir[viewIndex],k);
            transform.position = midDir * -100;
            transform.LookAt(transform.position + midDir);
            
        }
    }
    public void SetView(int index)
    {
        isChanging = true;
        begTime = Time.time;
        viewIndex = index;

    }
}
