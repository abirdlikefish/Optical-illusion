using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Rotater : Singleton<Rotater>
{
    [SerializeField]
    List<CenterPointPair> nearestPairs = new();
    public bool magneting;

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

        if (PlatformManager.IsMobile() && Config.Instance.isEditorMobileTest)
        {
            if (UIManager.Instance.isHoldRotate)
            {
                magneting = false;
                HandleGyroscopeInput();
            }
            else
            {
                Magnet();
            }
            lastQuaternion = Input.gyro.attitude;
            lastEuler = lastQuaternion.eulerAngles;
        }
        else if (PlatformManager.IsPC())
        {
            if (Input.GetMouseButton(1))
            {
                magneting = false;
                HandleMouseInput();
            }
            else
            {
                Magnet();
            }
        }
    }
    Vector2 mouseMove;
    void HandleMouseInput()
    {
        if (Player.Instance.IsBusy() && !Config.Instance.canRotateWhilePlayerMove)
            return;
        mouseMove.x = Input.GetAxis("Mouse X");
        mouseMove.y = Input.GetAxis("Mouse Y");
        transform.Rotate(Vector3.up, -mouseMove.x * Config.Instance.mouseRotateSpeed, Space.World);
        transform.Rotate(Vector3.right, mouseMove.y * Config.Instance.mouseRotateSpeed, Space.World);
    }
    public Vector3 userRotate;
    public Vector3 gravity;
    public Vector3 acceleration;
    public Quaternion lastQuaternion;
    public Vector3 lastEuler;

    void HandleGyroscopeInput()//Gyroscope ： 陀螺仪
    {
        userRotate = Input.gyro.rotationRate;
        gravity = Input.gyro.gravity;
        acceleration = Input.acceleration;
        if (Player.Instance.IsBusy() && !Config.Instance.canRotateWhilePlayerMove)
            return;
        if(lastEuler == null)
        {
            lastEuler = Input.gyro.attitude.eulerAngles;
            return;
        }
        transform.Rotate(Vector3.right,     userRotate.x * Config.Instance.reverseGyro.x * Config.Instance.gyroSpeed.x * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.up,        userRotate.y * Config.Instance.reverseGyro.y * Config.Instance.gyroSpeed.y * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.forward,   userRotate.z * Config.Instance.reverseGyro.z * Config.Instance.gyroSpeed.z * Time.deltaTime, Space.World);
        //transform.Rotate(new (userRotate.x*reverseGryoX,userRotate.y*reverseGryoY,userRotate.z*reverseGryoZ));
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
            if (!pair.canMagnet)
                continue;
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
