using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    [SerializeField]
    private Grid _grid;
    private readonly Dictionary<Vector3Int, HexTile> _hexTileDictionary = new Dictionary<Vector3Int, HexTile>();

    public void RemoveHexTile(HexTile hexTile)
    {
        _hexTileDictionary.Remove(GetGridPositionOfHex(hexTile.transform.position));
        hexTile.AfterTileBreak.Unsubscribe(RemoveHexTile);
    }

    public void AddHexTile(HexTile hexTile)
    {
        _hexTileDictionary.Add(GetGridPositionOfHex(hexTile.transform.position), hexTile);
        hexTile.AfterTileBreak.Subscribe(RemoveHexTile);
    }

    public Vector3 GetWorldPositionOfHex(Vector3Int gridPosition)
    {
        return _grid.CellToWorld(gridPosition);
    }

    public HexTile GetHexTileFromGridPosition(Vector3Int gridPosition)
    {
        return _hexTileDictionary[gridPosition];
    }

    public HexTile GetHexTileFromWorldPosition(Vector3 worldPosition)
    {
        return _hexTileDictionary[GetGridPositionOfHex(worldPosition)];
    }


    private Vector3Int GetGridPositionOfHex(Vector3 worldPosition)
    {
        return _grid.WorldToCell(worldPosition);
    }

    //Get A* path from A to B

    

}
