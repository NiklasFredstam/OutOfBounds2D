using UnityEditor.Playables;
using UnityEngine;

public class DamageTile : Ability
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
            Debug.Log("Reached");
            _eventManager.GE_DoDamage.Invoke(new DoDamageArg(sourceUnit, 0, targetTile));
        }
    }

}
