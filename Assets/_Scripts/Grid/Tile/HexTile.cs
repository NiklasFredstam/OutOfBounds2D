using UnityEngine;

public class HexTile : MonoBehaviour
{
    [SerializeField]
    private Vector3Int _currentGridPosition;
    [SerializeField]
    private TileStats _tileStats;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _polygonCollider;
    private Color _originalColor;

    //Behaviour
    public Event<int> BeforeTileDamage = new Event<int>();
    public Event<int> AfterTileDamage = new Event<int>();
    public Event<HexTile> BeforeTileBreak = new Event<HexTile>();
    public Event<HexTile> AfterTileBreak = new Event<HexTile>();



    public void SetStats(TileStats tileStats) { _tileStats = tileStats; }

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _polygonCollider = GetComponent<PolygonCollider2D>();
        UpdateTilePriority();
    }


    //Lets the object keep track of its own position
    public void UpdateTilePosition(Vector3Int newCellPosition)
    {
        //TODO: the latter should be in a helper method. That will be needed more times
        if (_currentGridPosition == null || Vector3.Distance(_currentGridPosition, newCellPosition) != 0)
        {
            _currentGridPosition = newCellPosition;
            //TODO: move tile
            UpdateTilePriority();
            //TODO: probably call event BeforeMoveTile and AfterMoveTile
        }
    }

    //Updates the tiles priority in the grid
    private void UpdateTilePriority()
    {
        if (_spriteRenderer != null && _currentGridPosition != null)
        {
            //We'd also need to change the sprites on the quarter rotations to flat top hexagons

            int sortingPrio = Helper.GetSortingPriorityFromHexPosition(_currentGridPosition);
            //This sets the tile as highest priority if at the lowest rank. Crashes if we go over a million rows :)
            _spriteRenderer.sortingOrder = sortingPrio;
            _polygonCollider.layerOverridePriority = sortingPrio;
        }
    }

    private void OnMouseEnter()
    {
        DarkenColor();
    }

    private void OnMouseExit()
    {
        RestoreColor();
    }

    private void OnMouseDown()
    {
    }

    private void OnMouseUp()
    {
        Debug.Log($"Clicked grid at {_currentGridPosition.x} {_currentGridPosition.y}");
        InputManager.OnTileClick.Invoke(this);
    }


    public void TakeDamage(int amount)
    {
        BeforeTileDamage.Invoke(amount);

        _tileStats.CurrentHealth -= amount;
        _tileStats.CurrentHealth = Mathf.Max(_tileStats.CurrentHealth, 0); // Clamp to 0
        Debug.Log($"{gameObject.name} took {amount} damage. Remaining health: {_tileStats.CurrentHealth}");
        
        AfterTileDamage.Invoke(amount);

        if (ShouldBreak())
        {
            BreakTile();
        }
    }

    public void Repair(int amount)
    {
        if (_tileStats.CurrentHealth < _tileStats.BaseHealth)
        {
            _tileStats.CurrentHealth += amount;
            _tileStats.CurrentHealth = Mathf.Min(_tileStats.CurrentHealth, _tileStats.BaseHealth); // Clamp to 3
            Debug.Log($"{gameObject.name} repaired by {amount}. Current health: {_tileStats.CurrentHealth}");
        }
        else
        {
            Debug.Log($"{gameObject.name} repaired by {amount} but it is already full health");
        }

    }

    public void Reinforce(int amount)
    {
        _tileStats.CurrentHealth += amount;
        Debug.Log($"{gameObject.name} reinforced by {amount}. Current health: {_tileStats.CurrentHealth}");
    }

    private void BreakTile()
    {
        BeforeTileBreak.Invoke(this);

        Debug.Log($"{gameObject.name} was destroyed");
        gameObject.SetActive(false);

        AfterTileBreak.Invoke(this);
    }


    private bool ShouldBreak()
    {
        return _tileStats.CurrentHealth == 0;
    }

    private void DarkenColor()
    {
        _originalColor = _spriteRenderer.color;
        Color darkerColor = _originalColor * 0.5f; // Reduce brightness (50% darker)
        darkerColor.a = _originalColor.a; // Preserve original alpha
        _spriteRenderer.color = darkerColor;
    }

    private void RestoreColor()
    {
        if (_originalColor != null)
        {
            _spriteRenderer.color = _originalColor;
        }
    }
}
