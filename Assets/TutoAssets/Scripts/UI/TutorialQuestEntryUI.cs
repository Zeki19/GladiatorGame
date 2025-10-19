using UnityEngine;
using TMPro;

/// <summary>
/// Represents a single quest entry in the tutorial UI panel.
/// Displays the quest label (title) and description (goal).
/// </summary>
public class TutorialQuestEntryUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI labelText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("Animation Settings")]
    [SerializeField] private float fadeInDuration = 0.3f;
    [SerializeField] private float fadeOutDuration = 0.2f;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        // Start invisible for fade-in effect
        _canvasGroup.alpha = 0f;
    }

    /// <summary>
    /// Sets the quest data and displays it.
    /// </summary>
    /// <param name="missionName">The quest label/title</param>
    /// <param name="missionDescription">The quest goal/description</param>
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

    /// <summary>
    /// Fades in the quest entry.
    /// </summary>
    private void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeCoroutine(0f, 1f, fadeInDuration));
    }

    /// <summary>
    /// Fades out the quest entry and destroys it.
    /// </summary>
    public void FadeOutAndDestroy()
    {
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
        Destroy(gameObject);
    }
}
