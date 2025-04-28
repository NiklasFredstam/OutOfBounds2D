using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ditch : Ability
{

    public override void UseAbility(Unit sourceUnit, GameObject target)
    {
        if (target.TryGetComponent<HexTile>(out var targetTile))
        {
            List<Vector3Int> path = GridManager.instance.GetQuickestPath(sourceUnit.GetCurrentPosition(), targetTile.GetCurrentGridPosition());
            foreach (Vector3Int pos in path)
            {
                EventManager.instance.GE_Move.Invoke(new MoveArg(sourceUnit, sourceUnit, pos, false));

            }
        }
    }
}
