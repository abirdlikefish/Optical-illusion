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
        public BaseCube cube_L;
        public BaseCube cube_R;
        public Vector3Int cubePos_L;
        public Vector3Int cubePos_R;
        public bool isPassable_L;
        public bool isPassable_R;
        public bool isVisited;
        public int depth_L;
        public int depth_R;
        public Vector2Int prevPos;
        public void reset()
        {
            // cube_L = null;
            // cube_R = null;
            isPassable_L = false;
            isPassable_R = false;
            isVisited = false;
            depth_L = -9999999;
            depth_R = -9999999;
            prevPos = Vector2Int.zero;
        }
    }
    CameraGrid[,] cameraGrid = new CameraGrid[maxCameraGridX, maxCameraGridY];

    public void Init()
    {
        CleanCameraGrid();
    }

    public void CleanCameraGrid()
    {
        for (int i = 0; i < maxCameraGridX; i++)
        {
            for (int j = 0; j < maxCameraGridY; j++)
            {
                cameraGrid[i, j].reset();
            }
        }
    }
    public void SetCameraGrid_visited(bool isVisited)
    {
        for (int i = 0; i < maxCameraGridX; i++)
        {
            for (int j = 0; j < maxCameraGridY; j++)
            {
                cameraGrid[i, j].isVisited = isVisited;
            }
        }
    }

    // void DrawGrid_L(Cube cube, Vector2Int pos, int depth, bool flag)
    void DrawGrid_L(BaseCube cube,Vector3Int cubePos, Vector2Int pos, int depth, bool flag)
    {
        if (cameraGrid[pos.x, pos.y].depth_L < depth)
        {
            cameraGrid[pos.x, pos.y].cube_L = cube;
            cameraGrid[pos.x, pos.y].cubePos_L = cubePos;
            cameraGrid[pos.x, pos.y].depth_L = depth;
            cameraGrid[pos.x, pos.y].isPassable_L = flag;
        }
    }
    // void DrawGrid_R(Cube cube, Vector2Int pos, int depth, bool flag)
    void DrawGrid_R(BaseCube cube,Vector3Int cubePos, Vector2Int pos, int depth, bool flag)
    {
        if (cameraGrid[pos.x, pos.y].depth_R < depth)
        {
            cameraGrid[pos.x, pos.y].cube_R = cube;
            cameraGrid[pos.x, pos.y].cubePos_R = cubePos;
            cameraGrid[pos.x, pos.y].depth_R = depth;
            cameraGrid[pos.x, pos.y].isPassable_R = flag;
        }
    }
    public void DrawGridFromCube(BaseCube cube, Vector3Int midPos, int depth)
    {
        Vector2Int pos = CubePos2CameraGridPos(midPos);
        DrawGrid_L(cube, midPos, pos, depth, true);
        DrawGrid_R(cube, midPos, pos, depth, true);

        DrawGrid_L(cube, midPos, pos + new Vector2Int(1, 1), depth, false);
        DrawGrid_R(cube, midPos, pos + new Vector2Int(1, 1), depth, false);

        DrawGrid_L(cube, midPos, pos + new Vector2Int(0, 1), depth, false);
        DrawGrid_R(cube, midPos, pos + new Vector2Int(1, 0), depth, false);
    }
    // public void DrawGridFromCube(BaseCube cube, Vector3Int pos, int depth)
    // {
    //     DrawGridFromCube(cube, CubePos2CameraGridPos(pos), depth);
    // }

    Queue<Vector2Int> bfsQueue = new Queue<Vector2Int>();
    Vector2Int[] midOffset = new Vector2Int[4] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    public void FindPath(Vector2Int begPos)
    {
        SetCameraGrid_visited(false);
        bfsQueue.Clear();
        bfsQueue.Enqueue(begPos);
        cameraGrid[begPos.x, begPos.y].isVisited = true;
        cameraGrid[begPos.x, begPos.y].prevPos = new Vector2Int(begPos.x, begPos.y);
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
    }

    public bool IsPassable(Vector2Int pos)
    {
        return cameraGrid[pos.x, pos.y].isPassable_L && cameraGrid[pos.x, pos.y].isPassable_R;
    }
    public bool IsVisited(Vector2Int pos)
    {
        return cameraGrid[pos.x, pos.y].isVisited;
    }
    public bool IsPassable(Vector3Int pos)
    {
        Vector2Int midPos = CameraGridManager.CubePos2CameraGridPos(pos);
        return IsPassable(midPos);
    }
    public bool IsVisited(Vector3Int pos)
    {
        Vector2Int midPos = CameraGridManager.CubePos2CameraGridPos(pos);
        return IsVisited(midPos);
    }
    public Vector3Int CameraGridPos2CubePos(Vector2Int midPos)
    {
        if (cameraGrid[midPos.x, midPos.y].depth_L > cameraGrid[midPos.x, midPos.y].depth_R)
        {
            return cameraGrid[midPos.x, midPos.y].cubePos_L;
        }
        else
        {
            return cameraGrid[midPos.x, midPos.y].cubePos_R;
        }
    }


    public bool SetTargetsToPlayer(Vector2Int target)
    {
        while (true)
        {
            Command_normal.AddTargetToPlayer(CameraGridPos2CubePos(target));
            if(target == cameraGrid[target.x, target.y].prevPos)
            {
                break;
            }
            target = cameraGrid[target.x, target.y].prevPos;
        }
        return true;
    }
    public bool SetTargetsToPlayer(Vector3Int target)
    {
        Vector2Int midPos = CubePos2CameraGridPos(target);
        return SetTargetsToPlayer(midPos);
    }
}
