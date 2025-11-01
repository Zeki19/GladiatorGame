using System;
using System.Collections;
using System.Collections.Generic;
using Factory.Essentials;
using UnityEngine;
using Utilities.Factory.WeaponFactory;
using Random = UnityEngine.Random;

namespace Weapons
{
    public class WeaponManager : MonoBehaviour
    {
        #region ForTesting
        
            private Factory<Weapon, SoWeapon> _factory;

        #endregion

        [SerializeField] private Transform inactiveWeapon;
        private WeaponPool _pool;
        
        [Header("Weapons list")]
        [SerializeField] private List<SoWeapon>  weapons;
        private Dictionary<GameObject, Weapon> _droppedWeapons;
        
        [Header("SpawnPoints list")]
        [SerializeField] private List<Vector3> spawnPoints;
        [SerializeField] private List<Vector3> usedSpawnPoint;
        
        [Header("Respawn config")]
        [SerializeField] private Vector2 RespawnTimeRange;
        [SerializeField] private Vector2Int RespawnWeaponQuantity;
        
        private void Awake()
        {
            _factory = new Factory<Weapon, SoWeapon>();
            _droppedWeapons = new Dictionary<GameObject, Weapon>();
            _pool = new WeaponPool(_factory.Create);
            ServiceLocator.Instance.RegisterService(this);
        }
        private void Start()
        {
            CrateWeaponInRandomPoint();
            CrateWeaponInRandomPoint();
        }

        IEnumerator RespawnWeapon()
        {
            yield return new WaitForSeconds(Random.Range(RespawnTimeRange.x, RespawnTimeRange.y));
            for (int i = 0; i < Random.Range(RespawnWeaponQuantity.x,RespawnWeaponQuantity.y+1); i++)
            {
                CrateWeaponInRandomPoint();
            }
        }
        
        private Weapon CreateWeapon(SoWeapon weaponConfig)
        {
            var newWeapon = _pool.Get(weaponConfig);
            _droppedWeapons.Add(newWeapon.WeaponGameObject,newWeapon);
            newWeapon.WeaponGameObject.transform.parent = transform;
            newWeapon.WeaponGameObject.SetActive(true);
            return newWeapon;
        }
        public Weapon RequestWeapon(SoWeapon weaponConfig)
        {
            Weapon weapon =CreateWeapon(weaponConfig);
            _droppedWeapons.Remove(weapon.WeaponGameObject);
            return weapon;
        }
        
        [ContextMenu("Spawn Weapon")]
        private void CrateWeaponInRandomPoint()
        {
            Weapon weapon = CreateWeapon(weapons[Random.Range(0,weapons.Count)]);

            var point = GetRandomSpawnPoint();
            weapon.WeaponGameObject.transform.position = point;
            
            float randomZ = Random.Range(0f, 360f);
            weapon.WeaponGameObject.transform.rotation = Quaternion.Euler(0f, 0f, randomZ);
        }
        private Vector3 GetRandomSpawnPoint()
        {
            if (spawnPoints.Count == 0)
            {
                spawnPoints = usedSpawnPoint;
                usedSpawnPoint.Clear();
            }
            
            var index = Random.Range(0, spawnPoints.Count);
            var returnValue = spawnPoints[index];
            spawnPoints.RemoveAt(index);
            usedSpawnPoint.Add(returnValue);
            
            return returnValue;
        }
        
        public void DestroyWeapon(Weapon weapon)
        {
            _pool.Release(weapon);
            weapon.WeaponGameObject.transform.parent = inactiveWeapon;
            weapon.WeaponGameObject.SetActive(false);
            if (_droppedWeapons.Count == 0)
                StartCoroutine(RespawnWeapon());
        }
        public Weapon PickUpWeaponInRange(Vector3 pos, float range)
        {
            foreach (var weapon in _droppedWeapons)
                if (Vector3.Distance(pos, weapon.Key.transform.position) < range)
                {
                    Weapon ToGive;
                    ToGive = weapon.Value;
                    _droppedWeapons.Remove(weapon.Key);
                    Debug.Log(_droppedWeapons.Count-1);
                    return ToGive;
                }
            return default;
        }
        public void CatchDroppedWeapon(Weapon weapon)
        {
            _droppedWeapons.Add(weapon.WeaponGameObject,weapon);
            weapon.WeaponGameObject.GetComponent<Collider2D>().enabled = true;
            weapon.WeaponGameObject.transform.parent = transform;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                Gizmos.DrawSphere(spawnPoints[i],0.5f);
                
            }
        }
    }
}
