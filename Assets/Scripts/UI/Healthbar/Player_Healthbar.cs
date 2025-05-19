using System;
using Entities;
using UnityEngine;
using UnityEngine.UI;

public class Player_Healthbar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private IHealth _health;
    public void Setup(IHealth health)
    {
        _health = health;
        slider.maxValue = _health.maxHealth;
        slider.value = _health.currentHealth;
        _health.OnDamage += OnDamaged;
        _health.OnHeal += OnHealed; 
    }
    private void OnDamaged(float damage)
    {
        slider.value = _health.currentHealth;
    }

    private void OnHealed(float heal)
    {
        slider.value = _health.currentHealth;
    }

    private void OnDestroy()
    {
        if (_health != null)
        {
            _health.OnDamage -= OnDamaged;
            _health.OnHeal -= OnHealed;
        }
    }
}