using System.Collections.Generic;
using UnityEngine;

public class StatusManager : Singleton<StatusManager>
{
    public List<Status> CreateStatuses(List<StatusType> statusTypes)
    {
        List<Status> statuses = new();
        /*for (int i = 0; i < statusTypes.Count; i++)
        {
            StatusType type = statusTypes[i];
            StatusScriptable statusScriptable = ResourceSystem.instance.GetStatus(type);
            Status newStatus = statusScriptable.CreateStatusInstance();
            statuses.Add(newStatus);
        }*/
        return statuses;
    }
}
