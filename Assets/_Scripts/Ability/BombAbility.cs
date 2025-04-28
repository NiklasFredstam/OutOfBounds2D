using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BombAbility : Ability
{
    public override void UseAbility(Unit sourceUnit, GameObject target)
    {
        if (target.TryGetComponent<HexTile>(out var targetTile))
        {
            MoveableManager.instance.SpawnMoveableObject(MoveableObjectType.Bomb, targetTile.GetCurrentGridPosition(), sourceUnit);
        }
    }
}