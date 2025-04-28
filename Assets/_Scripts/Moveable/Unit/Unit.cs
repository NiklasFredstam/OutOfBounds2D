
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class Unit : Moveable
{
    [SerializeField]
    protected UnitStats _unitStats;
    //list of status
    public List<Ability> Abilities {private get; set; }
    public List<Item> Items;
    protected PlayerAccount _controlledByPlayer;

    protected override void Awake()
    {
        base.Awake();
    }

    public virtual List<Ability> GetAbilities()
    {
        return new (Abilities.Concat(GetItemAbilities()));
    }

    private List<Ability> GetItemAbilities()
    {
        return Items.SelectMany(item => item.Abilities).ToList();
    }
    public PlayerAccount GetControllingPlayer()
    {
        return _controlledByPlayer;
    }

    public void SetControllingPlayer(PlayerAccount playerAccount)
    {
        _controlledByPlayer = playerAccount;
    }

    public int GetSpeed()
    {   
        //invoke statuses -> unitstats
        return _unitStats.Speed;
    }

    public int GetPower()
    {
        //invoke statuses -> unitstats
        return _unitStats.Power;
    }
    public int GetDestructivePower()
    {
        //invoke statuses -> unitstats
        return _unitStats.DestructivePower;
    }


    public virtual void SetUnitStats(UnitStats unitStats)
    {
        _unitStats = unitStats;
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

    public void Push(PushArg pushArg)
    {
        if (pushArg.SOURCE == this)
        {
            Debug.Log("Unit -  Push");
            EventManager.instance.GE_GetPushed.Invoke(new GetPushedArg(this, pushArg.TARGET, _unitStats.Power, pushArg.DIRECTION));
        }
    }

    public void UseAbility(UseAbilityArg useAbilityArg)
    {
        if (useAbilityArg.UNIT_SOURCE == this)
        {
            Debug.Log("Unit - Use Ability");
        }
    }

    public bool IsAlive()
    {
        return  _unitStats.Alive;
    }

    public override List<StatDetail> GetDetails() {
    
        List<StatDetail> statDetails = base.GetDetails();
        statDetails.Add(new StatDetail("Speed", GetSpeed(), GetSpeed() > _unitStats.Speed, GetSpeed() < _unitStats.Speed, DetailStatType.UnitSpeed));
        statDetails.Add(new StatDetail("Power", GetPower(), GetPower() > _unitStats.Power, GetPower() < _unitStats.Power, DetailStatType.UnitPower));
        statDetails.Add(new StatDetail("DestructivePower", GetDestructivePower(), GetDestructivePower() > _unitStats.DestructivePower, GetDestructivePower() < _unitStats.DestructivePower, DetailStatType.UnitDestructivePower));
        return statDetails;
    }

    public override string GetDescription()
    {
        return "";
    }
}