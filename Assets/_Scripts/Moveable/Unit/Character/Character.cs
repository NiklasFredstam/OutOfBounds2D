using System.Collections;
using UnityEngine;

public class Character : Unit
{
    [SerializeField]
    protected CharacterStats CharacterStats;
    public virtual void SetCharacterStats(CharacterStats characterStats)
    {
        CharacterStats = characterStats;
    }

    public override void Fall(FallArg fallArg)
    {
        if (fallArg.FALLING == this && _unitStats.Alive)
        {
            StartCoroutine(FallAndDisable());
        }
    }

    public override IEnumerator FallAndDisable()
    {
        _unitStats.Alive = false;
        yield return Helper.MoveToPosition(transform, new Vector3(transform.position.x, -20, -10), 1f);
        Debug.Log("Michi fell out of bounds.");
        gameObject.SetActive(false);
        UnitManager.instance.RemoveCharacter(this);
    }
}
