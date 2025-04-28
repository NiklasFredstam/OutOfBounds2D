using UnityEditor.Playables;
using UnityEngine;

public class DamageTile : Ability
{

    public override void UseAbility(Unit sourceUnit, GameObject target)
    {
        if (target.TryGetComponent<HexTile>(out var targetTile))
        {
            EventManager.instance.GE_DoDamage.Invoke(new DoDamageArg(sourceUnit, 0, targetTile));
        }
    }

}
