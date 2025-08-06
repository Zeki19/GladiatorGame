using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBarUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image bossIcon;
    [SerializeField] private TextMeshProUGUI bossNameText;
    [SerializeField] private Image healthFill;
    [SerializeField] private Image trailFill;

    [Header("Trail Settings")]
    [SerializeField] private float trailSpeed = 2f;

    [Header("Boss Info (Editable)")]
    [SerializeField] private string bossName;

    [Header("Boss Phase Sprites")]
    [SerializeField] private List<Sprite> bossPhaseSprites = new List<Sprite>();

    [Header("Phase Markers")]
    [SerializeField] private RectTransform healthBarFillArea; // El área visual de la barra de vida
    [SerializeField] private List<RectTransform> phaseMarkers = new List<RectTransform>();
    [Range(0f, 1f)]
    [SerializeField] private List<float> markerPositions = new List<float>(); // Porcentajes de posición

    [Header("Boss Reference")]
    [SerializeField] private EnemyManager bossManager;
    private void Start()
    {
        if (bossManager == null || bossManager.HealthComponent == null)
        {
            Debug.LogWarning("BossManager or HealthComponent not assigned");
            enabled = false;
            return;
        }

        bossNameText.text = bossName;

        if (bossPhaseSprites.Count > 0)
            bossIcon.sprite = bossPhaseSprites[0];

        healthFill.fillAmount = 1f;
        trailFill.fillAmount = 1f;

        bossManager.HealthComponent.OnDamage += OnHealthChanged;
        bossManager.HealthComponent.OnHeal += OnHealthChanged;

        UpdatePhaseMarkerPositions();
    }
    private void UpdatePhaseMarkerPositions()
    {
        if (healthBarFillArea == null) return;

        float barWidth = healthBarFillArea.rect.width;

        for (int i = 0; i < phaseMarkers.Count && i < markerPositions.Count; i++)
        {
            float clamped = Mathf.Clamp01(markerPositions[i]);

            if (phaseMarkers[i] == null) continue;

            Vector2 anchoredPos = phaseMarkers[i].anchoredPosition;

            // Corrige el punto de origen al inicio de la barra, incluso si el pivot es 0.5
            anchoredPos.x = (clamped * barWidth) - (barWidth * 0.5f);
            phaseMarkers[i].anchoredPosition = anchoredPos;
        }
    }
    private void OnHealthChanged(float _)
    {
        float percent = bossManager.HealthComponent.GetCurrentHealthPercentage() / 100f;
        healthFill.fillAmount = percent;

        StopAllCoroutines();
        StartCoroutine(UpdateTrail(percent));
    }
    private IEnumerator UpdateTrail(float target)
    {
        while (trailFill.fillAmount > target)
        {
            trailFill.fillAmount = Mathf.MoveTowards(trailFill.fillAmount, target, trailSpeed * Time.deltaTime);
            yield return null;
        }
        trailFill.fillAmount = target;
    }
    private void OnDestroy()
    {
        if (bossManager != null && bossManager.HealthComponent != null)
        {
            bossManager.HealthComponent.OnDamage -= OnHealthChanged;
            bossManager.HealthComponent.OnHeal -= OnHealthChanged;
        }
    }
    public void SetBossPhase(int phase)
    {
        if (phase >= 0 && phase < bossPhaseSprites.Count)
        {
            bossIcon.sprite = bossPhaseSprites[phase];
        }
    }
    public string BossName
    {
        get => bossName;
        set
        {
            bossName = value;
            if (bossNameText != null) bossNameText.text = value;
        }
    }
    public void SetBarColor(Color color)
    {
        healthFill.color = color;
        trailFill.color = new Color(color.r, color.g, color.b, 0.6f);
    }
}
