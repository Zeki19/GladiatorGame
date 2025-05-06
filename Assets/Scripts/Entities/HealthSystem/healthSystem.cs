using System;
using UnityEngine;

namespace HealthSystem
{
    public class HealthSystem : IHealth
    {
        private float _maxHealth;
        private float _currentHealth;
        private bool _isInvulnerable;
        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public float maxHealth
        {
            get => _maxHealth;
            private set => _maxHealth = Mathf.Max(0, value);
        }
        public float currentHealth
        {
            get => _currentHealth;
            private set => _currentHealth = Mathf.Clamp(value, 0, maxHealth);
        }

        public bool isInvulnerable
        {
            get => _isInvulnerable;
            set => _isInvulnerable = value;
        }

        #region EVENTS

        /// <summary>
        /// Event triggered when the entity dies (health reaches 0).
        /// </summary>
        public event Action OnDead;

        /// <summary>
        /// Event triggered when the entity is healed.
        /// The float parameter indicates the amount healed.
        /// </summary>
        public event Action<float> OnHeal;

        /// <summary>
        /// Event triggered when the entity takes damage.
        /// The float parameter indicates the amount of damage received.
        /// </summary>
        public event Action<float> OnDamage;

        #endregion

        /// <summary>
        /// Initializes a new instance of the HealthSystem with a specified maximum health.
        /// </summary>
        /// <param name="MaxHealth">The maximum health value to initialize with.</param>
        public HealthSystem(float MaxHealth)
        {
            maxHealth = MaxHealth;
            currentHealth = maxHealth;
            isInvulnerable = false;
        }

        /// <summary>
        /// Increases current health by a specified amount and invokes the OnHeal event.
        /// </summary>
        /// <param name="healingAmount">The amount of health to restore.</param>
        public void Heal(float healingAmount)
        {
            currentHealth += healingAmount;
            OnHeal?.Invoke(healingAmount);
        }

        /// <summary>
        /// Returns the maximum health value.
        /// </summary>
        public float GetMaxHealth()
        {
            return maxHealth;
        }

        /// <summary>
        /// Returns the current health value.
        /// </summary>
        public float GetCurrentHealth()
        {
            return currentHealth;
        }

        /// <summary>
        /// Returns the current health as a percentage of max health.
        /// </summary>
        public float GetCurrentHealthPercentage()
        {
            return (currentHealth / maxHealth) * 100f;
        }

        /// <summary>
        /// Fully restores health to the maximum value and invokes the OnHeal event.
        /// </summary>
        public void FullHeal()
        {
            currentHealth = maxHealth;
            OnHeal?.Invoke(maxHealth);
        }

        /// <summary>
        /// Reduces current health by a specified amount and invokes the OnDamage event.
        /// If health falls to zero or below, triggers the Kill method.
        /// </summary>
        /// <param name="damageAmount">The amount of damage to apply.</param>
        public void TakeDamage(float damageAmount)
        {
            if (isInvulnerable) return;
            currentHealth -= damageAmount;
            OnDamage?.Invoke(damageAmount);
            if (currentHealth <= 0)
            {
                Kill();
            }
        }

        /// <summary>
        /// Forces the entity to take lethal damage equal to its max health.
        /// Triggers the OnDead event if health drops to zero or below.
        /// </summary>
        public void Kill()
        {
            if (isInvulnerable) return;
            currentHealth -= maxHealth;
            if (currentHealth <= 0)
            {
                OnDead?.Invoke();
            }
            else
            {
                Debug.LogError("Did not die!!!");
            }
        }
        /// <summary>
        /// Checks whether the entity is still alive (health greater than zero).
        /// </summary>
        public bool IsAlive()
        {
            return currentHealth > 0;
        }
    }
}