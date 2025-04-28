using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BlinkDarker : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Coroutine fadeCoroutine;

    [SerializeField]
    private float _fadeTime = 0.5f;

    private Color _originalColor;
    private Color _darkerColor;

    private bool _isFading = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
        _darkerColor = _originalColor * 0.5f;
        _darkerColor.a = _originalColor.a;
    }

    public void StartFadeLoop()
    {
        if (!_isFading)
        {
            _isFading = true;
            fadeCoroutine = StartCoroutine(FadeLoop());
        }
    }

    public void StopFadeLoop()
    {
        if (_isFading)
        {
            _isFading = false;

            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(FadeToColorAndStop(_originalColor));
            }
            else
            {
                _spriteRenderer.color = _originalColor;
            }
        }

    }

    private IEnumerator FadeLoop()
    {
        while (_isFading)
        {

            yield return StartCoroutine(FadeToColor(_darkerColor));
            yield return StartCoroutine(FadeToColor(_originalColor));
            
        }
    }

    private IEnumerator FadeToColor(Color targetColor)
    {
        float time = 0f;
        Color startColor = _spriteRenderer.color;

        while (time < _fadeTime)
        {
            _spriteRenderer.color = Color.Lerp(startColor, targetColor, time / _fadeTime);
            time += Time.deltaTime;
            yield return null;
        }

        _spriteRenderer.color = targetColor;
    }

    public void SetFadeTime(float fadeTime)
    {
        _fadeTime = fadeTime;
    }

    public void SetDarkerMultiplier(float multiplier)
    {
        _darkerColor = _originalColor * multiplier;
        _darkerColor.a = _originalColor.a;
    }

    private IEnumerator FadeToColorAndStop(Color targetColor)
    {
        float time = 0f;
        Color startColor = _spriteRenderer.color;

        while (time < _fadeTime)
        {
            _spriteRenderer.color = Color.Lerp(startColor, targetColor, time / _fadeTime);
            time += Time.deltaTime;
            yield return null;
        }

        _spriteRenderer.color = targetColor;
        fadeCoroutine = null; // fully done
    }
}
