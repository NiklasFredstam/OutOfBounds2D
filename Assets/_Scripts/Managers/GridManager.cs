using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GridManager : Singleton<GridManager> 
{
    [SerializeField]
    public HexGrid HexGrid;

    //move this out
    private readonly List<Vector3Int> _exampleGrid = new List<Vector3Int>
    {
        new (0,0),
        new (1,0),
        new (2,0),
        new (-1,1),
        new (0,1),
        new (1,1),
        new (2,1),
        new (0,2),
        new (1,2),
        new (2,2),
    };

    public void SpawnHexGrid()
    {
        if (HexGrid == null) return;
        SpawnGrid(_exampleGrid);
    }


    //This will probably be unnceccsary later when you choose when to spawn /either preset or dynamically
    public HexTile GetHexTileFromGrid(Vector3Int gridPosition)
    {
        return HexGrid.GetHexTileFromGridPosition(gridPosition);
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
        Vector3 worldPosition = HexGrid.GetWorldPositionOfHex(hexPosition);

        //TODO: Move other tiles if spawned on a tile.

        //Get resource, create it, and save it in dict
        HexTileScriptable toSpawn = ResourceSystem.instance.GetTile(tileType);
        HexTile createdTile = Instantiate(toSpawn.Prefab, worldPosition, Quaternion.identity);
        HexGrid.AddHexTile(createdTile);

        //Set parent and name to keep it tidy
        GameObject grid = GameObject.Find("HexGrid");
        createdTile.transform.SetParent(grid.transform);
        createdTile.name = GetHexTileName(hexPosition);

        //Set stats and data on cell
        createdTile.UpdateTilePosition(hexPosition);
        createdTile.SetStats(toSpawn.TileStats);

        //Subscribe
        EventManager.instance.GE_TakeDamage.SubscribeOn(createdTile.TakeDamage);
        EventManager.instance.GE_Break.SubscribeOn(createdTile.Break);

        EventManager.instance.GE_TakeDamage.SubscribeAfter(createdTile.CheckBreak);
    }

    public void RemoveTile(HexTile tile)
    {
        EventManager.instance.GE_Break.UnsubscribeOn(tile.Break);
        EventManager.instance.GE_TakeDamage.UnsubscribeOn(tile.TakeDamage);

        EventManager.instance.GE_TakeDamage.UnsubscribeAfter(tile.CheckBreak);
        HexGrid.RemoveHexTile(tile);
    }

    private string GetHexTileName(Vector3Int hexPosition)
    {
        return $"HexTile({hexPosition.x}, {hexPosition.y})";
    }

    public HexTile GetHexTileForGameObject(GameObject gameObject)
    {
        Vector3Int gridPosition = Vector3Int.RoundToInt(gameObject.transform.position);
        return HexGrid.GetHexTileFromGridPosition(gridPosition);
    }

    public Vector3 GetWorldPositionToPlaceMoveableOnInGrid(Vector3Int gridPos)
    {
        return HexGrid.GetWorldPositionOfHex(gridPos);
    }

    public List<Vector3Int> GetQuickestPath(Vector3Int fromThisPosition, Vector3Int toThisPosition) {

        return HexGrid.FindPath(fromThisPosition, toThisPosition);
    }
}
