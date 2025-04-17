using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : Singleton<InputManager>
{

    private readonly Dictionary<SelectState, List<string>> INPUT_STATE_COLLIDERS = new()
    {
        { SelectState.None, new () },
        { SelectState.Grid, new () { "Grid" } },
        { SelectState.Moveable, new () { "Characters" } }
    };

    [SerializeField]
    private SelectState _InputState;
    [SerializeField]
    private GameObject _lastHoveredObject = null;
    [SerializeField]
    private GameObject _selectedObject = null;


    [SerializeField]
    private Ability _selectedAbility = null;

    [SerializeField]
    private string _selectedAbilityname = null;
    
    private Camera _mainCamera;
    [SerializeField]
    private GameObject _abilityWindow;


    public void DeactivateOtherAbilities()
    {
        foreach (Ability ability in _abilityWindow.GetComponentsInChildren<Ability>())
        {
            ability.gameObject.SetActive(false);
        }
    }
    public void SetUIAbilities(List<Ability> abilities)
    {
        DeactivateOtherAbilities();
        foreach (Ability ability in abilities)
        {
            ability.gameObject.SetActive(true);
        }
    }

    public void SetSelectedAbility(Ability ability)
    {
        if(ability == null)
        {
            _selectedAbility = null;
            _selectedAbilityname = null;
            SetInputState(SelectState.None);
        } else
        {
            _selectedAbility = ability;
            _selectedAbilityname = ability.GetName();
            SetInputState(ability.GetSelectState());
        }
    }
    public Ability GetSelectedAbility()
    {
        return _selectedAbility;
    }

    public GameObject GetSelectedObject()
    {
        return _selectedObject;
    }


    public void SetInputState(SelectState newState)
    {
        _InputState = newState;
    }
    private void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        HandleHover();
        HandleClick();
    }


    public void HandleHover()
    {
        GameObject targetObject = GetTargetGameObject();
        if (targetObject != null)
        {
            if (targetObject != _lastHoveredObject)
            {
                if (_lastHoveredObject != null)
                {
                    _lastHoveredObject.SendMessage("OnHoverLeave", SendMessageOptions.DontRequireReceiver);
                }
                targetObject.SendMessage("OnHoverEnter", SendMessageOptions.DontRequireReceiver);
                _lastHoveredObject = targetObject;
            }
        } else
        {
            if(_lastHoveredObject != null)
            {
                _lastHoveredObject.SendMessage("OnHoverLeave", SendMessageOptions.DontRequireReceiver);
                _lastHoveredObject = null;
            }
        }
    }

    public void HandleClick()
    {
        if (Input.GetMouseButtonDown(0) && _lastHoveredObject != null)
        {
            GameObject targetObject = GetTargetGameObject();
            if (targetObject != null) {
                if (_selectedObject != null)
                {
                    _selectedObject.SendMessage("OnDeselect", SendMessageOptions.DontRequireReceiver);
                }
                targetObject.SendMessage("OnSelect", SendMessageOptions.DontRequireReceiver);
                _selectedObject = targetObject;
            } else
            {
                if (_selectedObject != null)
                {
                    _selectedObject.SendMessage("OnDeselect", SendMessageOptions.DontRequireReceiver);
                    _selectedObject = null;
                }
            }
        }
    }

    public GameObject GetTargetGameObject()
    {
        Vector3 mousePos = Helper.GetMousePosition(_mainCamera);
        foreach (var layerName in INPUT_STATE_COLLIDERS[_InputState])
        {
            LayerMask currentLayerMask = LayerMask.GetMask(layerName);
            Collider2D[] hits = Physics2D.OverlapPointAll(mousePos, currentLayerMask);

            if (hits.Length > 0)
            {
                Collider2D topCollider = hits
                    .OrderByDescending(hit => hit.GetComponent<SpriteRenderer>()?.sortingOrder ?? 0)
                    .FirstOrDefault();

                if (topCollider != null)
                {
                    return topCollider.gameObject;
                }
            }
        }
        return null;
    }

    public void ClearCurrentSelections()
    {
        _selectedAbility = null;
        _selectedObject = null;
        //clear UI from here as well
    }
}
