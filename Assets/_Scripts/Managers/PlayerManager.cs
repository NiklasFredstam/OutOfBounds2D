using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField]
    private Dictionary<string, PlayerAccount> _connectedPlayers = new();

    public void RegisterPlayer(string userID, string displayName, bool isLocal)
    {
        PlayerAccount account = new (userID, displayName, isLocal);
        _connectedPlayers[userID] = account;
    }

    public PlayerAccount GetPlayerById(string id)
    {
        if (_connectedPlayers.ContainsKey(id))
        {
            return _connectedPlayers[id];
        }
        else
        {
            return null;
        }
    }

    public List<PlayerAccount> GetPlayersStillInTheGame()
    {
        return _connectedPlayers.Values.Where(player => player.HasAnyActiveUnits).ToList();
    }
    public List<Character> GetAllAllyCharacters(PlayerAccount account)
    {
        List<Character> allCharacters = MoveableManager.instance.GetAllCharacters();
        return allCharacters.Where(character => character.GetControllingPlayer() == account).ToList();
    }

    public List<Unit> GetAllAllyUnits(PlayerAccount account)
    {
        List<Unit> allUnits = MoveableManager.instance.GetAllUnits();
        return allUnits.Where(unit => unit.GetControllingPlayer() == account).ToList();
    }

    public List<Unit> GetAllEnemyUnits(PlayerAccount account)
    {
        List<Unit> allUnits = MoveableManager.instance.GetAllUnits();
        return allUnits.Where(unit => unit.GetControllingPlayer() != account).ToList();
    }

}