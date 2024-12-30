using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "Minion", menuName = "Scriptable Minion")]
public class MinionScriptable : ScriptableObject
{
    [SerializeField] private CharacterStats _stats;
    public CharacterStats MinionBaseStats => _stats;

    public GameObject Prefab;
    public string Description;
    public Sprite MenuSprite;
}