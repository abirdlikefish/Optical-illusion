using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PathFailCross : MonoBehaviour
{
    public float elasticity = 1f;
    public int vibrato = 10;
    private void Awake()
    {
        transform.DOScale(0.1f,0);
        transform.DOPunchScale(Vector3.one, 0.2f, vibrato ,elasticity);
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
