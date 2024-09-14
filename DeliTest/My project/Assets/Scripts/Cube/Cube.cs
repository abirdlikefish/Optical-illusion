using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public bool RefreshCube = false;

    public List<Material> sharedMaterials = new(); // 指向共享的材质
    List<Material> instanceMaterials = new(); // 实例材质
    public enum COLOR
    {
        BLACK,
        BLUE,
        GREEN,
        RED
    };
    public COLOR color;
    [SerializeField]
    GameObject trueMesh;
    [SerializeField]
    GameObject editMesh;
    public CenterPoint[] centerPoints = new CenterPoint[6];
    public CenterPoint GetSameDeltaCenterPoint(CenterPoint thatPoint)
    {
        foreach(var centerPoint in centerPoints)
        {
            if(centerPoint.transform.localPosition == thatPoint.transform.localPosition)
                return centerPoint;
        }
        return null;
    }
    

    public void ChangeColor(Cube nearCube)
    {
        Vector3 deltaCube = nearCube.transform.localPosition - transform.localPosition;
        trueMesh.GetComponent<MeshRenderer>().materials[0].SetFloat(CubeColor.Instance.deltavector_to_CanChange[deltaCube].Key, 1);
        trueMesh.GetComponent<MeshRenderer>().materials[0].SetColor(CubeColor.Instance.deltavector_to_CanChange[deltaCube].Value, CubeColor.Instance.color_mar[nearCube.color].GetColor("_Color"));
    }
    public void OnMouseDown()
    {
        PathFinder.Instance.SetDestinations(centerPoints); 
    }
    private void OnValidate()
    {
        if (!RefreshCube)
            return;
        RefreshCube = false;
        Config.Instance.OnValidate();
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
