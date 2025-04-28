using System.Collections.Generic;
using UnityEngine;

public class PushArg
{
    public Unit SOURCE;
    public int STRENGTH;
    public Moveable TARGET;
    public HexDirection DIRECTION;
    public PushArg(Unit source, Moveable target, int strength, HexDirection direction)
    {
        SOURCE = source;
        TARGET = target;
        STRENGTH = strength;
        DIRECTION = direction;
    }
}