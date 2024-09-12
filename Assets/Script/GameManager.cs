using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject begCube_inspector;
    public GameObject endCube_inspector;
    public GameObject prefab_cube;
    public GameObject prefab_player;

    CameraGridManager cameraGridManager;
    CubeManager cubeManager;
    SaveManager saveManager;
    Player player;

    #region Cube
    #endregion

    #region cameraGrid
    #endregion

    void Awake()
    {
        cameraGridManager = new CameraGridManager();        
        cubeManager = new CubeManager();
        saveManager = new SaveManager();
        cameraGridManager.Init();
        cubeManager.Init(cameraGridManager , prefab_cube , prefab_player);
        saveManager.Init(cubeManager);
    }

    void Start()
    {
        if(saveManager.LoadData())
        {
            player = saveManager.InitCube();
        }
        else
        {
        }
        //cubeManager.SetBegCube(begCube_inspector);
        cubeManager.DrawCameraGrid();
        cameraGridManager.FindPath(player.pos);
    }

    void Update()
    {
        //…Ë÷’µ„
        if(Input.GetMouseButtonDown(0) && player.isMoving == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10000, LayerMask.GetMask("Cube")))
            {
                //Debug.Log("Hit Object: " + hit.collider.gameObject.layer);
                cubeManager.SetEndCube(hit.collider.GetComponent<Cube>() , player);
            }
        }
    }

    //public void AddCube(GameObject gameObject)
    //{
    //    cubeManager.AddCube(gameObject);
    //}
}
