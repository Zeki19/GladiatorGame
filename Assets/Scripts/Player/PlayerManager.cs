using System;
using Core.Status;
using Entities;
using UnityEngine;

namespace Player
{
    public class PlayerManager : EntityManager
    {
        public PlayerStats stats;
        public PlayerWeaponController weaponController;
        [SerializeField]private EntityStatusManager<PlayerStatus> _status;
        public event Action<float> OnDash;

        private void Awake()
        {
            ServiceLocator.Instance.RegisterService(this);
            HealthSystem = new HealthSystem.HealthSystem(stats.MaxHealth);
            HealthSystem.OnDamage += DamageTaken;
            _status.SetUpStatus();
        }

        //private void OnDestroy()
        //{
        //    ServiceLocator.Instance.RemoveService(this);
        //}

        private void DamageTaken(float a)
        {
            PlaySound("Hit");
            //ServiceLocator.Instance.GetService<ArenaPainter>().PaintArena(transform, "Blood");
        }
        public void TriggerDash(float dashCooldown)
        {
            OnDash?.Invoke(dashCooldown);
        }
    }
}
