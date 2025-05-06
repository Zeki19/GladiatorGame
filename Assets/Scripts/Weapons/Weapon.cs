using Factory.Essentials;
using UnityEngine;
using Utilitys.Factory.WeaponFactory;

namespace Weapons
{
    public class Weapon:IConfigurable<SoWeapon>
    {
        public bool Attacking=false;
        public GameObject WeaponGameObject;
        private Collider2D _weaponCollider2D;
        private string _weaponName;
        public float BaseDamage;
        public float AttackSpeed;
        public float Range;
        private int _durability;
        private int _durabilityStandardLoss;
        private int _durabilityChargeLoss;
        //private enum _attackType{};
        private float _knockbackForce;
        public float SlowPercent;
        private float _changeThreshold;
        private float _chargePerAttack;
        private float _currentDurability;
        private bool _isOnCooldown;
        private bool _canAttack;
        //private Player _owner;
        private float _chargeMeter;
        public Attack BaseSoAttack;
        public Attack ChargeSoAttack;
        public Attack CurrentAttack;
    
        public void Configure(SoWeapon config)
        {
            WeaponGameObject=Object.Instantiate(config.weaponPrefab);
            WeaponGameObject.GetComponent<WeaponMono>().Weapon = this;
            _weaponCollider2D = WeaponGameObject.GetComponent<Collider2D>();
            _weaponName = config.weaponName;
            BaseDamage = config.baseDamage;
            AttackSpeed = config.attackSpeed;
            Range = config.range;
            _durability = config.durability;
            _durabilityStandardLoss = config.durabilityStandardLoss;
            _durabilityChargeLoss = config.durabilityChargeLoss;
            _knockbackForce = config.knockbackForce;
            SlowPercent = config.slowPercent;
            _changeThreshold = config.changeThreshold;
            _chargePerAttack = config.chargePerAttack;
            BaseSoAttack = config.basic.Clone();
        }
        public void SetCollision(bool state)
        {
            _weaponCollider2D.enabled = state;
        }
    }
}
