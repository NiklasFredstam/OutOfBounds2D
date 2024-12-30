using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager> 
{

    [SerializeField]
    private HexGrid _hexGrid;


    public void SpawnHexGrid()
    {
        if (_hexGrid == null) return;
        SpawnGrid(_exampleGrid);
    }

    public void MoveMoveableToHex(Moveable moveable, HexTile toTile)
    {
        if (toTile.gameObject.activeSelf)
        {
            HexTile fromTile = _hexGrid.GetHexTileFromWorldPosition(moveable.transform.position);
            fromTile.AfterTileBreak.Unsubscribe(moveable.Fall);
            toTile.AfterTileBreak.Subscribe(moveable.Fall);
            moveable.Move(toTile.transform.position);
        }
        //DO THIS ONE AT A TIME, calculate the path here and do it        
    }

    //This will probably be unnceccsary later when you choose when to spawn /either preset or dynamically
    public HexTile GetHexTileFromGrid(Vector3Int gridPosition)
    {
        return _hexGrid.GetHexTileFromGridPosition(gridPosition);
    }


    //This should probably be a complex object that is scriptable or generated.
    private void SpawnGrid(List<Vector3Int> hexesToSpawn)
    {
        foreach (Vector3Int hexPos in hexesToSpawn)
        {
            SpawnHexTile(TileType.Stone, hexPos);
        }
    }

    private void SpawnHexTile(TileType tileType, Vector3Int hexPosition)
    {
        //Get position to spawn cell
        Vector3 worldPosition = _hexGrid.GetWorldPositionOfHex(hexPosition);

        //TODO: Move other tiles if spawned on a tile.

        //Get resource, create it, and save it in dict
        HexTileScriptable toSpawn = ResourceSystem.instance.GetTile(tileType);
        HexTile createdTile = Instantiate(toSpawn.Prefab, worldPosition, Quaternion.identity);
        _hexGrid.AddHexTile(createdTile);

        //Set parent and name to keep it tidy
        GameObject grid = GameObject.Find("HexGrid");
        createdTile.transform.SetParent(grid.transform);
        createdTile.name = GetHexTileName(hexPosition);

        //Set stats and data on cell
        createdTile.UpdateTilePosition(hexPosition);
        createdTile.SetStats(toSpawn.TileStats);
    }

    private string GetHexTileName(Vector3Int hexPosition)
    {
        return $"HexTile({hexPosition.x}, {hexPosition.y})";
    }

    //move this out
    private List<Vector3Int> _exampleGrid = new List<Vector3Int>
    {
        new Vector3Int(0,0),
        new Vector3Int(1,0),
        new Vector3Int(2,0),
        new Vector3Int(-1,1),
        new Vector3Int(0,1),
        new Vector3Int(1,1),
        new Vector3Int(2,1),
        new Vector3Int(0,2),
        new Vector3Int(1,2),
        new Vector3Int(2,2),
    };
}
