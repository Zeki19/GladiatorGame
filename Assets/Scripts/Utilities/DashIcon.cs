using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DashIcon : MonoBehaviour
{
    [SerializeField] private Image targetImage;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.2f;

    private CanvasGroup canvasGroup;
    private Coroutine fadeRoutine;
    private Coroutine flashRoutine;
    private Color originalColor;
    private bool isVisible = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (targetImage == null)
        {
            Debug.LogError("No Image");
            enabled = false;
            return;
        }
        originalColor = targetImage.color;
        canvasGroup.alpha = 1f;
    }
    public void ShowIcon()
    {
        if (isVisible) return;
        isVisible = true;
        StartFade(1f);
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashOnce());
    }
    public void HideIcon()
    {
        if (!isVisible) return;
        isVisible = false;
        StartFade(0f);
    }
    private void StartFade(float targetAlpha)
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeTo(targetAlpha));
    }
    private IEnumerator FadeTo(float targetAlpha)
    {
        float start = canvasGroup.alpha;
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, targetAlpha, time / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
    }
    private IEnumerator FlashOnce()
    {
        targetImage.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        targetImage.color = originalColor;
    }
}
