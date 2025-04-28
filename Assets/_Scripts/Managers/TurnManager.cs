using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    [SerializeField]
    private int _gameTurnCounter = 0;
    [SerializeField]
    private List<Unit> _unitsAlreadyTakenTurn = new();
    [SerializeField]
    private List<Unit> _unitTurnOrder = new();
    [SerializeField]
    private Unit _currentTurnUnit;
    private PlayerAccount _currentTurnPlayer;

    public void OnBeforeGameStateChanged(GameState gameState)
    {
        UpdateUnitTurnOrder();
        switch (gameState)
        {
            case GameState.GameTurnStart:
                break;
            case GameState.UnitTurnStart:
                SetCurrentTurnUnit();
                SetCurrentTurnPlayer();
                break;
            case GameState.UnitTurnEnd:
                _unitsAlreadyTakenTurn.Add(_currentTurnUnit);
                _currentTurnUnit = null;
                _currentTurnPlayer = null;
                break;
            case GameState.GameTurnEnd:
                _unitsAlreadyTakenTurn.Clear();
                _gameTurnCounter++;
                break;
            default:
                break;
        }
    }

    public bool IsGameTurnOver()
    {
        foreach(Unit unit in MoveableManager.instance.GetLivingUnits())
        {
            if(!_unitsAlreadyTakenTurn.Contains(unit))
            {
                return false;
            }
        }
        return true;
    }

    public Unit GetCurrentTurnUnit()
    {
        return _currentTurnUnit;
    }


    public int GetCurrentGameTurn()
    {
        return _gameTurnCounter;
    }

    public bool ControlsCurrentTurnUnit()
    {
        return _currentTurnPlayer.IsLocalPlayer && _currentTurnUnit.GetControllingPlayer() == _currentTurnPlayer;
    }

    public bool IsAllowedToMoveUnit(Unit unit)
    {
        return unit.GetControllingPlayer().IsLocalPlayer && ControlsCurrentTurnUnit();
    }

    public void UpdateUnitTurnOrder()
    {
        _unitTurnOrder = MoveableManager.instance.GetLivingUnits().OrderBy(unit => unit.GetSpeed()).ToList();
    }

    private void SetCurrentTurnUnit()
    {
        _currentTurnUnit = _unitTurnOrder.FirstOrDefault(unit => !_unitsAlreadyTakenTurn.Contains(unit));
    }

    private void SetCurrentTurnPlayer()
    {
        _currentTurnPlayer = _currentTurnUnit.GetControllingPlayer();
    }

}