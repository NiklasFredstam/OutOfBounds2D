using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    [SerializeField]
    private Grid _grid;
    private readonly Dictionary<Vector3Int, HexTile> _hexTileDictionary = new();


    public void RemoveHexTile(HexTile hexTile)
    {
        _hexTileDictionary.Remove(GetGridPositionOfHex(hexTile.transform.position));
    }

    public void AddHexTile(HexTile hexTile)
    {
        _hexTileDictionary.Add(GetGridPositionOfHex(hexTile.transform.position), hexTile);
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



    public List<HexTile> GetAllTiles() => _hexTileDictionary.Values.ToList();


    private static readonly Vector3Int[] evenRowOffsets = new Vector3Int[]
    {
    new(+1, 0, 0),     // E
    new(0, +1, 0),     // NE
    new(-1, +1, 0),    // NW
    new(-1, 0, 0),     // W
    new(-1, -1, 0),    // SW
    new(0, -1, 0),     // SE
    };

    private static readonly Vector3Int[] oddRowOffsets = new Vector3Int[]
    {
    new(+1, 0, 0),     // E
    new(+1, +1, 0),    // NE
    new(0, +1, 0),     // NW
    new(-1, 0, 0),     // W
    new(0, -1, 0),     // SW
    new(+1, -1, 0),    // SE
    };

    private IEnumerable<Vector3Int> GetHexNeighbors(Vector3Int cellPos)
    {
        bool isOdd = cellPos.y % 2 != 0; // <-- Use Y now if row-offset
        var offsets = isOdd ? oddRowOffsets : evenRowOffsets;

        foreach (var offset in offsets)
        {
            Vector3Int neighbor = cellPos + offset;
            if (_hexTileDictionary.ContainsKey(neighbor))
                yield return neighbor;
        }
    }

    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal)
    {
        var openSet = new HashSet<Vector3Int> { start };
        var cameFrom = new Dictionary<Vector3Int, Vector3Int>();

        var gScore = new Dictionary<Vector3Int, float> { [start] = 0 };
        var fScore = new Dictionary<Vector3Int, float> { [start] = HexDistance(start, goal) };

        while (openSet.Count > 0)
        {
            Vector3Int current = openSet.OrderBy(n => fScore.GetValueOrDefault(n, float.MaxValue)).First();

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            openSet.Remove(current);

            foreach (var neighbor in GetHexNeighbors(current))
            {
                if (!_hexTileDictionary.ContainsKey(neighbor)) continue; // skip out-of-bounds tiles

                float tentativeGScore = gScore[current] + GetCost(current, neighbor);
                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + HexDistance(neighbor, goal);
                    openSet.Add(neighbor);
                }
            }
        }

        return null; // no path found
    }

    private float GetCost(Vector3Int from, Vector3Int to)
    {
        return 1f; // Or _hexTileDictionary[to].Cost if you have terrain costs
    }

    private int HexDistance(Vector3Int a, Vector3Int b)
    {
        // Convert to cube coordinates for distance
        int x1 = a.x;
        int y1 = a.y - (a.x - (a.x & 1)) / 2;
        int z1 = -x1 - y1;

        int x2 = b.x;
        int y2 = b.y - (b.x - (b.x & 1)) / 2;
        int z2 = -x2 - y2;

        return Mathf.Max(Mathf.Abs(x1 - x2), Mathf.Abs(y1 - y2), Mathf.Abs(z1 - z2));
    }

    private List<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
    {
        var path = new List<Vector3Int> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }
        return path;
    }
}