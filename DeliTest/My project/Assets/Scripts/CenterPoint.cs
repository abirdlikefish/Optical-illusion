using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPoint : MonoBehaviour
{
    public Vector3Int delta;
    [HideInInspector]
    public Cube cube;
    [HideInInspector]
    public string info;
    [SerializeField]
    List<NearPoint> nearPoints = new();
    private void Awake()
    {
        cube = transform.parent.GetComponent<Cube>();
        info = cube.name + "::" + name;
    }
    public void AddNearPointTimer(CenterPoint thatPoint)
    {
        NearPoint nearPoint = nearPoints.Find(x => x.nearPoint == thatPoint);
        if (nearPoint == null)
        {
            nearPoint = new NearPoint(thatPoint);
            nearPoints.Add(nearPoint);
        }
        nearPoint.nearTimer += Time.deltaTime;
        if(nearPoint.nearTimer >= CubeCombiner.nearTime)
            Debug.Log(info + " is near to " + thatPoint.info);
    }
    public void ClearNearPointTimer(CenterPoint thatPoint)
    {
        NearPoint nearPoint = nearPoints.Find(x => x.nearPoint == thatPoint);
        if (nearPoint == null)
            return;
        nearPoints.Find(x => x.nearPoint == thatPoint).nearTimer = 0f;
    }
}
class NearPoint
{
    public CenterPoint nearPoint;
    public float nearTimer;

    public NearPoint(CenterPoint nearPoint)
    {
        this.nearPoint = nearPoint;
        this.nearTimer = 0f;
    }
}