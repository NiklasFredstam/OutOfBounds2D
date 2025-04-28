using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class HexGrid : MonoBehaviour
{
    [SerializeField]
    private Grid _grid;
    private readonly Dictionary<Vector3Int, HexTile> _hexTileDictionary = new();


    public void RemoveHexTile(HexTile hexTile)
    {
        _hexTileDictionary.Remove(GetGridPositionOfWorldPos(hexTile.transform.position));
    }

    public void AddHexTile(HexTile hexTile)
    {
        _hexTileDictionary.Add(GetGridPositionOfWorldPos(hexTile.transform.position), hexTile);
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
        return _hexTileDictionary[GetGridPositionOfWorldPos(worldPosition)];
    }


    public Vector3Int GetGridPositionOfWorldPos(Vector3 worldPosition)
    {
        return _grid.WorldToCell(worldPosition);
    }



    public List<HexTile> GetAllTiles() => _hexTileDictionary.Values.ToList();

    public bool PositionHasHexTile(Vector3Int position)
    {
        return _hexTileDictionary.ContainsKey(position);
    }

    private static readonly Dictionary<HexDirection, Vector3Int> evenRowOffsets = new()
    {
        { HexDirection.E,  new(+1,  0, 0) },
        { HexDirection.NE, new(0,  +1, 0) },
        { HexDirection.NW, new(-1, +1, 0) },
        { HexDirection.W,  new(-1,  0, 0) },
        { HexDirection.SW, new(-1, -1, 0) },
        { HexDirection.SE, new(0,  -1, 0) },
    };

    private static readonly Dictionary<HexDirection, Vector3Int> oddRowOffsets = new()
    {
        { HexDirection.E,  new(+1,  0, 0) },
        { HexDirection.NE, new(+1, +1, 0) },
        { HexDirection.NW, new(0,  +1, 0) },
        { HexDirection.W,  new(-1,  0, 0) },
        { HexDirection.SW, new(0,  -1, 0) },
        { HexDirection.SE, new(+1, -1, 0) },
    };

    private IEnumerable<Vector3Int> GetHexNeighbors(Vector3Int cellPos)
    {
        bool isOdd = cellPos.y % 2 != 0; // <-- Use Y now if row-offset
        var offsets = isOdd ? oddRowOffsets.Values : evenRowOffsets.Values;

        foreach (var offset in offsets)
        {
            Vector3Int neighbor = cellPos + offset;
            if (_hexTileDictionary.ContainsKey(neighbor))
                yield return neighbor;
        }
    }

    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal, List<Vector3Int> nonTraversablePositions)
    {
        nonTraversablePositions = nonTraversablePositions
            .Where(pos => pos != start)
            .ToList();
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
                if (!_hexTileDictionary.ContainsKey(neighbor) || nonTraversablePositions.Contains(neighbor))
                {
                    continue; // skip out-of-bounds and non traversable tiles
                }
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
        Vector3Int ac = OffsetToCube(a);
        Vector3Int bc = OffsetToCube(b);
        return Mathf.Max(Mathf.Abs(ac.x - bc.x), Mathf.Abs(ac.y - bc.y), Mathf.Abs(ac.z - bc.z));
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

    public List<Vector3Int> GetCellCoordinatesInRange(Vector3Int center, int range)
    {
        List<Vector3Int> result = new List<Vector3Int>();
        Vector3Int centerCube = OffsetToCube(center);
        for (int dx = -range; dx <= range; dx++)
        {
            for (int dy = Mathf.Max(-range, -dx - range); dy <= Mathf.Min(range, -dx + range); dy++)
            {
                int dz = -dx - dy;
                Vector3Int neighborCube = new (centerCube.x + dx, centerCube.y + dy, centerCube.z + dz);

                Vector3Int neighborOffset = CubeToOffset(neighborCube);
                if (_hexTileDictionary.ContainsKey(neighborOffset))
                    result.Add(neighborOffset);
            }
        }
        return result;
    }
    private Vector3Int OffsetToCube(Vector3Int offset)
    {
        int x = offset.x - (offset.y - (offset.y & 1)) / 2;
        int z = offset.y;
        int y = -x - z;
        return new Vector3Int(x, y, z);
    }

    private Vector3Int CubeToOffset(Vector3Int cube)
    {
        int col = cube.x + (cube.z - (cube.z & 1)) / 2;
        int row = cube.z;
        return new Vector3Int(col, row, 0);
    }

    public HexDirection GetDirectionEnumTo(Vector3Int from, Vector3Int to)
    {
        if (from == to)
            throw new System.ArgumentException("Source and target positions are the same.");

        bool isOdd = from.y % 2 != 0;
        var offsets = isOdd ? oddRowOffsets : evenRowOffsets;

        HexDirection bestDirection = HexDirection.E;
        int bestDist = int.MaxValue;

        foreach (var pair in offsets)
        {
            Vector3Int neighbor = from + pair.Value;
            int dist = HexDistance(neighbor, to);
            if (dist < bestDist)
            {
                bestDist = dist;
                bestDirection = pair.Key;
            }
        }

        return bestDirection;
    }

    public List<Vector3Int> GetLineInDirection(Vector3Int start, HexDirection directionIndex, int length)
    {
        var result = new List<Vector3Int>();
        Vector3Int current = start;

        for (int i = 0; i < length; i++)
        { 
            bool isOdd = current.y % 2 != 0;
            Vector3Int offset = isOdd ? oddRowOffsets[directionIndex] : evenRowOffsets[directionIndex];
            current += offset;
            result.Add(current);
        }

        result.Remove(start);   
        return result;
    }
}