using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Ability
{

    public string AbilityName;

    public Sprite AbilityImage;

    public SelectState SelectState;

    public int TargetRange;

    public virtual void CommitAbility(Unit sourceUnit, GameObject target)
    {
        UseAbility(sourceUnit, target);
    }
    public abstract void UseAbility(Unit sourceUnit, GameObject target);


    public virtual void Initialize(AbilityScriptable data)
    {
        AbilityName = data.AbilityName;
        AbilityImage = data.AbilityWindowSprite;
        SelectState = data.SelectState;
        TargetRange = data.TargetRange;
    }


}
