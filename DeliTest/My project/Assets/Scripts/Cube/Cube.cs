using System.Collections.Generic;
using UnityEngine;
public class Cube : MonoBehaviour
{
    [HelpBox("保证是CubeCombiner的子物体", HelpBoxType.Warning)]
    public int Index => transform.GetSiblingIndex();
    #region Color
    public enum COLOR
    {
        BLACK,
        BLUE,
        GREEN,
        RED
    };
    public COLOR color;
    ComputeBuffer computeBuffer1;
    ComputeBuffer computeBuffer2;
    ComputeBuffer l1;
    ComputeBuffer l2;
    public void Change6SideColor()
    {
        List<Vector3> deltas = new();
        List<Color> colors = new();
        foreach (var nearCube in nearCubes)
        {
            deltas.Add(nearCube.transform.position - transform.position);
            colors.Add(nearCube.trueMesh.GetComponent<MeshRenderer>().materials[0].color);
        }
        if (l1 != null && l2 != null)
        {
            l1.Dispose();
            l2.Dispose();
        }
        if (nearCubes.Count != 0)
        {
            computeBuffer1 = new ComputeBuffer(deltas.Count, sizeof(float) * 3);
            computeBuffer1.SetData(deltas.ToArray());
            computeBuffer2 = new ComputeBuffer(colors.Count, sizeof(float) * 4);
            computeBuffer2.SetData(colors.ToArray());
        }
        else
        {
            computeBuffer1 = new ComputeBuffer(1, sizeof(float) * 3);
            computeBuffer1.SetData(new Vector3[1]);
            computeBuffer2 = new ComputeBuffer(1, sizeof(float) * 4);
            computeBuffer2.SetData(new Color[1]);
        }
        trueMesh.GetComponent<MeshRenderer>().materials[0].SetBuffer("positionBuffer", computeBuffer1);
        trueMesh.GetComponent<MeshRenderer>().materials[0].SetBuffer("colorBuffer", computeBuffer2);
        l1 = computeBuffer1;
        l2 = computeBuffer2;
    }
    #endregion

    #region move & rotate
    public Vector3 moveDelta;
    Vector3 moveFrom;
    Vector3 moveTarget;
    public float moveSpeed = 2f;
    /*[HideInInspector]*/public bool moveTriggered;
    [HideInInspector] public bool moveEnd;

    public Vector3 rotateDelta;
    Vector3 rotateFrom;
    Vector3 rotateTarget;
    public float rotateSpeed = 45f;
    /*[HideInInspector] */public bool rotateTriggered;
    [HideInInspector] public bool rotateEnd;
    bool hasResetCenter = false;
    public void InitMoveAndRotate()
    {
        moveFrom = transform.localPosition;

        rotateTarget = attached.eulerAngles;
        rotateTarget = new Vector3(Mathf.Round(rotateTarget.x), Mathf.Round(rotateTarget.y), Mathf.Round(rotateTarget.z));
    }
    public void MoveAndRotate()
    {
        if (UIManager.Instance.IsUIBusy)
            return;
        MyTriggerManager.Instance.busyMoves.Add(this);
        moveTarget = moveTriggered ? moveFrom + moveDelta : moveFrom;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, moveTarget, Time.deltaTime * moveSpeed);
        moveEnd = transform.localPosition == moveTarget;
        if(moveEnd)
        {
            MyTriggerManager.Instance.busyMoves.Remove(this);
        }

        MyTriggerManager.Instance.busyRotates.Add(this);
        if (rotateTriggered)
        {
            hasResetCenter = false;
            rotateTarget += rotateDelta;
            rotateTarget = new Vector3(Mathf.Round(rotateTarget.x), Mathf.Round(rotateTarget.y), Mathf.Round(rotateTarget.z));
            rotateTriggered = false;
        }
        attached.localRotation = Quaternion.RotateTowards(attached.localRotation, Quaternion.Euler(rotateTarget), rotateSpeed * Time.deltaTime);

        if (Quaternion.Angle(attached.localRotation, Quaternion.Euler(rotateTarget)) < 1)
        {
            MyTriggerManager.Instance.busyRotates.Remove(this);
            if (hasResetCenter)
                return;
            hasResetCenter = true;
            for (int j = 0; j < attached.childCount; j++)
            {
                attached.localRotation = Quaternion.Euler(rotateTarget);
                GameObject g = attached.GetChild(j).gameObject;
                if (!g.activeSelf || g.GetComponent<IAttached>() == null)
                    continue;
                g.GetComponent<IAttached>().SetCurCenter(GetNearestCenterAfterRotate(g.transform));
            }
        }

    }
    public CenterPoint GetNearestCenterAfterRotate(Transform tr)
    {//找到旋转后的最近的中心点
        CenterPoint ret = null;
        float dis = float.MaxValue;
        foreach (var centerPoint in centerPoints)
        {
            float temp = Vector3.Distance(centerPoint.transform.position, tr.position);
            if (temp < dis)
            {
                dis = temp;
                ret = centerPoint;
            }
        }
        return ret;
    }
    #endregion
    [Header(" ")]
    [HelpBox("↓下面的不用改↓",HelpBoxType.Warning)]
    #region material mesh center
    List<Material> instanceMaterials = new(); // 实例材质
    public Transform attached;
    [SerializeField]
    GameObject trueMesh;
    [SerializeField]
    GameObject editMesh;

    [HideInInspector]public CenterPoint[] centerPoints = new CenterPoint[6];
    [HideInInspector]public HashSet<Cube> nearCubes = new();
    
    public CenterPoint GetSameDeltaCenterPoint(CenterPoint thatPoint)
    {
        foreach(var centerPoint in centerPoints)
        {
            if(centerPoint.transform.localPosition == thatPoint.transform.localPosition)
                return centerPoint;
        }
        return null;
    }
    #endregion


    private void OnDisable()
    {
        l1.Dispose();
        l2.Dispose();
    }
    public void OnMouseDown()
    {
        Debug.Log(name + " OnMouseDown");
        if (UIManager.Instance.IsUIBusy)
            return;
        PathFinder.Instance.SetDestinations(centerPoints); 
    }
    public void RefreshColorAndName()
    {
        instanceMaterials.Clear();
        foreach (var sharedMaterial in CubeConfig.Instance.sharedMaterials)
        {
            instanceMaterials.Add(new Material(sharedMaterial));
        }
        trueMesh.GetComponent<MeshRenderer>().materials = instanceMaterials.ToArray();
        instanceMaterials[0].color = CubeConfig.Instance.color_mar[color].color;
        name = color.ToString() + transform.GetSiblingIndex().ToString();
    }
    public void MagnetPos()
    {
        transform.localPosition = new Vector3(Mathf.Round(transform.localPosition.x), Mathf.Round(transform.localPosition.y), Mathf.Round(transform.localPosition.z));
    }
    private void Update()
    {
        MoveAndRotate();
    }
}
