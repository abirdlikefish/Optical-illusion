using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Cube : MonoBehaviour
{
    [HelpBox("保证是CubeCombiner的子物体",HelpBoxType.Info)]
    [Header("刷新所有方块颜色和名字")]
    bool refreshColorAndName = true;
    [Header("将localPos四舍五入")]
    public bool magnetPos = false;
    public enum COLOR
    {
        BLACK,
        BLUE,
        GREEN,
        RED
    };
    public COLOR color;
    [Header(" ")]
    [HelpBox("↓下面的不用改↓",HelpBoxType.Warning)]
    public List<Material> sharedMaterials = new(); // 指向共享的材质
    List<Material> instanceMaterials = new(); // 实例材质
    
    [SerializeField]
    GameObject trueMesh;
    [SerializeField]
    GameObject editMesh;
    [HideInInspector]
    public CenterPoint[] centerPoints = new CenterPoint[6];
    public HashSet<Cube> nearCubes = new();
    
    public CenterPoint GetSameDeltaCenterPoint(CenterPoint thatPoint)
    {
        foreach(var centerPoint in centerPoints)
        {
            if(centerPoint.transform.localPosition == thatPoint.transform.localPosition)
                return centerPoint;
        }
        return null;
    }

    #region Debug
    ComputeBuffer computeBuffer1;
    ComputeBuffer computeBuffer2;
    ComputeBuffer l1;
    ComputeBuffer l2;

    #endregion

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
            l1.Release();
            l2.Release();
        }
        if (nearCubes.Count != 0)
        {
            computeBuffer1 = new ComputeBuffer(deltas.Count, sizeof(float) * 3);
            computeBuffer1.SetData(deltas.ToArray());
            trueMesh.GetComponent<MeshRenderer>().materials[0].SetBuffer("positionBuffer", computeBuffer1);
            Debug.Log(trueMesh.GetComponent<MeshRenderer>().materials[0].GetBuffer("positionBuffer").value);


            computeBuffer2 = new ComputeBuffer(colors.Count, sizeof(float) * 4);
            computeBuffer2.SetData(colors.ToArray());
            trueMesh.GetComponent<MeshRenderer>().materials[0].SetBuffer("colorBuffer", computeBuffer2);

        }
        else
        {
            computeBuffer1 = new ComputeBuffer(1, sizeof(float) * 3);
            computeBuffer1.SetData(new Vector3[1]);
            trueMesh.GetComponent<MeshRenderer>().materials[0].SetBuffer("positionBuffer", computeBuffer1);

            ComputeBuffer computeBuffer2 = new ComputeBuffer(1, sizeof(float) * 4);
            computeBuffer2.SetData(new Color[1]);
            trueMesh.GetComponent<MeshRenderer>().materials[0].SetBuffer("colorBuffer", computeBuffer2);
        }
        l1 = computeBuffer1;
        l2 = computeBuffer2;
    }
    public void OnMouseDown()
    {
        if (UIManager.Instance.IsUIBusy)
            return;
        PathFinder.Instance.SetDestinations(centerPoints); 
    }
    private void OnValidate()
    {
        if(Config.isFirstInScene)
        {
            Config.isFirstInScene = false;
            refreshColorAndName = true;
        }
        if(magnetPos)
        {
            magnetPos = false;
            transform.localPosition = new Vector3(Mathf.Round(transform.localPosition.x), Mathf.Round(transform.localPosition.y), Mathf.Round(transform.localPosition.z));
        }
        if (refreshColorAndName)
        {
            refreshColorAndName = false;
            for (int i = 0; i < CubeCombiner.Instance.transform.childCount; i++)
            {
                CubeCombiner.Instance.transform.GetChild(i).GetComponent<Cube>().MyOnValidate();
            }
        }
    }
    public void MyOnValidate()
    {
        instanceMaterials.Clear();
        foreach (var sharedMaterial in sharedMaterials)
        {
            instanceMaterials.Add(new Material(sharedMaterial));
        }
        trueMesh.GetComponent<MeshRenderer>().materials = instanceMaterials.ToArray();
        instanceMaterials[0].color = CubeColor.Instance.color_mar[color].color;
        name = color.ToString() + transform.GetSiblingIndex().ToString();
    }

    //void HideAllCenterPoints()
    //{
    //    foreach (var centerPoint in centerPoints)
    //    {
    //        centerPoint.GetComponent<MeshRenderer>().enabled = false;
    //    }
    //}
    //public List<CenterPoint> GetVisibleCenterPoint()
    //{
    //    List<CenterPoint> ret = new();
    //    foreach (var it in centerPoints)
    //    {
    //        bool foundSameAxis = false;
    //        foreach (var it2 in ret)
    //        {
    //            if (Vector3.Dot(it.delta, it2.delta) != 0)
    //            {
    //                foundSameAxis = true;
    //                if (it.transform.position.z < it2.transform.position.z - 0.001f)
    //                {
    //                    ret.Add(it);
    //                    ret.Remove(it2);
    //                    break;
    //                }
    //            }
    //        }
    //        if (foundSameAxis)
    //            continue;
    //        ret.Add(it);
    //    }
    //    return ret;
    //}
    //public CenterPoint GetNearestVisibleCenterPoint(Vector2 pos)
    //{
    //    List<CenterPoint> visibleCenterPoints = GetVisibleCenterPoint();
    //    CenterPoint nearest = null;
    //    float minDis = float.MaxValue;
    //    foreach (var centerPoint in visibleCenterPoints)
    //    {
    //        float dis = Vector2.Distance(Camera.main.WorldToScreenPoint(centerPoint.transform.position), pos);
    //        if (dis < minDis)
    //        {
    //            minDis = dis;
    //            nearest = centerPoint;
    //        }
    //    }
    //    return nearest;
    //}

}
