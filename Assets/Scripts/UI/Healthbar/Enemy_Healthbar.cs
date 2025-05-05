using System;
using Entities;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Healthbar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private IHealth _health;
    private void Start()
    {
        _health = ServiceLocator.Instance.GetService<EnemyManager>().HealthComponent;
        SetHealth();
    }
    void SetHealth()
    {
        slider.maxValue = _health.maxHealth;
        slider.value = _health.currentHealth;
        _health.OnDamage += OnDamaged;
    }
    private void OnDamaged(float damage) => slider.value = _health.currentHealth;
}
