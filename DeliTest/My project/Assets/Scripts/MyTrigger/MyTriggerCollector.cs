using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTriggerCollector : Singleton<MyTriggerCollector>
{
    public List<MyTrigger> busys = new();
    public bool IsBusy()
    {
        return busys.Count > 0;
    }
    public void AddBusy(MyTrigger trigger)
    {
        if(!busys.Contains(trigger))
            busys.Add(trigger);
    }
    public void RemoveBusy(MyTrigger trigger)
    {
        busys.Remove(trigger);
    }
}
