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
        public List<SoWeapon>  forTest;
        private Dictionary<GameObject, Weapon> _droppedWeapons;
        [SerializeField] private List<Vector3> spawnPoints;
        [SerializeField] private List<Vector3> UsedspawnPoints;
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
        private void CrateWeaponInRandomPoint()
        {
            Weapon weapon= CreateWeapon(forTest[Random.Range(0,forTest.Count)]);
            if (spawnPoints.Count < 2)
            {
                foreach (var point in UsedspawnPoints)
                {
                    spawnPoints.Add(point);
                }
                UsedspawnPoints.Clear();
            }
            var random = Random.Range(0, spawnPoints.Count);
            weapon.WeaponGameObject.transform.position = spawnPoints[random];
            UsedspawnPoints.Add(spawnPoints[random]);
            spawnPoints.RemoveAt(random);
        }
        public void DestroyWeapon(Weapon weapon)
        {
            _pool.Release(weapon);
            weapon.WeaponGameObject.transform.parent = inactiveWeapon;
            weapon.WeaponGameObject.SetActive(false);
            if (_droppedWeapons.Count == 0)
                StartCoroutine(RespawnWeapon());
        }
        
        /// <summary>
        /// Return the weapon if it is within the specified range.
        /// </summary>
        /// <param name="pos"> Position </param>
        /// <param name="range"> Range </param>
        /// <returns></returns>
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

        //private void OnDestroy()
        //{
        //    ServiceLocator.Instance.RemoveService(this);
        //}
    }
}
