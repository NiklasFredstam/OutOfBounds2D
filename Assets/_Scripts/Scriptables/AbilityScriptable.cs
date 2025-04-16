using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Scriptable Ability")]
public class AbilityScriptable : ScriptableObject
{
    public string AbilityName;
    public Ability Prefab;
    public Sprite AbilityWindowSprite;
    public AbilityType AbilityType;
    public SelectState SelectState;
}

public enum AbilityType
{
    Push,
    DamageTile,
    Ditch
}

