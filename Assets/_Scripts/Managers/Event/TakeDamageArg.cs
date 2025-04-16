using UnityEngine;

public class TakeDamageArg
{
    public Unit UNIT_SOURCE;
    public int DAMAGE;
    public HexTile TARGET;
    public TakeDamageArg(Unit unitSource, int damage, HexTile target)
    {
        UNIT_SOURCE = unitSource;
        DAMAGE = damage;
        TARGET = target;
    }
}