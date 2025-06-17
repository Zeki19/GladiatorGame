using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarForAI : MonoBehaviour
{
    [SerializeField]private Slider _slider;
    public EnemyManager _manager;

    private void Start()
    {
        _manager.HealthComponent.OnDamage += UpdateLife;
        _manager.HealthComponent.OnHeal += UpdateLife;

        _slider = GetComponent<Slider>();
        _slider.value = 1f;
    }

    void UpdateLife(float amount)
    {
        _slider.value = _manager.HealthComponent.GetCurrentHealthPercentage()/100;
    }
}