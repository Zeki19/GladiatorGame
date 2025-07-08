using Player;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarForAI : MonoBehaviour
{
    [Header("UI Images")]
    [SerializeField] private Image healthImage; 
    [SerializeField] private Image trailImage;   

    [Header("Trail Settings")]
    [SerializeField] private float trailSpeed = 2f;

    private PlayerManager _manager;

    private void Start()
    {
        _manager = ServiceLocator.Instance.GetService<PlayerManager>();

        _manager.HealthComponent.OnDamage += OnHealthChanged;
        _manager.HealthComponent.OnHeal += OnHealthChanged;

        healthImage.fillAmount = 1f;
        trailImage.fillAmount = 1f;
    }

    private void OnHealthChanged(float _)
    {
        float target = _manager.HealthComponent.GetCurrentHealthPercentage() / 100f;

        healthImage.fillAmount = target;

        StopAllCoroutines();
        StartCoroutine(UpdateTrail(target));
    }

    private System.Collections.IEnumerator UpdateTrail(float target)
    {
        while (trailImage.fillAmount > target)
        {
            trailImage.fillAmount = Mathf.MoveTowards(
                trailImage.fillAmount,
                target,
                trailSpeed * Time.deltaTime
            );
            yield return null;
        }
        trailImage.fillAmount = target;
    }

    private void OnDestroy()
    {
        if (_manager != null && _manager.HealthComponent != null)
        {
            _manager.HealthComponent.OnDamage -= OnHealthChanged;
            _manager.HealthComponent.OnHeal -= OnHealthChanged;
        }
    }
}