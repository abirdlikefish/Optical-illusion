using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public Vector3Int pos;
    public Vector2Int cameraGridPos;
    public int depth;
    public Material black;
    public Material white;
    public Material red;
    public void Init()
    {
        this.pos = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
        this.depth = pos.x + pos.y + pos.z;
        cameraGridPos = CameraGridManager.CubePos2CameraGridPos(pos);
    }
    public void Init(Vector3Int pos)
    {
        this.pos = pos;
        this.depth = pos.x + pos.y + pos.z;
        cameraGridPos = CameraGridManager.CubePos2CameraGridPos(pos);
    }
    void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        //GameObject.Find("GameManager").GetComponent<GameManager>().AddCube(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBeg()
    {
        gameObject.GetComponentInChildren<Renderer>().material = black;
    }
    public void SetEnd()
    {
        gameObject.GetComponent<Renderer>().material = red;
    }
    public void Reset()
    {
        gameObject.GetComponent<Renderer>().material = white;
    }
}
