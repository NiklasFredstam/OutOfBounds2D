using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : Singleton<InputManager>
{

    private readonly Dictionary<SelectState, List<string>> INPUT_STATE_COLLIDERS = new()
    {
        { SelectState.None, new () },
        { SelectState.Grid, new () { "Grid" } },
        { SelectState.Moveable, new () { "Moveable" } },
        { SelectState.Path, new () { "Grid" } }
    };

    [SerializeField]
    private SelectState _inputState;
    [SerializeField]
    private GameObject _lastHoveredObject = null;
    [SerializeField]
    private GameObject _selectedObject = null;

    [SerializeField]
    private Ability _selectedAbility = null;
    [SerializeField]
    private string _selectedAbilityname = null;
    [SerializeField] 
    private List<Vector3Int> _positionsWithinRange = new();
   
    private Camera _mainCamera;

    public List<Vector3Int> GetPositionsWithinRange()
    {
        return _positionsWithinRange;
    }

    public void SetSelectedAbility(Ability ability)
    {
        if(ability == null)
        {
            _selectedAbility = null;
            _selectedAbilityname = null;
            _positionsWithinRange.Clear();
            SetInputState(SelectState.None);
            EventManager.instance.UI_Targettable.Invoke(new TargettableArg(_inputState, _positionsWithinRange));
        }
        else
        {
            _selectedAbility = ability;
            _selectedAbilityname = ability.AbilityName;
            SetInputState(ability.SelectState);
            SetPositionsWithinRange(TurnManager.instance.GetCurrentTurnUnit().GetCurrentPosition(), _selectedAbility.TargetRange);
            EventManager.instance.UI_Targettable.Invoke(new TargettableArg(_inputState, _positionsWithinRange));
        }
    }

    public void SetPositionsWithinRange(Vector3Int currentPos, int range)
    {
        List<Vector3Int> validPositions = GridManager.instance.GetPositionsWithinRangeExcludingCenter(currentPos, range);
        List<Vector3Int> occupiedPositions = MoveableManager.instance.GetOccupiedPositions();

        switch (_inputState) 
        {
            case SelectState.Grid:
                break;
            case SelectState.Path:
                validPositions = validPositions
                    .Where(pos => {
                        if (occupiedPositions.Contains(pos)) return false;
                        var path = GridManager.instance.GetQuickestPath(currentPos, pos);
                        return path != null && path.Count <= range;
                    })
                    .ToList();
                break;
            case SelectState.Moveable:
                validPositions = validPositions
                    .Where(pos => occupiedPositions.Contains(pos))
                    .ToList();
                break;
            default:
                break;
        }

        _positionsWithinRange = validPositions;
    }


    public Ability GetSelectedAbility()
    {
        return _selectedAbility;
    }

    public GameObject GetSelectedObject()
    {
        return _selectedObject;
    }


    private void SetInputState(SelectState newState)
    {
        _inputState = newState;
    }

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        HandleHotkey();
        HandleHover();
        HandleClick();
    }

    void HandleHotkey()
    {
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                AbilityButtonUI button = UIManager.instance.GetButtonForKey(i);
                if (button != null)
                {
                    button.TriggerClick();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.instance.ExecuteSelectedAbility();
        }
    }

    public void HandleHover()
    {
        GameObject targetObject = GetTargetGameObject();
        EventManager.instance.UI_Hover.Invoke(new HoverArg(targetObject));
        _lastHoveredObject = targetObject;
    }

    public void HandleClick()
    {
        if (Input.GetMouseButtonDown(0) && _lastHoveredObject != null)
        {
            GameObject targetObject = GetTargetGameObject();
            EventManager.instance.UI_Select.Invoke(new SelectArg(targetObject));
            _selectedObject = targetObject;
        }
    }

    public GameObject GetTargetGameObject()
    {
        Vector3 mousePos = Helper.GetMousePosition(_mainCamera);

        Collider2D[] hits = Physics2D.OverlapPointAll(mousePos);
        if (hits.Length > 0)
        {
            Collider2D topCollider = hits
                .OrderByDescending(hit => GetSortingLayerPriority(hit.GetComponent<SpriteRenderer>()))
                .ThenByDescending(hit => hit.GetComponent<SpriteRenderer>()?.sortingOrder ?? 0)
                .FirstOrDefault();
            if (topCollider != null)
            {
                return topCollider.gameObject;
            }
        }
        
        return null;
    }

    private int GetSortingLayerPriority(SpriteRenderer sr)
    {
        if (sr == null) return -1;

        switch (sr.sortingLayerName)
        {
            case "Moveable":
                return 10;
            case "Grid":
                return 5;
            case "Background":
                return 0;
            default:
                return -1;
        }
    }

    public void ClearCurrentSelections()
    {
        _selectedAbility = null;
        _selectedObject = null;
    }
}
