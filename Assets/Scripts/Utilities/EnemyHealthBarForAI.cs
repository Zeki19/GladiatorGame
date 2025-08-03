using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Phase Markers (Editable)")]
    [SerializeField] private GameObject phaseMarker1;
    [SerializeField] private GameObject phaseMarker2;
    [SerializeField] private GameObject phaseMarker3;

    [SerializeField] private bool Mark1 = true;
    [SerializeField] private bool Mark2 = true;
    [SerializeField] private bool Mark3 = true;

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

        if (phaseMarker1 != null) phaseMarker1.SetActive(Mark1);
        if (phaseMarker2 != null) phaseMarker2.SetActive(Mark2);
        if (phaseMarker3 != null) phaseMarker3.SetActive(Mark3);
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