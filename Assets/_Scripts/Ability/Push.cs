using UnityEngine;

public class Push : Ability
{

    public override void UseAbility(Unit sourceUnit, GameObject target)
    {
        if (target.TryGetComponent<Moveable>(out var targetMoveable))
        {
            HexDirection direction = GridManager.instance.GetDirectionTo(sourceUnit.GetCurrentPosition(), targetMoveable.GetCurrentPosition());
            EventManager.instance.GE_Push.Invoke(new PushArg(sourceUnit, targetMoveable, 0, direction));
        }
    }
}
