using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SelectDarker : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private Color _darkerColor;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
        _darkerColor = _originalColor * 0.5f;
        _darkerColor.a = _originalColor.a;
    }

    public void SetDarker()
    {
        _spriteRenderer.color = _darkerColor;
    }

    public void ResetColor()
    {
        _spriteRenderer.color = _originalColor;
    }

}
