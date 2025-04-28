using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargettableArg
{
    public SelectState TARGETTABLE = SelectState.None;
    public List<Vector3Int> TARGETTABLE_POSITIONS = new();
    public TargettableArg(SelectState targettable, List<Vector3Int> targettablePositions)
    {
        TARGETTABLE = targettable;
        TARGETTABLE_POSITIONS = targettablePositions;
    }
}