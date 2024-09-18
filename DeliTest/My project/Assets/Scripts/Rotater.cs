using System.Collections.Generic;
using UnityEngine;

public class Rotater : Singleton<Rotater>
{
    [SerializeField]
    List<CenterPointPair> nearestPairs = new();
    public bool magneting;
    void Update()
    {
        if (UIManager.Instance.IsUIBusy)
            return;
        HandleMouseInput();
        if(!Input.GetMouseButton(1))
            Magnet();
    }
    void HandleMouseInput()
    {
        if (Player.Instance.IsBusy() && !Config.Instance.canRotateWhilePlayerMove)
            return;
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        if(Input.GetMouseButton(1))
        {
            magneting = false;
            transform.Rotate(Vector3.up, -mouseX * Config.Instance.mouseRotateSpeed, Space.World);
            transform.Rotate(Vector3.right, mouseY * Config.Instance.mouseRotateSpeed, Space.World);
        }
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
    [SerializeField]
    Vector3 newRotationEuler;
    void Magnet()
    {
        if (magneting)
        {
            newRotationEuler = newRotation.eulerAngles;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, Config.Instance.magnetRotateSpeed);
            if(Quaternion.Angle(transform.rotation,newRotation) <= 0.01)
            {
                Debug.Log("MagnetEnd" + " transform.rotation = " + transform.rotation + " newRotation = " + newRotation);
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
            magneting = !BusyCollector.Instance.IsBusy();
        }
    }

}
