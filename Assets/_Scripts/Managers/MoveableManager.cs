using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class MoveableManager : Singleton<MoveableManager>
{

    private readonly List<Unit> _allUnits = new();
    private readonly List<Character> _allCharacters = new();
    private readonly List<Moveable> _allMoveable = new();

    public void SpawnCharacters()
    {
        SpawnCharacter(CharacterType.Michi, new Vector3Int(0,0), PlayerManager.instance.GetPlayerById("1"));
        SpawnCharacter(CharacterType.Jibblon, new Vector3Int(2, 2), PlayerManager.instance.GetPlayerById("2"));
        SpawnCharacter(CharacterType.Phillzor, new Vector3Int(1, 1), PlayerManager.instance.GetPlayerById("2"));    

    }

    public void SpawnMoveableObject(MoveableObjectType type, Vector3Int gridPosition, Unit sourceUnit)
    {
        MoveableScriptable moveableScriptable = ResourceSystem.instance.GetMoveableObject(type);
        Moveable spawnedMoveable = Instantiate(moveableScriptable.Prefab, GridManager.instance.GetWorldPositionToPlaceMoveableOnInGrid(gridPosition), Quaternion.identity);
        GameObject moveableObjectsContainer = GameObject.Find("MoveableObjects");
        spawnedMoveable.transform.SetParent(moveableObjectsContainer.transform);
        spawnedMoveable.SetMoveableStats(moveableScriptable.MoveableStats);
        spawnedMoveable.SetCurrentPosition(gridPosition);
        spawnedMoveable.gameObject.name = "Bomb";
        spawnedMoveable.SetSourceBasedProperties(sourceUnit);

        SubscribeMoveableMethods(spawnedMoveable);

        //Add to lists
        _allMoveable.Add(spawnedMoveable);
    }

    private void SubscribeBombMethods(Bomb bombMoveable)
    {
        GameManager.OnBeforeGameStateChanged += bombMoveable.DecrementTimer;
    }

    private void UnsubscribeBombMethods(Bomb bombMoveable)
    {
        GameManager.OnBeforeGameStateChanged -= bombMoveable.DecrementTimer;
    }

    private void SubscribeUnitMethods(Unit spawned)
    {
        EventManager.instance.GE_DoDamage.SubscribeOn(spawned.DoDamage);
        EventManager.instance.GE_UseAbility.SubscribeOn(spawned.UseAbility);
        EventManager.instance.GE_Push.SubscribeOn(spawned.Push);

        SubscribeMoveableMethods(spawned);
    }

    private void UnsubscribeUnitMethods(Unit unit)
    {
        EventManager.instance.GE_DoDamage.UnsubscribeOn(unit.DoDamage);
        EventManager.instance.GE_UseAbility.UnsubscribeOn(unit.UseAbility);
        EventManager.instance.GE_Push.UnsubscribeOn(unit.Push);
        UnsubscribeMoveableMethods(unit);
    }


    private void SubscribeCharacterMethods(Character spawned)
    {

        SubscribeUnitMethods(spawned);
    }

    private void UnsubscribeCharacterMethods(Character character)
    {

        UnsubscribeUnitMethods(character);
    }

    private void SubscribeMoveableMethods(Moveable spawnedMoveable)
    {
        EventManager.instance.GE_Fall.SubscribeOn(spawnedMoveable.Fall);
        EventManager.instance.GE_Move.SubscribeOn(spawnedMoveable.Move);
        EventManager.instance.GE_GetPushed.SubscribeOn(spawnedMoveable.GetPushed);

        EventManager.instance.GE_Break.SubscribeAfter(spawnedMoveable.CheckFall);
        EventManager.instance.GE_Move.SubscribeAfter(spawnedMoveable.CheckFall);

        EventManager.instance.UI_Targettable.SubscribeOn(spawnedMoveable.OnTargettable);
        EventManager.instance.UI_Select.SubscribeOn(spawnedMoveable.OnSelect);
        EventManager.instance.UI_Hover.SubscribeOn(spawnedMoveable.OnHover);

        switch (spawnedMoveable)
        {
            case Bomb bomb:
                SubscribeBombMethods(bomb);
                break;
            default:
                break;
        }
    }

    private void UnsubscribeMoveableMethods(Moveable moveable)
    {
        EventManager.instance.GE_Fall.UnsubscribeOn(moveable.Fall);
        EventManager.instance.GE_Move.UnsubscribeOn(moveable.Move);
        EventManager.instance.GE_GetPushed.UnsubscribeOn(moveable.GetPushed);

        EventManager.instance.GE_Break.UnsubscribeAfter(moveable.CheckFall);
        EventManager.instance.GE_Move.UnsubscribeAfter(moveable.CheckFall);

        EventManager.instance.UI_Targettable.UnsubscribeOn(moveable.OnTargettable);
        EventManager.instance.UI_Select.UnsubscribeOn(moveable.OnSelect);
        EventManager.instance.UI_Hover.UnsubscribeOn(moveable.OnHover);
        switch (moveable)
        {
            case Bomb bomb:
                UnsubscribeBombMethods(bomb);
                break;
            default:
                break;
        }
    }


    private void SpawnCharacter(CharacterType type, Vector3Int gridPosition, PlayerAccount controllingPlayer)
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
        spawned.SetControllingPlayer(controllingPlayer);
        spawned.gameObject.name = controllingPlayer.DisplayName + ": " + type.ToString();

        //Set abilities
        spawned.Abilities = AbilityManager.instance.CreateAbilities(charScriptable.Abilities);


        //Set items
        spawned.Items = ItemManager.instance.CreateItems(charScriptable.Items);

        //Subscribe to events
        SubscribeCharacterMethods(spawned);

        //Add to lists
        _allUnits.Add(spawned);
        _allCharacters.Add(spawned);
        _allMoveable.Add(spawned);
    }

    public List<Unit> GetAllUnits()
    {
        return _allUnits;
    }

    public List<Character> GetAllCharacters()
    {
        return _allCharacters;
    }

    public void RemoveCharacter(Character character)
    {
        UnsubscribeCharacterMethods(character);
    }

    public void RemoveMoveable(Moveable moveable)
    {
        UnsubscribeMoveableMethods(moveable);

    }

    public List<Vector3Int> GetOccupiedPositions()
    {
        List<Vector3Int> positions = new();
        foreach (Moveable moveable in _allMoveable) 
        { 
            if(moveable.IsOnGrid())
            {
                positions.Add(moveable.GetCurrentPosition());
            }
        }
        return positions;
    }

    public List<Moveable> GetMoveablesAtPositions(List<Vector3Int> positions)
    {
        List<Moveable> moveables = new();
        foreach (Moveable moveable in _allMoveable)
        {
            if (moveable.IsOnGrid() && positions.Contains(moveable.GetCurrentPosition()))
            {
                moveables.Add(moveable);
            }
        }
        return moveables;
    }

    public List<Unit> GetLivingUnits()
    {
        return _allUnits.Where(unit => unit != null && unit.IsAlive()).ToList();
    }

}
