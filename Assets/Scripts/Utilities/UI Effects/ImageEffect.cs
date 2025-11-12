using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageEffect : MonoBehaviour
{
    private Image _image;
    private Color _color;
    private void Awake()
    {
        _image = GetComponent<Image>();
        _color = _image.color;
    }
    public void FadeOut(float duration, Action onComplete = null)
    {
        StartCoroutine(FadeRoutine(0, duration, onComplete));
    }
    public void FadeIn(float duration, Action onComplete = null)
    {
        StartCoroutine(FadeRoutine(1, duration, onComplete));
    }
    public void ZoomIn(float duration, Action onComplete = null)
    {
        StartCoroutine(ScaleRoutine(1, duration, onComplete));
    }
    public void ZoomOut(float duration, Action onComplete = null)
    {
        StartCoroutine(ScaleRoutine(0, duration, onComplete));
    }
    
    private IEnumerator FadeRoutine(float targetAlpha, float duration, Action onComplete)
    {
        float startAlpha = _color.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            _color.a = newAlpha;
            _image.color = _color;
            yield return null;
        }
        
        _color.a = targetAlpha;
        _image.color = _color;
        
        onComplete?.Invoke();
    }
    private IEnumerator ScaleRoutine(float targetScale, float duration, Action onComplete)
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.one * targetScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        transform.localScale = endScale;
        onComplete?.Invoke();
    }
    
}
