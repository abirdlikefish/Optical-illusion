using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Cube : MonoBehaviour
{
    [HelpBox("保证是CubeCombiner的子物体",HelpBoxType.Info)]
    [Header("刷新所有方块颜色和名字")]
    public bool refreshColorAndName = false;
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
    public List<Cube> nearCubes = new();
    
    public CenterPoint GetSameDeltaCenterPoint(CenterPoint thatPoint)
    {
        foreach(var centerPoint in centerPoints)
        {
            if(centerPoint.transform.localPosition == thatPoint.transform.localPosition)
                return centerPoint;
        }
        return null;
    }

    public void Change6SideColor()
    {
        if(name == "RED3")
        {
            int c = 1;
        }
        List<Vector3> deltas = new();
        List<Color> colors = new();
        foreach (var nearCube in nearCubes)
        {
            deltas.Add(nearCube.transform.position - transform.position);
            colors.Add(nearCube.trueMesh.GetComponent<MeshRenderer>().materials[0].GetColor("_ColorBase"));
        }
        if(nearCubes.Count != 0)
        {
            ComputeBuffer computeBuffer = new ComputeBuffer(deltas.Count, sizeof(float) * 3);
            computeBuffer.SetData(deltas.ToArray());
            trueMesh.GetComponent<MeshRenderer>().materials[0].SetBuffer("positionBuffer", computeBuffer);
            computeBuffer = new ComputeBuffer(colors.Count, sizeof(float) * 4);
            computeBuffer.SetData(colors.ToArray());
            trueMesh.GetComponent<MeshRenderer>().materials[0].SetBuffer("colorBuffer", computeBuffer);
        }

    }
    public void ChangeColor(Cube nearCube)
    {
        Vector3 deltaCube = nearCube.transform.localPosition - transform.localPosition;
        deltaCube = new Vector3(Mathf.Round(deltaCube.x), Mathf.Round(deltaCube.y), Mathf.Round(deltaCube.z));
        if (!CubeColor.Instance.deltavector_to_CanChange.ContainsKey(deltaCube))
        {
            return;
        }
        trueMesh.GetComponent<MeshRenderer>().materials[0].SetFloat(CubeColor.Instance.deltavector_to_CanChange[deltaCube].Key, 1);
    }
    public void OnMouseDown()
    {
        if (UIManager.Instance.IsUIBusy)
            return;
        PathFinder.Instance.SetDestinations(centerPoints); 
    }
    private void OnValidate()
    {
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
        instanceMaterials[0].SetColor("_ColorBase" , CubeColor.Instance.color_mar[color].GetColor("_Color")); 
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
