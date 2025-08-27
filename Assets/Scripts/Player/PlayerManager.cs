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
            HealthSystem.OnDamage += DamageTaken;
            _status.SetUpStatus();
        }

        private void DamageTaken(float a)
        {
            PlaySound("Hit","Player");
            //ServiceLocator.Instance.GetService<ArenaPainter>().PaintArena(transform, "Blood");
        }
    }
}
