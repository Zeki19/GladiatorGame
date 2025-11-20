using System;
using System.Collections;
using Entities;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResourcesUI : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private EntityManager entityManager;
    [Header("Bars")]
    [SerializeField] private ResourceBarTracker healthBar;
    [SerializeField] private ResourceBarTracker chargeBar;
    private bool chargeBarSetUp = false; 
    [SerializeField] private Image dashIcon;

    private PlayerWeaponController _weaponController;
    
    [Header("Charged bar effect")]
    [SerializeField] private Color chargedColorA = Color.green;
    [SerializeField] private Color chargedColorB = Color.yellow;
    [SerializeField] private float pulseSpeed = 2f;

    private Coroutine _pulseCoroutine;
    
    private void Start()
    {
        _weaponController = ServiceLocator.Instance.GetService<PlayerWeaponController>();
        
        SetUpHealthBar();
        entityManager.HealthComponent.OnDamage += Damage;
        entityManager.HealthComponent.OnHeal += Heal;
        entityManager.HealthComponent.OnDead += Dead;
        
        UpdateChargeBar();
        _weaponController.OnWeaponChanged += UpdateChargeBar;
        _weaponController.OnAttack += UpdateChargeBar;

        ServiceLocator.Instance.GetService<PlayerManager>().OnDash += Dashed;
    }

    private void Damage(float value)
    {
        healthBar.ChangeResourceByAmount((int)value * -1);
    }
    private void Heal(float value)
    {
        healthBar.ChangeResourceByAmount((int)value);
    }

    private void SetUpHealthBar()
    {
        healthBar.SetUp((int)entityManager.HealthComponent.maxHealth, true);
    }
    
    private readonly int _multiplier = 10;
    private void UpdateChargeBar()
    {
        if (_weaponController.HasWeapon)
        {
            ChargedEffect(_weaponController.Weapon.IsCharged());
            
            var max = _weaponController.Weapon.MaxCharge() * _multiplier;
            chargeBar.SetUp((int)max, false);
            
            var num = _weaponController.CheckWeaponChargePercent() * _multiplier;
            chargeBar.ChangeResourceToAmount((int)num);
            return;
        }
        
        chargeBar.SetUp((int)1, false);
    }

    private void ChargedEffect(bool value)
    {
        if (!chargeBar.isActiveAndEnabled) return;
        
        if (value)
        {
            _pulseCoroutine ??= StartCoroutine(PulseColors());
        }
        else
        {
            if (_pulseCoroutine != null)
            {
                StopCoroutine(_pulseCoroutine);
                _pulseCoroutine = null;
            }

            chargeBar.ResetColor();
        }
    }

    private IEnumerator PulseColors()
    {
        float t = 0f;
        while (true)
        {
            t += Time.deltaTime * pulseSpeed;
            float lerp = Mathf.PingPong(t, 1f);
            Color currentColor = Color.Lerp(chargedColorA, chargedColorB, lerp);
            chargeBar.ChangeColor(currentColor);
            yield return null;
        }
    }

    private void Dead()
    {
        entityManager.HealthComponent.OnDamage -= Damage;
        entityManager.HealthComponent.OnHeal -= Heal;
        entityManager.HealthComponent.OnDead -= Dead;
    }

    private void Dashed(float seconds)
    {
        dashIcon.color = new Color(dashIcon.color.r, dashIcon.color.g, dashIcon.color.b, 0f);
        
        if(gameObject.activeInHierarchy && dashIcon.gameObject.activeInHierarchy)
            StartCoroutine(FadeInDashIcon(seconds));
    }
    
    private IEnumerator FadeInDashIcon(float duration)
    {
        float time = 0f;
        Color startColor = dashIcon.color;
        Color endColor = new Color(dashIcon.color.r, dashIcon.color.g, dashIcon.color.b, .4f);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // Exponential-like growth (eases in quickly, then slows)
            float curve = 1f - Mathf.Exp(-5f * t);

            dashIcon.color = Color.Lerp(startColor, endColor, curve);
            yield return null;
        }

        // Ensure it's fully visible at the end
        dashIcon.color = new Color(dashIcon.color.r, dashIcon.color.g, dashIcon.color.b, 1f);
    }
}
