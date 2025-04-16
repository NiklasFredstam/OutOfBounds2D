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


    public void OnBeforeGameStateChanged(GameState gameState)
    {
        
        switch(gameState)
        {
            case GameState.GameTurnStart:
                UpdateUnitTurnOrder();
                break;
            case GameState.UnitTurnStart:
                UpdateUnitTurnOrder();
                SetCurrentTurnUnit();
                break;
            case GameState.UnitTurnEnd:
                _unitsAlreadyTakenTurn.Add(_currentTurnUnit);
                _currentTurnUnit = null;
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
        foreach(Unit unit in _unitTurnOrder)
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

    private void UpdateUnitTurnOrder()
    {
        _unitTurnOrder = Helper.GetLivingUnits(UnitManager.instance.GetAllUnits()).OrderBy(unit => unit.GetSpeed()).ToList();
    }

    private void SetCurrentTurnUnit()
    {

        _currentTurnUnit = _unitTurnOrder.FirstOrDefault(unit => !_unitsAlreadyTakenTurn.Contains(unit));
    }

}