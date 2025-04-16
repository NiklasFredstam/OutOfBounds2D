
using UnityEngine;

public class FallArg
{
    public Unit UNIT_SOURCE;
    public Moveable FALLING;
    public FallArg(Unit unitSource, Moveable falling)
    {
        UNIT_SOURCE = unitSource;
        FALLING = falling;
    }
}
