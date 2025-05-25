using System;
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
        [SerializeField] private PlayerManager manager;
        [SerializeField] private GameObject playerRotation;
        [SerializeField] private LayerMask collisionLayer;
        private List<GameObject> _enemiesHit= new List<GameObject>();
        private Weapon _weapon;
        public LayerMask mask;
        public float offset;
        private WeaponManager _weaponManager;

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
        }

        private void LookDir()
        {
            transform.rotation = playerRotation.transform.rotation;
            transform.position = playerRotation.transform.position +(playerRotation.transform.up)*offset;
        }

        public void Attack()
        {
            if (_weapon!=null)
            {
               var controller= manager.controller as PlayerController;
               controller?.ChangeToAttack();
            }
        }
        public void GrabWeapon()
        {
            if (_weapon != null) return;
            var weapon = _weaponManager.PickUpWeaponInRange(transform.position, 1);
            if (weapon != default)
            {
                _weapon = weapon;
                _weapon.WeaponGameObject.gameObject.transform.parent = transform;
                _weapon.WeaponGameObject.gameObject.transform.localPosition=Vector3.zero;
                _weapon.WeaponGameObject.gameObject.transform.localRotation=quaternion.identity;
                _weapon.BaseSoAttack.FinishAnimation += ClearEnemiesList;
                _weapon.WeaponGameObject.GetComponent<Collider2D>().enabled = false;
                manager.weapon = _weapon;
            }
        }

        public void DropWeapon()
        {
            if(_weapon is { Attacking: false })
            {
                _weapon.WeaponGameObject.GetComponent<Collider2D>().enabled = true;
                _weapon.BaseSoAttack.FinishAnimation -= ClearEnemiesList;
                _weapon.WeaponGameObject.transform.parent = null;
                _weapon = null;
            }
        }

        private void DestroyWeapon()
        {
            if(_weapon is { Attacking: false })
            {
                _weapon.BaseSoAttack.FinishAnimation -= ClearEnemiesList;
                _weaponManager.DestroyWeapon(_weapon);
                _weapon = null;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsInLayerMask(other.gameObject, collisionLayer)) return;
        
            if (_enemiesHit.Any(hits => hits==other.gameObject)) return;
            var enemyManager = ServiceLocator.Instance.GetService<EnemiesManager>().GetManager(other.gameObject);
            enemyManager.HealthComponent.TakeDamage(_weapon.BaseDamage);
            if (enemyManager.model is IKnockbackable knockbackable)
            {
                knockbackable.ApplyKnockbackFromSource(this.transform.position,_weapon.KnockbackForce);
            }
            _weapon.AffectDurability(_weapon.DurabilityStandardLoss);
            _enemiesHit.Add(other.gameObject);
        }

        public void CheckDurability()
        {
            if (!_weapon.AffectDurability(0))
            {
                DestroyWeapon();
            }
        }
        bool IsInLayerMask(GameObject obj, LayerMask layerMask)
        {
            return (layerMask.value & (1 << obj.layer)) != 0;
        }

        private void ClearEnemiesList()
        {
            _enemiesHit.Clear();
        }
    }
}
