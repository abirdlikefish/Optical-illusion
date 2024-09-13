using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerManager
{
    GameObject prefab_player;

    Player player;
    public Vector3Int pos{get {return player.pos;}}

    public void Init(GameObject prefab_player)
    {
        this.prefab_player = prefab_player;
    }

    public void CleanPlayer()
    {
        if(player == null)  return;
        GameObject.Destroy(player.gameObject);
        player = null;
    }
    public void InitPlayer(Vector3Int pos)
    {
        player = GameObject.Instantiate(prefab_player, pos , Quaternion.identity).GetComponent<Player>();
        player.Init(pos);
    }
    public void MoveBegin()
    {
        player.isMoving = true;
    }
    public void AddTargetToPlayer(Vector3Int pos)
    {
        player.AddTarget(pos);
    }
}
