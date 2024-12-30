using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    //rename to unit manager


    //this will ALWAYS keep them. It wont remove them. We simply get all from here.
    //Then we can return living ones
    private List<Unit> _allUnits = new List<Unit>();
    private List<Character> _allCharacters = new List<Character>();

    public void SpawnCharacters()
    {
        SpawnCharacter(CharacterType.Michi, new Vector3Int(0,0));
    }


    //Make type into enum
    private void SpawnCharacter(CharacterType type, Vector3Int gridPosition)
    {
        //Get resource
        CharacterScriptable charScriptable = ResourceSystem.instance.GetCharacter(type);

        //Initiate recource
        Character spawned = Instantiate(charScriptable.Prefab, gridPosition, Quaternion.identity);

        //Set parent object for keeping things tidy
        GameObject characterContainer = GameObject.Find("Characters");
        spawned.transform.SetParent(characterContainer.transform);

        //Set Stats
        spawned.SetCharacterStats(charScriptable.CharacterStats);
        spawned.SetMoveableStats(charScriptable.MoveableStats);
        spawned.SetUnitStats(charScriptable.UnitStats);

        SubscribeUnitToFirstGridTile(spawned, gridPosition);

        //Add to lists
        _allUnits.Add(spawned);
        _allCharacters.Add(spawned);
    }

    private void SubscribeUnitToFirstGridTile(Unit unit, Vector3Int gridPosition)
    {
        HexTile spawnTile = GridManager.instance.GetHexTileFromGrid(gridPosition);
        spawnTile.AfterTileBreak.Subscribe(unit.Fall);
    }

    public List<Unit> GetLivingUnits()
    {
        return new List<Unit>(_allUnits.Where(unit => unit.UnitStats.Alive));
    }

}
