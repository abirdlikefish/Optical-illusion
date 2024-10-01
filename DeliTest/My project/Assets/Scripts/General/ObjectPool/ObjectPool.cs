using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    GameObject tPrefab;
    [HelpBox("设置对象池父物体，不设置则在同级Sible中生成新物体作为父物体",HelpBoxType.Info)]
    [SerializeField]
    Transform objectPoolParent;
    int initCount = 16;
    int poolCount;
    List<GameObject> allObject;
    Stack<GameObject> availableObject;
    public void Initialize()
    {
        allObject = new();
        availableObject = new();
        tPrefab = gameObject;
        if(objectPoolParent == null)
        {
            GameObject g = new GameObject(name + "Pool");
            g.transform.parent = transform.parent;
            objectPoolParent = g.transform;
        }
        MyCreateNew(poolCount);
    }
    void MyCreateNew(int newCount)
    {
        for (int i = 0; i < newCount; i++)
        {
            GameObject g = Instantiate(tPrefab, Vector3.zero, Quaternion.identity, objectPoolParent);
            g.SetActive(false);
            allObject.Add(g);
            availableObject.Push(g);
        }
    }

    public GameObject MyInstantiate(Vector3 fPos, Quaternion fRot, Transform fParent = null)
    {
        if (availableObject.Count == 0)
        {
            MyCreateNew((int)(poolCount == 0 ? initCount : poolCount * 0.5f));
        }
        GameObject g = availableObject.Pop();
        g.transform.SetPositionAndRotation(fPos, fRot);
        if (fParent != null)
            g.transform.parent = fParent;
        else
            g.transform.parent = objectPoolParent;
        g.gameObject.SetActive(true);
        return g;
    }
    public void MyDestroy(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogError("MyDestroy null " + tPrefab.name + " !");
            return;
        }
        obj.SetActive(false);
        availableObject.Push(obj);
    }
}
