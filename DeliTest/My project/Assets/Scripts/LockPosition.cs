using UnityEngine;

[ExecuteInEditMode]
public class LockPosition : MonoBehaviour
{
    public bool lockPosition = true;
    public bool lockRotation = true;
    public bool lockScale = true;

    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;
    private Vector3 initialLocalScale;

    void Start()
    {
        //initialLocalPosition = transform.localPosition;
        //initialLocalRotation = transform.localRotation;
        //initialLocalScale = transform.localScale;
    }

    void Update()
    {
        //if (lockPosition)
        //{
        //    transform.localPosition = initialLocalPosition;
        //}
        //if (lockRotation)
        //{
        //    transform.localRotation = initialLocalRotation;
        //}
        //if (lockScale)
        //{
        //    transform.localScale = initialLocalScale;
        //}
    }
}
