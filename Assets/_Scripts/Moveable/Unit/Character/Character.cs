using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit
{
    [SerializeField]
    protected CharacterStats CharacterStats;

    public virtual void SetCharacterStats(CharacterStats characterStats)
    {
        CharacterStats = characterStats;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Fall(FallArg fallArg)
    {
        if (fallArg.FALLING == this && _unitStats.Alive && _moveableStats.IsOnGrid)
        {
            _unitStats.Alive = false;
            _moveableStats.IsOnGrid = false;
            Debug.Log(gameObject.name +  " fell out of bounds.");
            MoveableManager.instance.RemoveCharacter(this);
            AnimationQueue.instance.EnqueueAnimation(AnimateFall(transform, new Vector3(transform.position.x, transform.position.y - 10, 10), 2f));
        }
    }

    public override List<StatDetail> GetDetails()
    {
        return base.GetDetails();
    }

    public override string GetDescription()
    {
        return "Character Description";
    }

}
