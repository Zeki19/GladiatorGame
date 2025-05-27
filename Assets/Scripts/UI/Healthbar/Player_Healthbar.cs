using System;
using Entities;
using UnityEngine;
using UnityEngine.UI;


public class Player_Healthbar : MonoBehaviour
{
    [SerializeField] private Image CircleBar;
    private IHealth _health;

    public void Setup(IHealth health)
    {
        _health = health;
        UpdateFill();
        _health.OnDamage += OnDamaged;
        _health.OnHeal += OnHealed;
    }

    private void OnDamaged(float damage)
    {
        UpdateFill();
    }

    private void OnHealed(float heal)
    {
        UpdateFill();
    }

    private void UpdateFill()
    {
        if (_health == null || _health.maxHealth <= 0) return;

        float pct = _health.currentHealth / _health.maxHealth;
        CircleBar.fillAmount = pct;
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