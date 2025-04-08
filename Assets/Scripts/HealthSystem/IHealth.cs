using System;
using UnityEngine;

public interface IHealth
{
    float maxHealth { get; }
    float currentHealth { get; }

    event Action OnDead;
    event Action<float> OnHeal;
    event Action<float> OnDamage;

    void Heal(float healingAmount);
    float GetMaxHealth();
    float GetCurrentHealth();
    float GetCurrentHealthPercentage();
    void FullHeal();
    void TakeDamage(float damageAmount);
    void Kill();
    bool IsAlive();
}
