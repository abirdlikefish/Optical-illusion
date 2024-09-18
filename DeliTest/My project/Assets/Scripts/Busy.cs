using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Busy : MonoBehaviour
{
    public void AddBusy(Busy iBusy)
    {
        if (!BusyCollector.Instance.busys.Contains(iBusy))
            BusyCollector.Instance.busys.Add(iBusy);
    }
    public void RemoveBusy(Busy iBusy)
    {
        BusyCollector.Instance.busys.Remove(iBusy);
    }
}