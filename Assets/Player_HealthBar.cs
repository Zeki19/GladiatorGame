using Player;
using UnityEngine;
using UnityEngine.UI;

public class Player_HealthBar : MonoBehaviour
{
    private PlayerManager _manager;
    [SerializeField] private Image healthFillImage;

    private void Start()
    {
        _manager = ServiceLocator.Instance.GetService<PlayerManager>();
        _manager.HealthComponent.OnDamage += UpdateLife;
        _manager.HealthComponent.OnHeal += UpdateLife;

        UpdateLife(100); 
    }
    private void OnDestroy()
    {
        if (_manager != null && _manager.HealthComponent != null)
        {
            _manager.HealthComponent.OnDamage -= UpdateLife;
            _manager.HealthComponent.OnHeal -= UpdateLife;
        }
    }

    void UpdateLife(float amount)
    {
        float fill = _manager.HealthComponent.GetCurrentHealthPercentage() / 100f;
        healthFillImage.fillAmount = fill;
    }
}