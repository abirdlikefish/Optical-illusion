using System.Collections.Generic;
using UnityEngine;



public class BusyCollector : Singleton<BusyCollector>
{
    public List<Busy> busys = new();
    public bool IsBusy()
    {
        return busys.Count > 0;
    }
}