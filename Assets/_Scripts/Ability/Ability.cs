using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Ability : MonoBehaviour
{

    [SerializeField]
    private TMP_Text _name;

    [SerializeField]
    private TMP_Text _hotkeyNumber;

    private SelectState _selectedState;

    public void SetAbilityName(string text)
    {
        _name.text = text;
    }

    public void SetHotkeyNumber(string hotkeyNumber)
    {
        _hotkeyNumber.text = hotkeyNumber;
    }

    public void AbilityClick()
    {
        InputManager.instance.SetSelectedAbility(this);
    }
    public abstract void QueueAbility(Unit sourceUnit, GameObject target);
    public SelectState GetSelectState() 
    {
        return _selectedState;
    }

    public void SetSelectedState(SelectState selectedState)
    {
        _selectedState = selectedState;
    }

    public string GetName() { return _name.text; }   

}
