using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PathSuccessCircle : MonoBehaviour
{
    //����DoTweenÿһ֡��ת
    private void Awake()
    {
        transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
    }
}
