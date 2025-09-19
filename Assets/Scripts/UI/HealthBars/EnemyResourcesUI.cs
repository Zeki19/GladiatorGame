using Entities;
using UnityEngine;

public class EnemyResourcesUI : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private EntityManager entityManager;
    [Header("Bars")]
    [SerializeField] private ResourceBarTracker healthBar;
    void Start()
    {
        entityManager.HealthComponent.OnDamage += Damage;
        entityManager.HealthComponent.OnHeal += Heal;
        entityManager.HealthComponent.OnDead += Dead;

        SetUpHealthBar();
    }
    
    private void SetUpHealthBar()
    {
        healthBar.SetUp((int)entityManager.HealthComponent.maxHealth, true);
    }

    private void Damage(float value)
    {
        healthBar.ChangeResourceByAmount((int)value * -1);
    }
    private void Heal(float value)
    {
        healthBar.ChangeResourceByAmount((int)value);
    }
    private void Dead()
    {
        entityManager.HealthComponent.OnDamage -= Damage;
        entityManager.HealthComponent.OnHeal -= Heal;
        entityManager.HealthComponent.OnDead -= Dead;
    }
}
