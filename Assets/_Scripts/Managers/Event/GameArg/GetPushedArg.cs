using System.Collections.Generic;
using UnityEngine;

public class GetPushedArg
{
    public Unit SOURCE;
    public Moveable TARGET;
    public int POWER;
    public HexDirection DIRECTION;
    public GetPushedArg(Unit source, Moveable target, int power, HexDirection direction)
    {
        SOURCE = source;
        TARGET = target;
        POWER = power;
        DIRECTION = direction;
    }
}