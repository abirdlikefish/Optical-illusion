using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class MTest : MonoBehaviour
{
    public int i = 0;
    public string data = "test";
    private void OnValidate()
    {
        JsonIO.WriteCurSheet("aaaatest", "0", this);
    }
}
