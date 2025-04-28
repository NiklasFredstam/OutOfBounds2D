using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile", menuName = "Scriptable Tile")]
public class HexTileScriptable : ScriptableObject
{
    [SerializeField] private TileStats _tileStats;
    public TileStats TileStats => _tileStats;
    public HexTile Prefab;
    public TileType TileType;
}
public enum TileType
{
    Stone
}
