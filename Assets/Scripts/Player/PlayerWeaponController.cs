using System.Collections.Generic;
using System.Linq;
using Enemies;
using Entities.Interfaces;
using Unity.Mathematics;
using UnityEngine;
using Utilities.Factory.WeaponFactory;

using Weapons;
using System;

namespace Player
{
    public class PlayerWeaponController : MonoBehaviour
    {
        [SerializeField] private LayerMask collisionLayer;
        [SerializeField] private SoWeapon startingWeapon;
        private Transform _playerRotation;
        private PlayerManager _manager;
        private WeaponManager _weaponManager;
        public Weapon Weapon { get; private set; }
        public float offset;
        private readonly List<GameObject> _enemiesHit = new List<GameObject>();
        public static event Action OnPlayerWeaponPicked;
        public static event Action OnPlayerWeaponDropped;
        public static event Action OnPlayerAttacked;
        public static event Action OnPlayerChargedAttack;
        private void Awake()
        {
            ServiceLocator.Instance.RegisterService(this);
        }

        private void Start()
        {
            _weaponManager = ServiceLocator.Instance.GetService<WeaponManager>();
            _manager = ServiceLocator.Instance.GetService<PlayerManager>();
            if (startingWeapon != null)
            {
                EquipWeapon(_weaponManager.RequestWeapon(startingWeapon));
            }
            _playerRotation = _manager.model.transform;
        }

        void Update()
        {
            LookDir();
            Weapon?.CooldownCounter();
        }

        private void LookDir()
        {
            transform.rotation = _playerRotation.rotation;
            transform.position = _playerRotation.position + (_playerRotation.up) * offset;
        }

        public void Attack()
        {
            if (Weapon == null || !Weapon.CanAttack()) return;
            var controller = _manager.controller as PlayerController;
            controller?.ChangeToAttack();
            Weapon.CurrentAttack = Weapon.BaseAttack;
            OnPlayerAttacked?.Invoke();
        }

        public void ChargeAttack()
        {
            if (Weapon == null || !Weapon.CanAttack()) return;
            var controller = _manager.controller as PlayerController;
            if (Weapon.CheckCharge())
            {
                controller?.ChangeToChargeAttack();
                Weapon.CurrentAttack = Weapon.ChargeAttack;
                OnPlayerChargedAttack?.Invoke();
            }
        }

        public void GrabWeapon()
        {
            if (Weapon != null) return;
            var weapon = _weaponManager.PickUpWeaponInRange(transform.position, 1);
            EquipWeapon(weapon);

        }

        private void EquipWeapon(Weapon weapon)
        {
            if (weapon == default) return;
            Weapon = weapon;
            Weapon.WeaponGameObject.gameObject.transform.parent = transform;
            Weapon.WeaponGameObject.gameObject.transform.SetLocalPositionAndRotation(Vector3.zero,
                quaternion.identity);
            Weapon.BaseAttack.FinishAnimation += ClearEnemiesList;
            Weapon.WeaponGameObject.GetComponent<Collider2D>().enabled = false;
            OnPlayerWeaponPicked?.Invoke();
        }

        public void DropWeapon()
        {
            if (Weapon is not { Attacking: false }) return;
            _weaponManager.CatchDroppedWeapon(Weapon);
            Weapon.BaseAttack.FinishAnimation -= ClearEnemiesList;
            Weapon.ChargeAttack.FinishAnimation -= ClearEnemiesList;
            Weapon = null;
            OnPlayerWeaponDropped?.Invoke();
        }

        private void DestroyWeapon()
        {
            if (Weapon is not { Attacking: false }) return;

            Weapon.BaseAttack.FinishAnimation -= ClearEnemiesList;
            Weapon.ChargeAttack.FinishAnimation -= ClearEnemiesList;
            _weaponManager.DestroyWeapon(Weapon);
            _manager.PlaySound("BreakWeapon", "Player");
            Weapon = null;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsInLayerMask(other.gameObject, collisionLayer) || _enemiesHit.Any(hits => hits == other.gameObject))
                return;
            
            var enemyManager = ServiceLocator.Instance.GetService<EnemiesManager>().GetManager(other.gameObject);
            if (enemyManager==null)return;
            
            enemyManager.HealthComponent.TakeDamage(Weapon.Damage());
            if (enemyManager.model is IKnockbackable knockbackable)
                knockbackable.ApplyKnockbackFromSource(this.transform.position, Weapon.KnockbackForce);
            Weapon.ChargeWeapon();
            Weapon.AffectDurability();
            _enemiesHit.Add(other.gameObject);
        }

        public void CheckDurability()
        {
            if (!Weapon.CheckDurability())
                DestroyWeapon();
        }

        bool IsInLayerMask(GameObject obj, LayerMask layerMask)
        {
            return (layerMask.value & (1 << obj.layer)) != 0;
        }

        private void ClearEnemiesList()
        {
            _enemiesHit.Clear();
        }

        public float CheckWeaponDurabilityPercent() => Weapon.DurabilityPercent();
        public float CheckWeaponChargePercent() => Weapon.ChargePercent();
    }
}