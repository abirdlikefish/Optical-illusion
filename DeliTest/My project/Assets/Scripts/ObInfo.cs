using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObWarning : MonoBehaviour
{
    float ObInfoTimer;
    // Start is called before the first frame update
    void Start()
    {
        ObInfoTimer = Config.Instance.ObInfoTime;
    }

    // Update is called once per frame
    void Update()
    {
        ObInfoTimer -= Time.deltaTime;
        //添加闪烁效果
        if (ObInfoTimer % 0.2f < 0.1f)
            GetComponent<MeshRenderer>().enabled = true;
        else
            GetComponent<MeshRenderer>().enabled = false;
        if (ObInfoTimer < 0)
            Destroy(gameObject);
    }
}
