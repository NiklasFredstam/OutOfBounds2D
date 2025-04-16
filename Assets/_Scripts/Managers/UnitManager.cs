using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class UnitManager : Singleton<UnitManager>
{

    //this will ALWAYS keep them. It wont remove them. We simply get all from here.
    //Then we can return living ones
    private List<Unit> _allUnits = new();
    private List<Character> _allCharacters = new();

    public void SpawnCharacters()
    {
        SpawnCharacter(CharacterType.Michi, new Vector3Int(0,0));
        SpawnCharacter(CharacterType.Jibblon, new Vector3Int(2, 2));
    }


    //Make type into enum
    private void SpawnCharacter(CharacterType type, Vector3Int gridPosition)
    {
        //Get resource
        CharacterScriptable charScriptable = ResourceSystem.instance.GetCharacter(type);

        //Initiate recource
        Character spawned = Instantiate(charScriptable.Prefab, GridManager.instance.GetWorldPositionToPlaceMoveableOnInGrid(gridPosition), Quaternion.identity);

        //Set parent object for keeping things tidy
        GameObject characterContainer = GameObject.Find("Characters");
        spawned.transform.SetParent(characterContainer.transform);

        //Set Stats
        spawned.SetCharacterStats(charScriptable.CharacterStats);
        spawned.SetMoveableStats(charScriptable.MoveableStats);
        spawned.SetUnitStats(charScriptable.UnitStats);
        spawned.SetCurrentPosition(gridPosition);

        //Set abilities
        //Maybe move this to ability manager?
        spawned.SetAbilities(AbilityManager.instance.CreateAbilities(charScriptable.Abilities));


        //Subscribe to events
        EventManager.instance.GE_DoDamage.SubscribeOn(spawned.DoDamage);
        EventManager.instance.GE_Fall.SubscribeOn(spawned.Fall);
        EventManager.instance.GE_UseAbility.SubscribeOn(spawned.UseAbility);
        EventManager.instance.GE_Move.SubscribeOn(spawned.Move);

        EventManager.instance.GE_Break.SubscribeAfter(spawned.CheckFall);

        //Add to lists
        _allUnits.Add(spawned);
        _allCharacters.Add(spawned);
    }

    public List<Unit> GetAllUnits()
    {
        return _allUnits;
    }

    public void RemoveCharacter(Character character)
    {
        EventManager.instance.GE_DoDamage.UnsubscribeOn(character.DoDamage);
        EventManager.instance.GE_Fall.UnsubscribeOn(character.Fall);
        EventManager.instance.GE_UseAbility.UnsubscribeOn(character.UseAbility);
        EventManager.instance.GE_Move.UnsubscribeOn(character.Move);

        EventManager.instance.GE_Break.UnsubscribeAfter(character.CheckFall);

    }

}
