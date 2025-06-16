using System.Collections.Generic;
using System.Linq;
using Enemies;
using Entities.Interfaces;
using Unity.Mathematics;
using UnityEngine;
using Weapons;

namespace Player
{
    public class PlayerWeaponController : MonoBehaviour
    {
        [SerializeField] private GameObject playerRotation;
        [SerializeField] private LayerMask collisionLayer;
        [SerializeField] private PlayerManager manager;
        [SerializeField] private LayerMask mask;
        private WeaponManager _weaponManager;
        private Weapon _weapon;
        public Weapon Weapon => _weapon;
        public float offset;
        private readonly List<GameObject> _enemiesHit = new List<GameObject>();

        private void Awake()
        {
            ServiceLocator.Instance.RegisterService(this);
        }

        private void Start()
        {
            _weaponManager = ServiceLocator.Instance.GetService<WeaponManager>();
        }

        void Update()
        {
            LookDir();
            _weapon?.CooldownCounter();
        }

        private void LookDir()
        {
            transform.rotation = playerRotation.transform.rotation;
            transform.position = playerRotation.transform.position + (playerRotation.transform.up) * offset;
        }

        public void Attack()
        {
            if (_weapon == null || !_weapon.CanAttack()) return;
            var controller = manager.controller as PlayerController;
            controller?.ChangeToAttack();
            _weapon.CurrentAttack = _weapon.BaseAttack;
        }

        public void ChargeAttack()
        {
            if (_weapon == null || !_weapon.CanAttack()) return;
            var controller = manager.controller as PlayerController;
            if (_weapon.CheckCharge())
            {
                controller?.ChangeToChargeAttack();
                _weapon.CurrentAttack = _weapon.ChargeAttack;
            }
        }

        public void GrabWeapon()
        {
            if (_weapon != null) return;
            var weapon = _weaponManager.PickUpWeaponInRange(transform.position, 1);
            EquipWeapon(weapon);
        }

        private void EquipWeapon(Weapon weapon)
        {
            if (weapon == default) return;
            _weapon = weapon;
            _weapon.WeaponGameObject.gameObject.transform.parent = transform;
            _weapon.WeaponGameObject.gameObject.transform.SetLocalPositionAndRotation(Vector3.zero,
                quaternion.identity);
            _weapon.BaseAttack.FinishAnimation += ClearEnemiesList;
            _weapon.WeaponGameObject.GetComponent<Collider2D>().enabled = false;
        }

        public void DropWeapon()
        {
            if (_weapon is not { Attacking: false }) return;

            _weapon.WeaponGameObject.GetComponent<Collider2D>().enabled = true;
            _weapon.BaseAttack.FinishAnimation -= ClearEnemiesList;
            _weapon.WeaponGameObject.transform.parent = null;
            _weapon = null;
        }

        private void DestroyWeapon()
        {
            if (_weapon is not { Attacking: false }) return;

            _weapon.BaseAttack.FinishAnimation -= ClearEnemiesList;
            _weaponManager.DestroyWeapon(_weapon);
            _weapon = null;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsInLayerMask(other.gameObject, collisionLayer) || _enemiesHit.Any(hits => hits == other.gameObject))
                return;

            var enemyManager = ServiceLocator.Instance.GetService<EnemiesManager>().GetManager(other.gameObject);
            enemyManager.HealthComponent.TakeDamage(_weapon.Damage());
            if (enemyManager.model is IKnockbackable knockbackable)
                knockbackable.ApplyKnockbackFromSource(this.transform.position, _weapon.KnockbackForce);
            _weapon.ChargeWeapon();
            _weapon.AffectDurability();
            _enemiesHit.Add(other.gameObject);
        }

        public void CheckDurability()
        {
            if (!_weapon.CheckDurability())
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

        public float CheckWeaponDurabilityPercent() => _weapon.DurabilityPercent();
        public float CheckWeaponChargePercent() => _weapon.ChargePercent();
    }
}