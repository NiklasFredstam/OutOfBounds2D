using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButtonUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _name;
    [SerializeField]
    private TMP_Text _hotkeyNumberText;
    [SerializeField]
    private Image _abilityImage;
    private Ability _ability;
    [SerializeField]
    private Unit _unitOwningAbility;

    private int _hotkeyIndex;

    public void CreateAbilityButton(Ability ability, int hotkeyNumber, Unit unitOwningAbility)
    {
        _ability = ability;
        _abilityImage.sprite = ability.AbilityImage;
        _name.text = ability.AbilityName;
        _hotkeyNumberText.text = hotkeyNumber.ToString();
        _unitOwningAbility = unitOwningAbility;
        gameObject.name = name + ": " +  unitOwningAbility.gameObject.name;
        _hotkeyIndex = hotkeyNumber;
    }

    public void OnClick()
    {
        if(TurnManager.instance.IsAllowedToMoveUnit(_unitOwningAbility))
        {
            InputManager.instance.SetSelectedAbility(_ability);
        }
    }


    public int GetHotkey() => _hotkeyIndex;
    public void TriggerClick() => OnClick();

}