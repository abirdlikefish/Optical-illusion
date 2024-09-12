using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGridManager
{

    public static Vector3Int offset = new Vector3Int(50, 0, 50);
    static Vector3Int normal = new Vector3Int(1, 1, 1);
    static int maxCameraGridX = 100;
    static int maxCameraGridY = 100;
    public static Vector2Int CubePos2CameraGridPos(Vector3Int c)
    {
        c -= c.y * normal;
        c += offset;
        return new Vector2Int(c.x, c.z);
    }

    struct CameraGrid
    {
        public Cube cube_L;
        public Cube cube_R;
        public bool isPassable_L;
        public bool isPassable_R;
        public bool isVisited;
        public int depth_L;
        public int depth_R;
        public Vector2Int prevPos;
        public void reset()
        {
            isPassable_L = false;
            isPassable_R = false;
            isVisited = false;
            depth_L = -9999999;
            depth_R = -9999999;
        }
    }
    CameraGrid[,] cameraGrid = new CameraGrid[maxCameraGridX, maxCameraGridY];

    public void Init()
    {
        CleanCameraGrid();
    }

    public void ResetCameraGrid()
    {
        for (int i = 0; i < maxCameraGridX; i++)
        {
            for (int j = 0; j < maxCameraGridY; j++)
            {
                cameraGrid[i, j].reset();
            }
        }
    }
    public void CleanCameraGrid()
    {
        for (int i = 0; i < maxCameraGridX; i++)
        {
            for (int j = 0; j < maxCameraGridY; j++)
            {
                cameraGrid[i, j].isVisited = false;
            }
        }
    }

    public void DrawGrid_L(Cube cube, Vector2Int pos, int depth, bool flag)
    {
        if (cameraGrid[pos.x, pos.y].depth_L < depth)
        {
            cameraGrid[pos.x, pos.y].cube_L = cube;
            cameraGrid[pos.x, pos.y].depth_L = depth;
            cameraGrid[pos.x, pos.y].isPassable_L = flag;
        }
    }
    public void DrawGrid_R(Cube cube, Vector2Int pos, int depth, bool flag)
    {
        if (cameraGrid[pos.x, pos.y].depth_R < depth)
        {
            cameraGrid[pos.x, pos.y].cube_R = cube;
            cameraGrid[pos.x, pos.y].depth_R = depth;
            cameraGrid[pos.x, pos.y].isPassable_R = flag;
        }
    }

    Queue<Vector2Int> bfsQueue = new Queue<Vector2Int>();
    Vector2Int[] midOffset = new Vector2Int[4] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    public void FindPath(Vector2Int begPos)
    {
        CleanCameraGrid();
        bfsQueue.Clear();
        bfsQueue.Enqueue(begPos);
        cameraGrid[begPos.x, begPos.y].isVisited = true;
        while (bfsQueue.Count > 0)
        {
            Vector2Int midPos = bfsQueue.Dequeue();
            for (int i = 0; i < 4; i++)
            {
                Vector2Int pos = midPos + midOffset[i];
                if (IsPassable(pos) && cameraGrid[pos.x, pos.y].isVisited == false)
                {
                    cameraGrid[pos.x, pos.y].isVisited = true;
                    cameraGrid[pos.x, pos.y].prevPos = midPos;
                    bfsQueue.Enqueue(pos);
                }
            }
        }
    }
    public void FindPath(Vector3Int playerPos)
    {
        Vector2Int begPos = CubePos2CameraGridPos(playerPos);
        FindPath(begPos);
        //bfsQueue.Clear();
        //bfsQueue.Enqueue(begPos);
        //cameraGrid[begPos.x, begPos.y].isVisited = true;
        //while (bfsQueue.Count > 0)
        //{
        //    Vector2Int midPos = bfsQueue.Dequeue();
        //    for (int i = 0; i < 4; i++)
        //    {
        //        Vector2Int pos = midPos + midOffset[i];
        //        if (IsPassable(pos) && cameraGrid[pos.x, pos.y].isVisited == false)
        //        {
        //            cameraGrid[pos.x, pos.y].isVisited = true;
        //            cameraGrid[pos.x, pos.y].prevPos = midPos;
        //            bfsQueue.Enqueue(pos);
        //        }
        //    }
        //}
    }

    public bool IsPassable(Vector2Int pos)
    {
        return cameraGrid[pos.x, pos.y].isPassable_L && cameraGrid[pos.x, pos.y].isPassable_R;
    }
    public Vector3Int CameraGridPos2CubePos(Vector2Int midPos)
    {
        if (cameraGrid[midPos.x, midPos.y].depth_L > cameraGrid[midPos.x, midPos.y].depth_R)
        {
            return cameraGrid[midPos.x, midPos.y].cube_L.pos;
        }
        else
        {
            return cameraGrid[midPos.x, midPos.y].cube_R.pos;
        }
    }


    public bool SetTarget(Vector2Int target , Vector2Int begPos , Player player)
    {
        if (cameraGrid[target.x , target.y].isVisited == false)
        {
            return false;
        }
        player.CleanTarget();
        while (target != begPos)
        {
            Debug.Log("target : " + target);

            player.AddTarget(CameraGridPos2CubePos(target));
            //player.AddTarget(target);
            target = cameraGrid[target.x, target.y].prevPos;
        }
        //player.AddTarget(target);
        player.AddTarget(CameraGridPos2CubePos(target));
        return true;
    }
}
