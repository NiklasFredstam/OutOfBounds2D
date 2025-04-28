using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityManager : Singleton<AbilityManager>
{

    public List<Ability> CreateAbilities(List<AbilityType> abilitiyTypes)
    {
        List<Ability> abilities = new();
        for (int i = 0; i < abilitiyTypes.Count; i++) 
        {
            abilities.Add(CreateAbility(abilitiyTypes[i]));
        }
        return abilities;
    }

    public Ability CreateAbility(AbilityType type) 
    {
        AbilityScriptable abilityScriptable = ResourceSystem.instance.GetAbility(type);
        Ability newAbility = abilityScriptable.CreateAbilityInstance();
        return newAbility;
    }



}
