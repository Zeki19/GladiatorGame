using Factory.Essentials;
using UnityEngine;
using Utilities.Factory.Essentials;
using Utilities.Factory.WeaponFactory;
using Weapons.Attacks;

namespace Weapons
{
    public class Weapon : IConfigurable<SoWeapon>
    {
        public string WeaponName { get; private set; }
        public GameObject WeaponGameObject { get; private set; }
        private Collider2D _weaponCollider2D;

        #region BaseStats

        public float BaseDamage { get; private set; }
        public float ChargeDamage { get; private set; }
        public float AttackSpeed { get; private set; }
        public float Range { get; private set; }
        public float KnockbackForce { get; private set; }
        public float SlowPercent { get; private set; }

        #endregion

        #region Cooldown

        private float CoolDown { get; set; }
        private float _timer;

        #endregion

        #region Durability

        private int Durability { get; set; }
        private int DurabilityStandardLoss { get; set; }
        private int DurabilityChargeLoss { get; set; }
        private float CurrentDurability { get; set; }

        #endregion

        #region Charge

        private float ChangeThreshold { get; set; }
        private float ChargePerAttack { get; set; }
        private float ChargeMeter { get; set; }

        #endregion

        #region Bools

        public bool Attacking { get; set; } = false;
        private bool IsOnCooldown { get; set; }

        #endregion

        #region Attacks

        public Attack BaseAttack { get; private set; }
        public Attack ChargeAttack { get; private set; }
        public Attack CurrentAttack { get; set; }

        #endregion


        public void Configure(SoWeapon config)
        {
            DurabilityStandardLoss = config.durabilityStandardLoss;
            DurabilityChargeLoss = config.durabilityChargeLoss;
            ChargePerAttack = config.chargePerAttack;
            ChangeThreshold = config.changeThreshold;
            KnockbackForce = config.knockbackForce;
            ChargeDamage = config.chargeDamage;
            SlowPercent = config.slowPercent;
            AttackSpeed = config.attackSpeed;
            WeaponName = config.weaponName;
            Durability = config.durability;
            BaseDamage = config.baseDamage;
            CoolDown = config.cooldown;
            Range = config.range;

            ChargeAttack = config.charge.Clone();
            BaseAttack = config.basic.Clone();

            CurrentDurability = Durability;

            _timer = 0;
        }

        public void Initialize(SoWeapon config)
        {
            WeaponGameObject = Object.Instantiate(config.weaponPrefab);
            _weaponCollider2D = WeaponGameObject.GetComponent<Collider2D>();
        }

        public void SetCollision(bool state) => _weaponCollider2D.enabled = state;

        /// <summary>
        /// This is an addition if you want to damage use a negative number
        /// </summary>
        /// <returns>Return False if Broken</returns>
        public bool AffectDurability()
        {
            CurrentDurability -= IsCurrentAttackBase() ? DurabilityStandardLoss : DurabilityChargeLoss;
            CurrentDurability = Mathf.Clamp(CurrentDurability, 0, Durability);
            Debug.Log("Current Weapon Durability :" + CurrentDurability);
            return CheckDurability();
        }

        public bool CheckDurability() => !(CurrentDurability <= 0);
        private bool IsCurrentAttackBase() => CurrentAttack == BaseAttack;
        public bool CheckCharge() => ChargeMeter >= ChangeThreshold;

        public void ChargeWeapon()
        {
            if (IsCurrentAttackBase())
                ChargeMeter += ChargePerAttack;
        }

        public void ResetChangeMeter() => ChargeMeter = 0;

        public bool CanAttack() => !IsOnCooldown;

        public void CooldownCounter()
        {
            if (_timer <= 0)
                IsOnCooldown = false;
            _timer -= Time.deltaTime;
        }

        public void PutOnCooldown()
        {
            IsOnCooldown = true;
            _timer = CoolDown;
        }

        public float Damage() => IsCurrentAttackBase() ? BaseDamage : ChargeDamage;
    }
}