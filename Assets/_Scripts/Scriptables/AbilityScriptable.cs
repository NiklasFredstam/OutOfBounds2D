using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Scriptable Ability")]
public class AbilityScriptable : ScriptableObject
{
    public string AbilityName;
    public Sprite AbilityWindowSprite;
    public AbilityType AbilityType;
    public SelectState SelectState;
    public int TargetRange;

    public Ability CreateAbilityInstance()
    {
        Ability ability = AbilityType switch
        {
            AbilityType.Push => new Push(),
            AbilityType.Ditch => new Ditch(),
            AbilityType.DamageTile => new DamageTile(),
            AbilityType.Bomb => new BombAbility(),
            _ => null
        };

        if (ability == null)
        {
            Debug.LogWarning($"Ability type {AbilityType} is not supported.");
            return null;
        }

        ability.Initialize(this);
        return ability;
    }
}

public enum AbilityType
{
    Push,
    DamageTile,
    Ditch,
    Bomb
}

