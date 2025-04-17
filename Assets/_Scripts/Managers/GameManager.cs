using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public static Action<GameState> OnBeforeGameStateChanged;
    public static Action<GameState> OnAfterGameStateChanged;
    public GameState State { get; private set; }

    void Start()
    {
        ChangeState(GameState.Starting);
    }

    public void ChangeState(GameState newState)
    {
        OnBeforeGameStateChanged?.Invoke(newState);
        Debug.Log($"The gamestate is now : {newState}");

        State = newState;
        switch (State)
        {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.SpawnHexGrid:
                HandleSpawnHexGrid(); 
                break;
            case GameState.SpawnCharacters:
                HandleSpawnCharacters();
                break;
            case GameState.GameStart:
                ChangeState(GameState.GameTurnStart);
                break;
            case GameState.GameTurnStart:
                ChangeState(GameState.UnitTurnStart);
                break;
            case GameState.UnitTurnStart:
                HandleUnitTurnStart();
                break;
            case GameState.UnitTurnEnd:
                HandleUnitTurnEnd();
                break;
            case GameState.GameTurnEnd:
                ChangeState(GameState.GameTurnStart);
                break;
            case GameState.GameOver:
                Debug.Log("The game is over, resetting the game");
                SceneManager.LoadScene("Game");
                EventManager.instance.ClearGameEvents();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterGameStateChanged?.Invoke(newState);
    }


    private void HandleStarting()
    {
        OnBeforeGameStateChanged += TurnManager.instance.OnBeforeGameStateChanged;
        EventManager.instance.GE_Fall.SubscribeAfter(CheckGameOver);
        ChangeState(GameState.SpawnHexGrid);
    }

    private void HandleSpawnHexGrid()
    {
        //Setup grid
        GridManager.instance.SpawnHexGrid();
        ChangeState(GameState.SpawnCharacters);
    }

    private void HandleSpawnCharacters()
    {
        //Setup units
        UnitManager.instance.SpawnCharacters();
        ChangeState(GameState.GameStart);
    }

    private void HandleUnitTurnEnd() 
    {
        InputManager.instance.ClearCurrentSelections();
        Debug.Log($"All Players have taken their turn? {TurnManager.instance.IsGameTurnOver()}");
        if (TurnManager.instance.IsGameTurnOver())
        {
            ChangeState(GameState.GameTurnEnd);
        }
        else
        {
            ChangeState(GameState.UnitTurnStart);
        }
    }

    private void HandleUnitTurnStart()
    {
        List<Ability> abilitiesForCurrentTurn = TurnManager.instance.GetCurrentTurnUnit() != null ? TurnManager.instance.GetCurrentTurnUnit().GetAbilities() : new();
        InputManager.instance.SetUIAbilities(abilitiesForCurrentTurn);
    }

    public void CheckGameOver(FallArg arg)
    {
        if (Helper.GetLivingUnits(UnitManager.instance.GetAllUnits()).Count == 0)
        {
            ChangeState(GameState.GameOver);
        }
    }



    public void ExecuteSelectedAbility()
    {
        Ability abilityToExecute = InputManager.instance.GetSelectedAbility();
        GameObject target = InputManager.instance.GetSelectedObject();
        Unit currentUnitTurn = TurnManager.instance.GetCurrentTurnUnit();
        if (target != null && abilityToExecute != null)
        {
            InputManager.instance.SetSelectedAbility(null);
            abilityToExecute.QueueAbility(currentUnitTurn, target);
            ChangeState(GameState.UnitTurnEnd);
        }
    }
}
