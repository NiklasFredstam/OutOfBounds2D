using System.Collections.Generic;
using UnityEngine;


public class MoveArg
{
    public Unit SOURCE;
    public Moveable TARGET;
    public Vector3Int TO_POSITION;

    public bool IS_PUSH = false;
    public MoveArg(Unit source, Moveable target, Vector3Int toPosition, bool isPush)
    {
        SOURCE = source;
        TARGET = target;
        TO_POSITION = toPosition;
        IS_PUSH = isPush;
    }
}

