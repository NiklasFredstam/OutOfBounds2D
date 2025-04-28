using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        new (3,0),
        new (-1,1),
        new (0,1),
        new (1,1),
        new (2,1),
        new (3,1),
        new (-1,2),
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

        EventManager.instance.UI_Targettable.SubscribeOn(createdTile.OnTargettable);
        EventManager.instance.UI_Select.SubscribeOn(createdTile.OnSelect);
        EventManager.instance.UI_Hover.SubscribeOn(createdTile.OnHover);

    }

    public void RemoveTile(HexTile tile)
    {
        EventManager.instance.GE_Break.UnsubscribeOn(tile.Break);
        EventManager.instance.GE_TakeDamage.UnsubscribeOn(tile.TakeDamage);

        EventManager.instance.GE_TakeDamage.UnsubscribeAfter(tile.CheckBreak);

        EventManager.instance.UI_Targettable.UnsubscribeOn(tile.OnTargettable);
        EventManager.instance.UI_Select.UnsubscribeOn(tile.OnSelect);
        EventManager.instance.UI_Hover.UnsubscribeOn(tile.OnHover);

        HexGrid.RemoveHexTile(tile);
    }

    private string GetHexTileName(Vector3Int hexPosition)
    {
        return $"HexTile({hexPosition.x}, {hexPosition.y})";
    }

    public HexTile GetHexTileForGameObject(GameObject gameObject)
    {
        return HexGrid.GetHexTileFromWorldPosition(gameObject.transform.position);
    }


    public Vector3Int GetCellPositionOfGameObject(GameObject gameObject)
    {
        return HexGrid.GetGridPositionOfWorldPos(gameObject.transform.position);
    }

    public Vector3 GetWorldPositionToPlaceMoveableOnInGrid(Vector3Int gridPos)
    {
        return HexGrid.GetWorldPositionOfHex(gridPos);
    }


    public List<Vector3Int> GetQuickestPath(Vector3Int fromThisPosition, Vector3Int toThisPosition) {

        List<Vector3Int> path = HexGrid.FindPath(fromThisPosition, toThisPosition, MoveableManager.instance.GetOccupiedPositions());
        path?.RemoveAt(0);
        return path;
    }

    public List<Vector3Int> GetPositionsWithinRange(Vector3Int fromPosition, int range)
    {
        List<Vector3Int> positions = HexGrid.GetCellCoordinatesInRange(fromPosition, range);
        return positions;
    }

    public List<Vector3Int> GetPositionsWithinRangeExcludingCenter(Vector3Int fromPosition, int range)
    {
        List<Vector3Int> positions = HexGrid.GetCellCoordinatesInRange(fromPosition, range);
        positions.Remove(fromPosition);
        return positions;
    }

    public HexDirection GetDirectionTo(Vector3Int fromPos, Vector3Int toPos)
    {
        return HexGrid.GetDirectionEnumTo(fromPos, toPos);
    }

    public List<Vector3Int> GetLineInDirection(Vector3Int fromPos, HexDirection direction, int length)
    {
        return HexGrid.GetLineInDirection(fromPos, direction, length);
    }

    public List<Vector3Int> GetLineForPush(Vector3Int fromPos, HexDirection direction, int length)
    {
        List<Vector3Int> line = HexGrid.GetLineInDirection(fromPos, direction, length);
        List<Vector3Int> traversableLine = new();
        foreach (Vector3Int pos in line)
        {
            if (!IsTileTraversable(pos))
            {
                //If something is in the way, then we just stop here.
                break;
            }
            traversableLine.Add(pos);
            if(!HexTileExistsAtPosition(pos))
            {
                //If the tile is missing we go over the edge and then fall.
                break;
            }
        }
        return traversableLine;
    }

    public bool HexTileExistsAtPosition(Vector3Int position)
    {
        return HexGrid.PositionHasHexTile(position);
    }

    public bool IsTileTraversable(Vector3Int position)
    {
        return !MoveableManager.instance.GetOccupiedPositions().Contains(position);
    }
}
