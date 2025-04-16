using System;
using UnityEngine;

public class HexTile : MonoBehaviour, ISelectable, IHovereable
{
    private SpriteRenderer _spriteRenderer;
    private Collider2D _polygonCollider;
    private Color _originalColor;

    [SerializeField]
    private Vector3Int _currentGridPosition;
    [SerializeField]
    private TileStats _tileStats;


    public void SetStats(TileStats tileStats) { _tileStats = tileStats; }

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _polygonCollider = GetComponent<PolygonCollider2D>();
        _originalColor = _spriteRenderer.color;
        UpdateTilePriority();
    }

    public void MovedTo(Moveable moveable)
    {
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

    public void TakeDamage(TakeDamageArg takeDamageArg)
    {
        if(takeDamageArg.TARGET == this)
        {
            _tileStats.CurrentHealth -= takeDamageArg.DAMAGE;
            _tileStats.CurrentHealth = Mathf.Max(_tileStats.CurrentHealth, 0); // Min health is 0
            Debug.Log($"{gameObject.name} took {takeDamageArg.DAMAGE} damage. Remaining health: {_tileStats.CurrentHealth}");
        }
    }

    public void Repair(int amount)
    {
        if (_tileStats.CurrentHealth < _tileStats.BaseHealth)
        {
            _tileStats.CurrentHealth += amount;
            _tileStats.CurrentHealth = Mathf.Min(_tileStats.CurrentHealth, _tileStats.BaseHealth); // Max heal is Basehealth
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
    public void CheckBreak(TakeDamageArg damageArg)
    {
        if (_tileStats.CurrentHealth == 0)
        {
            EventManager.instance.GE_Break.Invoke(new BreakArg(damageArg.UNIT_SOURCE, _currentGridPosition, this));
        }
    }

    public void Break(BreakArg breakArg)
    {
        if(breakArg.TILE == this)
        {
            Debug.Log($"{gameObject.name} was destroyed");
            gameObject.SetActive(false);
            GridManager.instance.RemoveTile(this);
        }
    }

    private void DarkenColor()
    {
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

    public void OnSelect()
    {
    }

    public void OnDeselect()
    {
    }

    public void OnHoverLeave()
    {
        RestoreColor();
    }

    public void OnHoverEnter()
    {
        DarkenColor();
    }

    public Vector3Int GetCurrentGridPosition()
    {
        return _currentGridPosition;
    }
}
