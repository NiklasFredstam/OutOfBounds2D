using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : Singleton<AbilityManager>
{

    [SerializeField]
    private GameObject _abilityContainer;

    public List<Ability> CreateAbilities(List<AbilityType> abilitiyTypes)
    {
        List<Ability> abilities = new();

        int totalAbilities = abilities.Count;
        for (int i = 0; i < abilitiyTypes.Count; i++) 
        {
            AbilityType abT = abilitiyTypes[i];
            Ability instantiated = GetAbility(abT);
            instantiated.SetHotkeyNumber((i + 1).ToString());
            abilities.Add(instantiated);
        }
        return abilities;
    }

    private Ability GetAbility(AbilityType type)
    {
        AbilityScriptable abilityScriptable = ResourceSystem.instance.GetAbility(type);

        Ability spawned = Instantiate(abilityScriptable.Prefab, _abilityContainer.transform, false);

        spawned.SetAbilityName(abilityScriptable.AbilityName);
        spawned.SetSelectedState(abilityScriptable.SelectState);

        //Input/UI manager handles setting active
        spawned.gameObject.SetActive(false);

        return spawned;
    }

}
