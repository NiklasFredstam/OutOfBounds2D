using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Status", menuName = "Scriptable Status")]
public class StatusScriptable : ScriptableObject
{
    public string StatusName;
    public StatusType StatusType;

    public Status CreateItemInstance()
    {
        Status status = StatusType switch
        {
            StatusType.Lightweight => new Lightweight(),
            _ => null
        };

        if (status == null)
        {
            Debug.LogWarning($"Ability type {StatusType} is not supported.");
            return null;
        }

        status.Initialize(this);
        return status;
    }
}

public enum StatusType
{
    Lightweight,
}