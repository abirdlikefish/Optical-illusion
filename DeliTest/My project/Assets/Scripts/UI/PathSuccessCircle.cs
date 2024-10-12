using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PathSuccessCircle : MonoBehaviour
{
    //利用DoTween每一帧旋转
    private void Awake()
    {
        transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
    }
}
