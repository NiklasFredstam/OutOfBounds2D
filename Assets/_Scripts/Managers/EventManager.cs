using System;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{

    public GameEvent<DoDamageArg> GE_DoDamage = new ("Do Damage");

    public GameEvent<TakeDamageArg> GE_TakeDamage = new ("Take Damage");

    public GameEvent<BreakArg> GE_Break = new ("Break");

    public GameEvent<UseAbilityArg> GE_UseAbility = new ("Use Ability");

    public GameEvent<FallArg> GE_Fall = new("Fall");

    public GameEvent<MoveArg> GE_Move = new("Move");

     
    public void ClearGameEvents()
    {
        GE_DoDamage.Clear();
        GE_TakeDamage.Clear();
        GE_Break.Clear();
        GE_UseAbility.Clear();
        GE_Fall.Clear();
        GE_Move.Clear();
    }
}
