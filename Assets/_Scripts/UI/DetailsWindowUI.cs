using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailsWindowUI : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _objectSprite;
    [SerializeField]
    private TMP_Text _description;

    public void Setup(Sprite objectSprite, string description)
    {
        _objectSprite.sprite = objectSprite;
        _description.text = description;
    }
}