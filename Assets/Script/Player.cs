using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Stack<Vector3Int> target = new Stack<Vector3Int>();
    public float waitTime = 0.1f;
    public float lastMoveTime;
    public bool isMoving;
    public Vector3Int pos;
    // CameraGridManager cameraGridManager;
    public void Init(Vector3Int pos)
    {
        SetPosition(pos);
        isMoving = false;
    }
    public void AddTarget(Vector3Int pos)
    {
        //target.Enqueue(pos);
        target.Push(pos);
    }
    // public void AddTarget(Vector2Int pos)
    // {
    //     target.Push(new Vector3Int(pos.x, 0, pos.y) - CameraGridManager.offset);
    // }
    // public void CleanTarget()
    // {
    //     target.Clear();
    // }
    void SetPosition(Vector3Int pos)
    {
        transform.position = pos + Vector3Int.up;
        this.pos = pos;
    }
    void Update()
    {
        if(isMoving)
        {
            if (Time.time - lastMoveTime < waitTime)
                return;
            if (target.Count == 0)
            {
                isMoving = false;
                Command_normal.Arrive();
                return;
            }
            SetPosition(target.Pop());
            lastMoveTime = Time.time;
            return;
        }
        else
        {
            // if(target.Count > 0)
            // {
            //     isMoving = true;
            // }
        }
    }
}
