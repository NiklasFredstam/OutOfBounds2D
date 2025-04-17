using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
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
            List<Vector3Int> path = GridManager.instance.GetQuickestPath(sourceUnit.GetCurrentPosition(), targetTile.GetCurrentGridPosition());

            List<string> pathAsStrings = path.Select(p => p.ToString()).ToList();
            Debug.Log("Full Path as Strings:\n" + string.Join(" -> ", pathAsStrings));

            _eventManager.GE_Move.Invoke(new MoveArg(sourceUnit, 0, path));
        }
    }
}
