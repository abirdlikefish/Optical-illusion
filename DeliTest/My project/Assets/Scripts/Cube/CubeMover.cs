using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMover : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    float moveSpeed = 0.2f;
    //private void Awake()
    //{
    //    target = transform;
    //}
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
        //为什么几乎不移动

    }
}
