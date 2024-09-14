using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : BaseCube
{
    // public Vector3Int pos;
    // public Vector2Int cameraGridPos;
    public int depth;
    public void Init(CubeManager cubeManager , Vector3Int pos )
    {
        this.pos = pos;
        this.depth = pos.x + pos.y + pos.z;
        this.cubeManager = cubeManager; 
        transform.position = pos;
        
        cubeManager.SetCubeMatrix(pos, this);
        // cameraGridPos = CameraGridManager.CubePos2CameraGridPos(pos);
    }
    void Awake()
    {
        
    }
    void Start()
    {
    }

    void Update()
    {
        
    }

    public override void SetBeg()
    {
        gameObject.GetComponentInChildren<Renderer>().material = black;
    }
    public override void SetEnd()
    {
        gameObject.GetComponent<Renderer>().material = red;
    }
    public override void Reset()
    {
        gameObject.GetComponent<Renderer>().material = white;
    }
}
