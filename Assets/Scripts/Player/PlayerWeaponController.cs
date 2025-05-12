using System.Collections.Generic;
using System.Linq;
using Enemies;
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
            //var size = Physics2D.OverlapCircle(transform.position, 1, mask);
            var weapon = ServiceLocator.Instance.GetService<WeaponManager>().PickUpWeaponInRange(transform.position, 1);
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsInLayerMask(other.gameObject, collisionLayer)) return;
        
            if (_enemiesHit.Any(hits => hits==other.gameObject)) return;
            
            ServiceLocator.Instance.GetService<EnemiesManager>().GetManager(other.gameObject).GetEnemyHealthSystem().TakeDamage(_weapon.BaseDamage);
            _enemiesHit.Add(other.gameObject);
        
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
