using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Rotater : Singleton<Rotater>
{
    [SerializeField]
    List<CenterPointPair> nearestPairs = new();
    public bool magneting;
    public bool reverseGryoX = false;
    public bool reverseGryoY = false;
    public bool reverseGryoZ = false;
    public float rotateSpeed = 1.0f;
    public bool isEditorMobileTest = true;
    private void Awake()
    {
        if (PlatformManager.IsMobile())
        {
            Input.gyro.enabled = true;
        }
    }
    void Update()
    {
        if (UIManager.Instance.IsUIBusy)
            return;

        if (PlatformManager.IsMobile() && isEditorMobileTest)
        {
            if (UIManager.Instance.isHoldRotate)
            {
                HandleGyroscopeInput();
                return;
            }
            lastEulerAngle = Input.gyro.attitude.eulerAngles;
            Magnet();
        }
        if (PlatformManager.IsPC())
        {
            HandleMouseInput();
            if (!Input.GetMouseButton(1))
                Magnet();
        }
    }
    public Vector2 mouseMove;
    void HandleMouseInput()
    {
        if (Player.Instance.IsBusy() && !Config.Instance.canRotateWhilePlayerMove)
            return;
        mouseMove.x = Input.GetAxis("Mouse X");
        mouseMove.y = Input.GetAxis("Mouse Y");
        //float mouseX = Input.GetAxis("Mouse X");
        //float mouseY = Input.GetAxis("Mouse Y");
        if(Input.GetMouseButton(1))
        {
            magneting = false;
            transform.Rotate(Vector3.up, -mouseMove.x * Config.Instance.mouseRotateSpeed, Space.World);
            transform.Rotate(Vector3.right, mouseMove.y * Config.Instance.mouseRotateSpeed, Space.World);
        }
    }

    Vector3 lastEulerAngle;
    void HandleGyroscopeInput()//Gyroscope ： 陀螺仪
    {
        if (Player.Instance.IsBusy() && !Config.Instance.canRotateWhilePlayerMove)
            return;
        //读取相邻两帧陀螺仪旋转角
        if(lastEulerAngle == null)
        {
            lastEulerAngle = Input.gyro.attitude.eulerAngles;
            return;
        }    
        Vector3 thisEulerAngle = Input.gyro.attitude.eulerAngles;
        Vector3 deltaEulerAngle = thisEulerAngle - lastEulerAngle;
        lastEulerAngle = thisEulerAngle;
        //Debug.Log("thisEulerAngle:" + thisEulerAngle);
        //Debug.Log("lastEulerAngle:" + lastEulerAngle);
        //Debug.Log("deltaEulerAngle:" + deltaEulerAngle);
        int reverseX = reverseGryoX ? -1 : 1;
        int reverseY = reverseGryoY ? -1 : 1;


        //transform.rotation = Quaternion.Euler(deltaEulerAngle) * transform.rotation;
        transform.Rotate(Vector3.up, -deltaEulerAngle.x * reverseX * rotateSpeed, Space.World);
        transform.Rotate(Vector3.right, deltaEulerAngle.y * reverseY * rotateSpeed, Space.World);
        transform.Rotate(Vector3.forward, deltaEulerAngle.z * rotateSpeed, Space.World);
        //transform.Rotate(-deltaEulerAngle * reverse * rotateSpeed, Space.World);

    }
    public Vector3 GetInitDelta(CenterPoint c1, CenterPoint c2)
    {
        return c2.cube.transform.localPosition + c2.transform.localPosition - c1.cube.transform.localPosition - c1.transform.localPosition;
    }
    float DisSquare(Vector2 p1, Vector2 p2)
    {
        return (p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y);
    }
    [SerializeField]
    Quaternion newRotation;
    void Magnet()
    {
        if (magneting)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, Config.Instance.magnetRotateSpeed);
            if(Quaternion.Angle(transform.rotation,newRotation) <= 0.01)
            {
                magneting = false;
            }
            return;
        }

        float nearestDis = float.MaxValue;
        nearestPairs.Clear();
        foreach(var pair in CubeCombiner.Instance.centerPointPairs)
        {
            float curDis = DisSquare(Camera.main.WorldToScreenPoint(pair.first.transform.position), Camera.main.WorldToScreenPoint(pair.second.transform.position));
            if (curDis < nearestDis && !CubeCombiner.Instance.IsCubeNear(pair.first,pair.second))
            {
                nearestDis = curDis;
                nearestPairs.Clear();
                nearestPairs.Add(pair);
            }
            else if (DisSquare(Camera.main.WorldToScreenPoint(pair.first.transform.position), Camera.main.WorldToScreenPoint(pair.second.transform.position)) == nearestDis)
            {
                nearestPairs.Add(pair);
            }

        }
        if(nearestPairs.Count != 0)
        {
            Vector3 delta = nearestPairs[0].second.transform.position - nearestPairs[0].first.transform.position;
            Quaternion currentRotation = transform.rotation;
            Quaternion addedRotation = Quaternion.FromToRotation(delta, Vector3.forward);
            newRotation = addedRotation * currentRotation;
            if (Quaternion.Angle(transform.rotation, newRotation) <= 0.05)
                return;
            //Debug.Log("Magnet BECAUSE:" + nearestPairs[0].first.name + " & " + nearestPairs[0].second.name + " are near!");
            magneting = MyTriggerManager.Instance.busyMoves.Count == 0;
        }
    }

}
