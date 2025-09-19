using System;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using Entities.Interfaces;
using Unity.Mathematics;
using UnityEngine;
using Utilities.Factory.WeaponFactory;
using Weapons;

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
        
        public event Action OnHit;
        public event Action OnWeaponChanged;
        public bool HasWeapon => Weapon != null;

        private void Awake()
        {
            ServiceLocator.Instance.RegisterService(this);
        }

        private void OnDestroy()
        {
            AttackFinishSubscription(false);
            ServiceLocator.Instance.RemoveService(this);
        }

        private void Start()
        {
            _weaponManager = ServiceLocator.Instance.GetService<WeaponManager>();
            _manager = ServiceLocator.Instance.GetService<PlayerManager>();
            if(startingWeapon!=null)
                EquipWeapon(_weaponManager.RequestWeapon(startingWeapon));
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
            if (Weapon.ConsumeDurabilityOnMissStandard())
                Weapon.AffectDurability();
        }

        public void ChargeAttack()
        {
            if (Weapon == null || !Weapon.CanAttack()) return;
            var controller = _manager.controller as PlayerController;
            if (Weapon.IsCharged())
            {
                controller?.ChangeToChargeAttack();
                Weapon.CurrentAttack = Weapon.ChargeAttack;
                if (Weapon.ConsumeDurabilityOnMissCharge())
                    Weapon.AffectDurability();
            }
        }

        #region EquipAndUnquip

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
            Weapon.WeaponGameObject.gameObject.transform.SetLocalPositionAndRotation(Vector3.up*offset, 
                quaternion.identity);
            AttackFinishSubscription(true);
            Weapon.WeaponGameObject.GetComponent<Collider2D>().enabled = false;
            weapon.BaseAttack.SetUp(weapon.WeaponGameObject,_manager.model,_manager.view,_manager.controller as PlayerController,this,_manager.controller);
            weapon.ChargeAttack.SetUp(weapon.WeaponGameObject,_manager.model,_manager.view,_manager.controller as PlayerController,this,_manager.controller);
            
            OnWeaponChanged?.Invoke();
        }

        public void DropWeapon()
        {
            if (Weapon is not { Attacking: false }) return;
            _weaponManager.CatchDroppedWeapon(Weapon);
            UnEquipWeapon();
        }

        private void DestroyWeapon()
        {
            if (Weapon is not { Attacking: false }) return;
            var weaponForDestruction = Weapon;
            UnEquipWeapon();
            _weaponManager.DestroyWeapon(weaponForDestruction);
            _manager.PlaySound("BreakWeapon", "Player");
        }

        private void UnEquipWeapon()
        {
            AttackFinishSubscription(false);
            Weapon.BaseAttack.OnUnequip();
            Weapon.ChargeAttack.OnUnequip();
            Weapon = null;
            
            OnWeaponChanged?.Invoke();
        }
        
        private void AttackFinishSubscription(bool subscribe)
        {
            if (subscribe)
            {
                Weapon.BaseAttack.AttackFinish += AttackFinish;
                Weapon.ChargeAttack.AttackFinish += AttackFinish;
            }
            else
            {
                Weapon.BaseAttack.AttackFinish -= AttackFinish;
                Weapon.ChargeAttack.AttackFinish -= AttackFinish;
            }
        }

        #endregion
        
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
            
            OnHit?.Invoke();
        }
        
        private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
        {
            return (layerMask.value & (1 << obj.layer)) != 0;
        }

        private void AttackFinish()
        {
            ClearEnemiesList();
            Weapon.WeaponGameObject.gameObject.transform.SetLocalPositionAndRotation(Vector3.up*offset, 
                quaternion.identity);
        }

        private void ClearEnemiesList()
        {
            _enemiesHit.Clear();
        }
        
        public void CheckDurability()
        {
            if (!Weapon.CheckDurability())
                DestroyWeapon();
        }
        
        public float CheckWeaponDurabilityPercent() => Weapon.DurabilityPercent();
        
        public float CheckWeaponChargePercent() => Weapon.ChargePercent();
    }
}