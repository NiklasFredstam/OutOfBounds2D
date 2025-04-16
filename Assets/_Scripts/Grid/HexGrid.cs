using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    [SerializeField]
    private Grid _grid;
    private readonly Dictionary<Vector3Int, HexTile> _hexTileDictionary = new Dictionary<Vector3Int, HexTile>();


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

    public List<HexTile> FindPathBetween(HexTile start, HexTile goal)
    {
        List<HexTile> result = new List<HexTile>();
        List<Vector3Int> resultPositions = FindPath(GetGridPositionOfHex(start.transform.position), GetGridPositionOfHex(goal.transform.position));
        if(resultPositions == null) { return null; }
        foreach (Vector3Int gridPos in resultPositions)
        {
            result.Add(_hexTileDictionary[gridPos]);
        }
        return result;
    }

    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal)
    {
        if (!_hexTileDictionary.ContainsKey(start) || !_hexTileDictionary.ContainsKey(goal))
        {
            Debug.LogError("Start or Goal node does not exist in the dictionary.");
            return null;
        }

        if (!_hexTileDictionary[start].gameObject.activeSelf || !_hexTileDictionary[goal].gameObject.activeSelf)
        {
            Debug.LogError("Start or Goal node is not traversable.");
            return null;
        }

        var openSet = new PriorityQueue<Vector3Int>();
        var cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        var gScore = new Dictionary<Vector3Int, float>();
        var fScore = new Dictionary<Vector3Int, float>();

        foreach (var key in _hexTileDictionary.Keys)
        {
            gScore[key] = float.MaxValue;
            fScore[key] = float.MaxValue;
        }

        gScore[start] = 0;
        fScore[start] = Heuristic(start, goal);
        openSet.Enqueue(start, fScore[start]);

        while (openSet.Count > 0)
        {
            Vector3Int current = openSet.Dequeue();

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            foreach (Vector3Int neighbor in GetNeighbors(current))
            {
                if (!_hexTileDictionary.ContainsKey(neighbor) || !_hexTileDictionary[neighbor].gameObject.activeSelf)
                    continue;

                float tentativeGScore = gScore[current] + Cost(current, neighbor);

                if (tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + Heuristic(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                        openSet.Enqueue(neighbor, fScore[neighbor]);
                }
            }
        }

        Debug.LogError("Path not found.");
        return null;
    }
    private List<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
    {
        List<Vector3Int> path = new List<Vector3Int> { current };

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }

        path.Reverse();
        return path;
    }

    public List<HexTile> GetAllTiles() => _hexTileDictionary.Values.ToList();

    private float Heuristic(Vector3Int a, Vector3Int b)
    {
        return Vector3Int.Distance(a, b);
    }

    private float Cost(Vector3Int a, Vector3Int b)
    {
        return 1.0f; // Uniform cost; modify if needed
    }

    private List<Vector3Int> GetNeighbors(Vector3Int hex)
    {
        List<Vector3Int> directions = new List<Vector3Int>
        {
            new(1, -1, 0),
            new(1, 0, -1),
            new(0, 1, -1),
            new(-1, 1, 0),
            new(-1, 0, 1),
            new(0, -1, 1)
        };

        List<Vector3Int> neighbors = new List<Vector3Int>();

        foreach (var direction in directions)
        {
            Vector3Int neighbor = hex + direction;
            if (_hexTileDictionary.ContainsKey(neighbor))
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }


}
public class PriorityQueue<T>
{
    private readonly List<KeyValuePair<T, float>> _elements = new List<KeyValuePair<T, float>>();

    public int Count => _elements.Count;

    public void Enqueue(T item, float priority)
    {
        _elements.Add(new KeyValuePair<T, float>(item, priority));
    }

    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < _elements.Count; i++)
        {
            if (_elements[i].Value < _elements[bestIndex].Value)
            {
                bestIndex = i;
            }
        }

        T bestItem = _elements[bestIndex].Key;
        _elements.RemoveAt(bestIndex);
        return bestItem;
    }

    public bool Contains(T item)
    {
        foreach (var element in _elements)
        {
            if (EqualityComparer<T>.Default.Equals(element.Key, item))
            {
                return true;
            }
        }

        return false;
    }
}

