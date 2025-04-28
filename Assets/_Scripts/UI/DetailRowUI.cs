using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailRowUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _value;
    [SerializeField]
    private Image _statIcon;

    public void Setup(string statName, string value, bool isStatHigherThanBase, bool isStatLowerThanBase, Sprite sprite)
    {
        _value.text = value;
        if(isStatHigherThanBase)
        {
            _value.color = Color.green;
        }
        if(isStatLowerThanBase)
        {
            _value.color = Color.red;
        }
        _statIcon.sprite = sprite;
    }
}