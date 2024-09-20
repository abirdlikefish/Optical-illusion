using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTriggerMoveCube : MyTrigger
{
    List<Vector3> froms = new();
    [SerializeField]
    Vector3 delta;
    [SerializeField]
    float moveSpeed = 0.2f;
    void Start()
    {
        foreach (var effectCube in effectCubes)
            froms.Add(effectCube.transform.position);
    }
    // Update is called once per frame
    void Update()
    {
        if (UIManager.Instance.IsUIBusy)
            return;
        bool busy = false;
        MyTriggerManager.Instance.busyMoves.Add(this);
        for (int i = 0; i < effectCubes.Count; i++)
        {
            Vector3 target = triggered ? froms[i] + delta : froms[i];
            effectCubes[i].transform.localPosition = Vector3.MoveTowards(effectCubes[i].transform.localPosition, target, Time.deltaTime * moveSpeed);
            if (effectCubes[i].transform.localPosition != target)
            {
                busy = true;
            }
        }
        if(!busy)
        {
            MyTriggerManager.Instance.busyMoves.Remove(this);
        }
    }

    
}
