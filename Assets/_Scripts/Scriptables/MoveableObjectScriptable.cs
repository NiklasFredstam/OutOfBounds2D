using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Moveable Object", menuName = "Scriptable Moveable Object")]
public class MoveableScriptable : ScriptableObject
{
    [SerializeField] private MoveableStats _moveableStats;
    public MoveableStats MoveableStats => _moveableStats;

    public Moveable Prefab;
    public string Description;
    public Sprite MenuSprite;
    public MoveableObjectType MoveableType;

    public string ObjectName;
}



public enum MoveableObjectType
{
    Bomb,
}