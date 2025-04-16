
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class Unit : Moveable
{
    [SerializeField]
    protected UnitStats _unitStats;
    //list of status
    protected List<Ability> _abilities;

    public int GetSpeed()
    {   
        //invoke statuses -> unitstats
        return _unitStats.Speed;
    }
    public virtual void SetUnitStats(UnitStats unitStats)
    {
        _unitStats = unitStats;
    }

    public virtual void SetAbilities(List<Ability> abilities)
    {
        this._abilities = abilities;
    }

    public virtual List<Ability> GetAbilities()
    {
        return _abilities;
    }


    public void DoDamage(DoDamageArg damageArg)
    {
        if (damageArg.SOURCE == this)
        {
            Debug.Log("Unit -  Do Damage");
            _unitStats.DamageDealt += _unitStats.DestructivePower;
            EventManager.instance.GE_TakeDamage.Invoke(new TakeDamageArg(this, _unitStats.DestructivePower, damageArg.TARGET));
        }
    }

    public void Push(Moveable target)
    {
    }

    public void UseAbility(UseAbilityArg useAbilityArg)
    {
        if (useAbilityArg.UNIT_SOURCE == this)
        {
            Debug.Log("Unit - Use Ability");
        }
    }

    public bool IsAlive(bool isCommitedAction)
    {
        return  _unitStats.Alive;
    }


}