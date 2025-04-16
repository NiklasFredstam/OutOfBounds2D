using UnityEngine;

public class Ditch : Ability
{
    EventManager _eventManager;
    void Awake()
    {
        _eventManager = EventManager.instance;
    }

    public override void QueueAbility(Unit sourceUnit, GameObject target)
    {
        _eventManager.GE_UseAbility.Invoke(new UseAbilityArg(sourceUnit, null));


        if (target.TryGetComponent<HexTile>(out var targetTile))
        {
            _eventManager.GE_Move.Invoke(new MoveArg(sourceUnit, 0, targetTile));
        }
    }
}
