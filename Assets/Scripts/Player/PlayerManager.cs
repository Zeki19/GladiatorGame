using System;
using System.Collections.Generic;
using Core.Status;
using Entities;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

namespace Player
{
    public class PlayerManager : EntityManager
    {
        public PlayerStats stats;
        public PlayerWeaponController weaponController;
        [SerializeField]private EntityStatusManager<PlayerStatus> _status;


        public DashIcon dashIconUI; 

        private void Awake()
        {
            ServiceLocator.Instance.RegisterService(this);
            HealthSystem = new HealthSystem.HealthSystem(stats.MaxHealth);
            HealthSystem.OnDamage += damageTaken;
            _status.SetUpStatus();
        }

        private void damageTaken(float a)
        {
            PlaySound("Hit","Player");
        }

        private void Update()
        {
            Debug.Log(_status.GetStatus(CommonStatus.IsAlive));
            Debug.Log(_status.GetStatus(PlayerStatus.IsAlive));
        }
    }
}
