using UnityEngine;
using TMPro;
using System;

public class TutorialQuestEntryUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI labelText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("Animation Settings")]
    [SerializeField] private float fadeInDuration = 0.3f;
    [SerializeField] private float fadeOutDuration = 0.2f;

    private CanvasGroup _canvasGroup;
    private bool _isDestroying = false;


    public event Action OnFadeOutComplete;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        _canvasGroup.alpha = 0f;
    }
    public void SetQuestData(string missionName, string missionDescription)
    {
        if (labelText != null)
        {
            labelText.text = missionName;
        }
        else
        {
            Debug.LogWarning("TutorialQuestEntryUI: labelText is not assigned!");
        }

        if (descriptionText != null)
        {
            descriptionText.text = missionDescription;
        }
        else
        {
            Debug.LogWarning("TutorialQuestEntryUI: descriptionText is not assigned!");
        }

        FadeIn();
    }


    private void FadeIn()
    {
        if (_isDestroying) return;

        StopAllCoroutines();
        StartCoroutine(FadeCoroutine(0f, 1f, fadeInDuration));
    }


    public void FadeOutAndDestroy()
    {
        if (_isDestroying) return;

        _isDestroying = true;
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine());
    }

    private System.Collections.IEnumerator FadeCoroutine(float from, float to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        _canvasGroup.alpha = to;
    }

    private System.Collections.IEnumerator FadeOutCoroutine()
    {
        yield return FadeCoroutine(_canvasGroup.alpha, 0f, fadeOutDuration);

        OnFadeOutComplete?.Invoke();

        yield return null;

        Destroy(gameObject);
    }
}