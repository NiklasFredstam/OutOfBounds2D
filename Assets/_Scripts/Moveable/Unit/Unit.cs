using UnityEngine;

public class Unit : Moveable
{

    //TODO: take the stats through status list before getting
    [SerializeField]
    public UnitStats UnitStats;

    public Event<int> OnDealDamage = new Event<int>();

    public virtual void SetUnitStats(UnitStats unitStats)
    {
        UnitStats = unitStats;
    }

    public virtual void Walk(Vector3 newPosition)
    {
        Move(newPosition);
    }

    public virtual void DealDamage()
    {
        //magic test number
        OnDealDamage.Invoke(UnitStats.DestructivePower);
    }
}
