using System.Collections.Generic;
using UnityEngine;
//单例
//需要被继承 xxx : Singleton<xxx>
//获取单例 xxx.Instance
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    static T instance;
    [Header("Singleton")]
    //多场景下是否销毁
    public bool global = false;
    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<T>();
            return instance;
        }
    }
    private void Awake()
    {
        if (global)
        {
            if (Instance != null && Instance != this)
            {
                GameObject that = instance.gameObject;
                //List<AchieveInfo> temp = new List<AchieveInfo>();
                //if (instance is UIManager)
                //{
                //    temp = (instance as UIManager).List_achieves;
                //}
                //instance = this as T;
                //if (instance is UIManager)
                //{
                //    (instance as UIManager).List_achieves = temp;
                //}
                Destroy(that);
            }
            DontDestroyOnLoad(gameObject);
        }
    }

}
