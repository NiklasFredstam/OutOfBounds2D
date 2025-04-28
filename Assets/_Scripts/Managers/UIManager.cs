using System.Collections.Generic;
using System.Linq;
using UnityEditor.Playables;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{

    [SerializeField]
    private GameObject _abilityContainer;
    [SerializeField]
    private GameObject _abilityButtonPrefab;

    private readonly List<AbilityButtonUI> _abilityButtons = new();


    private IInspectable _previousHover;
    [SerializeField]
    private DetailsWindowUI _hoverDetailsWindow;
    [SerializeField]
    private GameObject _hoverStatListContainer;

    [SerializeField]
    private GameObject _statDetailPrefab;

    [Header("Stat Icons")]
    [SerializeField] private Sprite _tileHealthIcon;
    [SerializeField] private Sprite _unitPowerIcon;
    [SerializeField] private Sprite _unitDestructivePowerIcon;
    [SerializeField] private Sprite _moveableWeightIcon;
    [SerializeField] private Sprite _unitSpeedIcon;

    protected override void Awake()
    {
        base.Awake();
        EventManager.instance.UI_Hover.SubscribeOn(ShowDetailsWindow);
        _hoverDetailsWindow.gameObject.SetActive(false);
    }


    public void CreateAbilityButtons(List<Ability> abilities, Unit unitOwningAbility)
    {
        DestroyAbilityButtons();

        for (int i = 0; i < abilities.Count; i++)
        {
            Ability ability = abilities[i];
            GameObject buttonGO = Instantiate(_abilityButtonPrefab, _abilityContainer.transform);
            AbilityButtonUI buttonUI = buttonGO.GetComponent<AbilityButtonUI>();

            if (buttonUI == null)
            {
                Debug.LogError("AbilityButtonUI component missing from prefab.");
                continue;
            }
            buttonUI.CreateAbilityButton(
                ability,
                i + 1,
                unitOwningAbility
            );
            _abilityButtons.Add( buttonUI );
        }
    }

    public void ShowDetailsWindow(HoverArg hoverArg)
    {
        if (hoverArg.HOVERED_OBJECT == null) 
        { 
            _hoverDetailsWindow.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Hiding");
            _hoverDetailsWindow.gameObject.SetActive(true);
        }
    }


    public void SetDetailsWindowInfo(IInspectable inspectable)
    {
        if(_previousHover == inspectable)
        {
            return;
        }
        _previousHover = inspectable;
        DestroyStateInfoRows();
        List<StatDetail> statDetails = inspectable.GetDetails();
        Sprite sprite = inspectable.GetObjectSprite();
        string description = inspectable.GetDescription();

        
        foreach (StatDetail detail in statDetails) 
        {
            GameObject detailRow = Instantiate(_statDetailPrefab, _hoverStatListContainer.transform);
            DetailRowUI statDetailRowUI = detailRow.GetComponent<DetailRowUI>();

            if (statDetailRowUI == null)
            {
                Debug.LogError("AbilityButtonUI component missing from prefab.");
                continue;
            }
            statDetailRowUI.Setup(
                detail.StatName,
                detail.Value.ToString(),
                detail.IsValueHigherThanBase,
                detail.IsValueLowerThanBase,
                GetStatIcon(detail.DetailStatType)
            );
        }
        _hoverDetailsWindow.Setup(sprite, description);
    }

    public void DestroyStateInfoRows()
    {
        foreach (Transform child in _hoverStatListContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }


    public void DestroyAbilityButtons()
    {
        foreach (Transform child in _abilityContainer.transform)
        {
            Destroy(child.gameObject);
        }
        _abilityButtons.Clear();
    }

    public AbilityButtonUI GetButtonForKey(int key)
    {
        return _abilityButtons.FirstOrDefault(b => b.GetHotkey() == key);
    }

    public Sprite GetStatIcon(DetailStatType detailStatType)
    {

        // Set correct icon based on stat type
        return detailStatType switch
        {
            DetailStatType.TileHealth => _tileHealthIcon,
            DetailStatType.UnitPower => _unitPowerIcon,
            DetailStatType.UnitDestructivePower => _unitDestructivePowerIcon,
            DetailStatType.MoveableWeight => _moveableWeightIcon,
            DetailStatType.UnitSpeed => _unitSpeedIcon,
            _ => null,
        };
    }
}