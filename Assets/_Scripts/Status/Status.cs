using System.Collections.Generic;
using UnityEngine;

public class Status
{
    public string StatusName;

    public virtual void Initialize(StatusScriptable data)
    {
        StatusName = data.StatusName;
    }
}
