using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour, IInspectable
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Vector3Int _currentGridPosition;
    [SerializeField]
    private TileStats _tileStats;

    private BlinkDarker _blinkDarker;
    private SelectDarker _selectDarker;
    private HoverDarker _hoverDarker;

    public void SetStats(TileStats tileStats) { _tileStats = tileStats; }

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _blinkDarker = GetComponent<BlinkDarker>();
        _selectDarker = GetComponent<SelectDarker>();
        _hoverDarker = GetComponent<HoverDarker>();
        UpdateTilePriority();
    }

    public void MovedTo(Moveable moveable)
    {
    }


    //Lets the object keep track of its own position
    public void UpdateTilePosition(Vector3Int newCellPosition)
    {
        if (_currentGridPosition == null || Vector3.Distance(_currentGridPosition, newCellPosition) != 0)
        {
            _currentGridPosition = newCellPosition;
            UpdateTilePriority();
        }
    }

    private void UpdateTilePriority()
    {
        if (_spriteRenderer != null && _currentGridPosition != null)
        {
            //We'd also need to change the sprites on the quarter rotations to flat top hexagons
            int sortingPrio = Helper.GetSortingPriorityFromHexPosition(_currentGridPosition);
            _spriteRenderer.sortingOrder = sortingPrio;
        }
    }

    public void TakeDamage(TakeDamageArg takeDamageArg)
    {
        if(takeDamageArg.TARGET == this)
        {
            _tileStats.CurrentHealth -= takeDamageArg.DAMAGE;
            _tileStats.CurrentHealth = Mathf.Max(_tileStats.CurrentHealth, 0);
            Debug.Log($"{gameObject.name} took {takeDamageArg.DAMAGE} damage. Remaining health: {_tileStats.CurrentHealth}");
        }
    }

    public void Repair(int amount)
    {
        if (_tileStats.CurrentHealth < _tileStats.BaseHealth)
        {
            _tileStats.CurrentHealth += amount;
            _tileStats.CurrentHealth = Mathf.Min(_tileStats.CurrentHealth, _tileStats.BaseHealth);
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

    public void OnHover(HoverArg hoverArg)
    {
        if (hoverArg.HOVERED_OBJECT == gameObject)
        {
            _hoverDarker.SetDarker();
            UIManager.instance.SetDetailsWindowInfo(this);
        }
        else
        {
            _hoverDarker.ResetColor();
        }
    }

    public void OnSelect(SelectArg selectArg)
    {
        if (selectArg.SELECTED_OBJECT == gameObject)
        {
            _selectDarker.SetDarker();

        }
        else
        {
            _selectDarker.ResetColor();
        }
    }

    public void OnTargettable(TargettableArg targettableArg)
    {
        if (Helper.IsTileSelectable(targettableArg.TARGETTABLE) && targettableArg.TARGETTABLE_POSITIONS.Contains(_currentGridPosition))
        {
            _blinkDarker.StartFadeLoop();
        } else
        {
            _blinkDarker.StopFadeLoop();
        }
    }

    public int GetHealth()
    {
        return _tileStats.CurrentHealth;
    }


    public Vector3Int GetCurrentGridPosition()
    {
        return _currentGridPosition;
    }

    public List<StatDetail> GetDetails()
    {
        StatDetail detail = new StatDetail("Health", GetHealth(), GetHealth() > _tileStats.BaseHealth, GetHealth() < _tileStats.BaseHealth, DetailStatType.TileHealth);
        return new() { detail };
    }

    public string GetDescription()
    {
        return "Tile Description";
    }

    public Sprite GetObjectSprite()
    {
        return _spriteRenderer.sprite;
    }


}
