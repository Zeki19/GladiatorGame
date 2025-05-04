using System.Collections.Generic;
using System.Linq;
using Enemies;
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
        public float offset;
        
        void Start()
        {
            CreateAWeapon();
            _weapon._baseSoAttack.FinishAnimation += ClearEnemiesList;
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
        private void CreateAWeapon()
        {
            _weapon=ServiceLocator.Instance.GetService<WeaponManager>().GetWeapon();
            _weapon.WeaponGameObject.transform.parent = this.transform;
            manager.weapon = _weapon;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsInLayerMask(other.gameObject, collisionLayer)) return;
        
            if (_enemiesHit.Any(hits => hits==other.gameObject)) return;
            
            ServiceLocator.Instance.GetService<EnemiesManager>().GetManager(other.gameObject).GetEnemyHealthSystem().TakeDamage(_weapon._baseDamage);
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
