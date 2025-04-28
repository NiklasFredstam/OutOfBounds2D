using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public static Action<GameState> OnBeforeGameStateChanged;
    public static Action<GameState> OnAfterGameStateChanged;
    public GameState State { get; private set; }


    public bool IsGameOver = false;

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
                if(!IsGameOver)
                {
                    HandleGameOver();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterGameStateChanged?.Invoke(newState);
    }


    private void HandleStarting()
    {
        //get into game premises from some menu manager. like
        //get players, selected characters, selected starting positions, generate map(?)
        PlayerManager.instance.RegisterPlayer("1", "Frälsaren", true);
        PlayerManager.instance.RegisterPlayer("2", "Jibbletron", true);


        OnBeforeGameStateChanged += TurnManager.instance.OnBeforeGameStateChanged;
        EventManager.instance.GE_Fall.SubscribeAfter(CheckGameOver);
        ChangeState(GameState.SpawnHexGrid);
    }

    private void HandleGameOver()
    {
        Debug.Log("The game is over it resulted in a " +
                  (PlayerManager.instance.GetPlayersStillInTheGame().Count == 0 ? "TIE" : "WIN") +
                  " , resetting the game");
        IsGameOver = true;

        SceneManager.LoadScene("Game");
        EventManager.instance.ClearGameEvents();
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
        MoveableManager.instance.SpawnCharacters();
        ChangeState(GameState.GameStart);
    }

    private void HandleUnitTurnEnd() 
    {

        UIManager.instance.DestroyAbilityButtons();
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
        if (TurnManager.instance.ControlsCurrentTurnUnit() && !IsGameOver)
        {
            List<Ability> abilitiesForCurrentTurn = TurnManager.instance.GetCurrentTurnUnit() != null ? TurnManager.instance.GetCurrentTurnUnit().GetAbilities() : new();
            UIManager.instance.CreateAbilityButtons(abilitiesForCurrentTurn, TurnManager.instance.GetCurrentTurnUnit());
        }
    }

    public void CheckGameOver(FallArg arg)
    {
        List<PlayerAccount> playersStillInTheGame = PlayerManager.instance.GetPlayersStillInTheGame();
        foreach (PlayerAccount player in playersStillInTheGame)
        {
            CheckGameOverForPlayer(player);
        }

        List<PlayerAccount> playersStillInTheGameAfterElimination = PlayerManager.instance.GetPlayersStillInTheGame();
        if (playersStillInTheGameAfterElimination.Count <= 1)
        {
            ChangeState(GameState.GameOver);
        }
    }

    public void CheckGameOverForPlayer(PlayerAccount player)
    {
        List<Character> characters = PlayerManager.instance.GetAllAllyCharacters(player);
        bool anyAlive = characters.Any(character => character.IsAlive());

        if (!anyAlive)
        {
            player.Eliminate();
        }
    }


    public bool IsValidTarget(GameObject target)
    {
        return target != null && 
            InputManager.instance.GetPositionsWithinRange().Contains(GridManager.instance.GetCellPositionOfGameObject(target));
    }


    public void ExecuteSelectedAbility()
    {
        Ability abilityToExecute = InputManager.instance.GetSelectedAbility();
        GameObject target = InputManager.instance.GetSelectedObject();
        Unit currentUnitTurn = TurnManager.instance.GetCurrentTurnUnit();
        if (IsValidTarget(target) && abilityToExecute != null)
        {
            InputManager.instance.SetSelectedAbility(null);
            abilityToExecute.CommitAbility(currentUnitTurn, target);
            ChangeState(GameState.UnitTurnEnd);
        }
    }
}
