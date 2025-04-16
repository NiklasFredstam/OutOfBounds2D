using UnityEngine;

public class BreakArg
{
    public Unit SOURCE;
    public Vector3Int POSITION;
    public HexTile TILE;
    public BreakArg(Unit unitSource, Vector3Int position, HexTile tile)
    {
        SOURCE = unitSource;
        POSITION = position;
        TILE = tile;
    }
}