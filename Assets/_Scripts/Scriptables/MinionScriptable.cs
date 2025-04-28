using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "Minion", menuName = "Scriptable Minion")]
public class MinionScriptable : ScriptableObject
{
    [SerializeField] private UnitStats _stats;
    public UnitStats MinionBaseStats => _stats;

    public GameObject Prefab;
    public string Description;
    public Sprite MenuSprite;
}