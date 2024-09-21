using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeConfig : Singleton<CubeConfig>
{
    public SerializableDictionary<Cube.COLOR, Material> color_mar;
    public Dictionary<Vector3, KeyValuePair<string, string>> deltavector_to_CanChange;
    public List<Material> sharedMaterials = new(); // 指向共享的材质
    private void Awake()
    {
        deltavector_to_CanChange = new Dictionary<Vector3, KeyValuePair<string,string>>()
        {
            { new Vector3(1, 0, 0),new KeyValuePair<string,string>("_CanChangeRight","_ColorRight")},
            { new Vector3(-1, 0, 0),new KeyValuePair<string,string>("_CanChangeLeft","_ColorLeft")},
            { new Vector3(0, 1, 0),new KeyValuePair<string,string>("_CanChangeUp","_ColorUp")},
            { new Vector3(0, -1, 0),new KeyValuePair<string,string>("_CanChangeDown","_ColorDown")},
            { new Vector3(0, 0, -1),new KeyValuePair<string,string>("_CanChangeFront","_ColorFront")},
            { new Vector3(0, 0, 1),new KeyValuePair<string,string>("_CanChangeBehind","_ColorBehind")},
        };

    }
}
