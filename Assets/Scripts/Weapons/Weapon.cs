using Factory.Essentials;
using UnityEngine;
using Utilitys.Factory.Essentials;
using Utilitys.Factory.WeaponFactory;
using Weapons.Attacks;

namespace Weapons
{
    public class Weapon:IConfigurable<SoWeapon>
    {
        public string WeaponName { get; private set; }
        public GameObject WeaponGameObject{ get; private set; }
        private Collider2D _weaponCollider2D;
        #region BaseStats

            public float BaseDamage { get; private set; }
            public float AttackSpeed{ get; private set; }
            public float Range{ get; private set; }
            public float KnockbackForce{ get; private set; }
            public float SlowPercent{ get; private set; }

        #endregion
        #region Durability

            public int Durability{ get; private set; }
            public int DurabilityStandardLoss{ get; private set; }
            public int DurabilityChargeLoss{ get; private set; }
            public float CurrentDurability{ get; private set; }

        #endregion
        #region Charge

            public float ChangeThreshold{ get; private set; }
            public float ChargePerAttack{ get; private set; }
            public float ChargeMeter{ get; private set; }

        #endregion
        #region Bools
        
        public bool Attacking { get; set; } = false;
        public bool IsOnCooldown{ get; private set; }
        public bool CanAttack{ get; private set; }
        
        #endregion
        #region Attacks

            public Attack BaseSoAttack{ get; private set; }
            public Attack ChargeSoAttack{ get; private set; }
            public Attack CurrentAttack{ get; private set; }

        #endregion
        
    
        public void Configure(SoWeapon config)
        {
            DurabilityStandardLoss = config.durabilityStandardLoss;
            DurabilityChargeLoss = config.durabilityChargeLoss;
            ChargePerAttack = config.chargePerAttack;
            ChangeThreshold = config.changeThreshold;
            KnockbackForce = config.knockbackForce;
            SlowPercent = config.slowPercent;
            AttackSpeed = config.attackSpeed;
            WeaponName = config.weaponName;
            Durability = config.durability;
            BaseDamage = config.baseDamage;
            Range = config.range;
            
            ChargeSoAttack = config.charge.Clone();
            BaseSoAttack = config.basic.Clone();

            CurrentDurability = Durability;
        }
        public void Initialize(SoWeapon config)
        {
            WeaponGameObject=Object.Instantiate(config.weaponPrefab);
            _weaponCollider2D = WeaponGameObject.GetComponent<Collider2D>();
        }
        public void SetCollision(bool state)
        {
            _weaponCollider2D.enabled = state;
        }
        /// <summary>
        /// This is an addition if you want to damage use a negative number
        /// </summary>
        /// <param name="durabilityChange"><br /> Positive = Repair<br /> Negative = Damage</param>
        /// <returns>Return False if Broken</returns>
        public bool AffectDurability(float durabilityChange)
        {
            CurrentDurability += durabilityChange;
            CurrentDurability = Mathf.Clamp(CurrentDurability, 0, Durability);
            Debug.Log("Current Weapon Durability :" + CurrentDurability);
            return !(CurrentDurability <= 0);
        }
    }
}
