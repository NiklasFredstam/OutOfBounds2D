using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "Object", menuName = "Scriptable Objects")]
public abstract class ObjectScriptable : ScriptableObject
{
    public string Description;
    public Sprite MenuSprite;
    public GameObject Prefab;
}