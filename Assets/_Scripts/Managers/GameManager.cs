using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GameManager : Singleton<GameManager>
{
    public static Event<GameState> OnBeforeStateChanged = new Event<GameState>();
    public static Event<GameState> OnAfterStateChanged = new Event<GameState>();

    public GameState State { get; private set; }

    void Start() => ChangeState(GameState.Starting);


    //Start turnmanager
    
    public Unit GetNextTurnUnit()
    {
        return CharacterManager.instance.GetLivingUnits()[0];
    }

    //End turnmanager

    public void ActionTileSelect(HexTile tile)
    {
        //Unsubscribe so it can't happen twice
        //Might even need fast click protection.
        InputManager.OnTileClick.Unsubscribe(ActionTileSelect);
        Unit unit = GetNextTurnUnit();
        //If it's a unit move

        //If its target ability

        //If its a attack tile


        switch (State)
        {
            case GameState.AttackTileSelect:
                unit.OnDealDamage.Subscribe(tile.TakeDamage);
                unit.DealDamage();
                unit.OnDealDamage.Unsubscribe(tile.TakeDamage);
                break;
            case GameState.MoveToTileSelect:
                GridManager.instance.MoveMoveableToHex(unit, tile);
                break;
            default:
                break;
        }

        ChangeState(State == GameState.AttackTileSelect ? GameState.MoveToTileSelect : GameState.AttackTileSelect);

    }

    public void ChangeState(GameState newState)
    {
        OnBeforeStateChanged.Invoke(newState);

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
            case GameState.CreateUI:
                HandleCreateUI();
                break;
            case GameState.NextTurn:
                ExecuteTurn();
                break;
            case GameState.AttackTileSelect:
                InputManager.OnTileClick.Subscribe(ActionTileSelect);
                break;
            case GameState.MoveToTileSelect:
                InputManager.OnTileClick.Subscribe(ActionTileSelect);
                break;
            case GameState.GameOver:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged.Invoke(newState);

        Debug.Log($"The state is now: {newState}");
    }


    private void HandleStarting()
    {
        //Setup whatever

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
        CharacterManager.instance.SpawnCharacters();
        ChangeState(GameState.AttackTileSelect);
    }

    private void HandleCreateUI()
    {

    }

    private void HandleTurnEnd() 
    {
        if (IsGameOver())
        {
            ChangeState(GameState.GameOver);
        }
    }

    private void ExecuteTurn()
    {
        ChangeState(GameState.AttackTileSelect);
        //Wait for input
        //Execute Input
        //Check game over
        //otherwise ChangeState(NextTurn)

        //I
    }

    private bool IsGameOver()
    {
        return CharacterManager.instance.GetLivingUnits().Count == 0;
    }
}
