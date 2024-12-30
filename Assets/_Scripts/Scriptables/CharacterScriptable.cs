using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
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
}

public enum CharacterType
{
    Michi
}
