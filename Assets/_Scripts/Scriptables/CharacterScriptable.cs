using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Character")]
public class CharacterScriptable : ScriptableObject
{

    [SerializeField] private CharacterStats _characterStats;
    public CharacterStats CharacterStats => _characterStats;


    [SerializeField] private UnitStats _unitStats;
    public UnitStats UnitStats => _unitStats;


    [SerializeField] private MoveableStats _moveableStats;
    public MoveableStats MoveableStats => _moveableStats;



    public Character Prefab;
    public string Description;
    public Sprite MenuSprite;
    public CharacterType CharacterType;

    [SerializeField] private List<AbilityType> _abilities;
    public List<AbilityType> Abilities => _abilities;

    [SerializeField] private List<ItemType> _items;
    public List<ItemType> Items => _items;
}

public enum CharacterType
{
    Michi,
    Jibblon,
    Phillzor
}
