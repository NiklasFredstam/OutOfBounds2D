using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Character : Unit
{
    [SerializeField]
    private CharacterStats CharacterStats;
    public virtual void SetCharacterStats(CharacterStats characterStats)
    {
        CharacterStats = characterStats;
    }
    public override void Fall(HexTile tile)
    {
        BeforeFall.Invoke(this);

        StartCoroutine(MoveToPosition(new Vector3(transform.position.x, -7, 8), 1f));
        UnitStats.Alive = false;
        Debug.Log("Character is out of bounds.");
        gameObject.SetActive(false);

        AfterFall.Invoke(this);
    }
}
