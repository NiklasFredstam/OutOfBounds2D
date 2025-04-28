

using UnityEngine;

public class DoDamageArg
{
    public Unit SOURCE;
    public int DAMAGE;
    public HexTile TARGET;
    public DoDamageArg(Unit source, int damage, HexTile target)
    {
        SOURCE = source;
        DAMAGE = damage;
        TARGET = target;
    }
}

