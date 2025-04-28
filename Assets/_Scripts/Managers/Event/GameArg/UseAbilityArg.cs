using UnityEngine;

public class UseAbilityArg
{
    public Unit UNIT_SOURCE;
    public HexTile TILE_TARGET;
    public UseAbilityArg(Unit unitSource, HexTile tileTarget)
    {
        UNIT_SOURCE = unitSource;
        TILE_TARGET = tileTarget;
    }
}