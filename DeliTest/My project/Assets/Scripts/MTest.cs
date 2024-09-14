using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTest : MonoBehaviour
{

    private void Awake()
    {
        Material material = GetComponent<MeshRenderer>().materials[0]; // 这会创建一个材质的实例

        // 修改材质的属性
        material.SetColor("_ColorBase", Color.red);
    }
   
}
