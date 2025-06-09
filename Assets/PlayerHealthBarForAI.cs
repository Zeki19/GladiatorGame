using Player;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarForAI : MonoBehaviour
{
    private PlayerManager _manager;
    [SerializeField]private Slider _slider;

    private void Start()
    {
        _manager=ServiceLocator.Instance.GetService<PlayerManager>();
        _manager.HealthComponent.OnDamage += UpdateLife;
        _manager.HealthComponent.OnHeal += UpdateLife;

        _slider = GetComponent<Slider>();
        _slider.value = 0.5f;
    }

    void UpdateLife(float amount)
    {
        _slider.value = _manager.HealthComponent.GetCurrentHealthPercentage()/100;
    }
}
