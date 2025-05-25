using System.Collections.Generic;
using Factory.Essentials;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using Utilitys.Factory.WeaponFactory;

namespace Weapons
{
    public class WeaponManager : MonoBehaviour
    {
        #region ForTesting

        [SerializeField]private Vector3 startingPos;//For Testing
        private Factory<Weapon, SoWeapon> _factory;

        #endregion

        public WeaponPool Pool;
        [SerializeField] private Transform inactiveWeapon;
        public List<SoWeapon>  forTest;
        private Dictionary<GameObject, Weapon> _droppedWeapons;
        private void Awake()
        {
            _factory = new Factory<Weapon, SoWeapon>();
            _droppedWeapons = new Dictionary<GameObject, Weapon>();
            Pool = new WeaponPool(_factory.Create);
            ServiceLocator.Instance.RegisterService(this);
        }

        
        private void Start()
        {
            GetWeapon();
        }

        private void CreateWeapon(SoWeapon weaponConfig)
        {
            var newWeapon = Pool.Get(weaponConfig);
            _droppedWeapons.Add(newWeapon.WeaponGameObject,newWeapon);
            newWeapon.WeaponGameObject.transform.parent = transform;
        }

        private void GetWeapon()
        {
            foreach (var weapon in forTest)
            {
                CreateWeapon(weapon);
            }
            var num=1;
            foreach (var weapon in _droppedWeapons)
            {
                weapon.Key.transform.position = startingPos + new Vector3(2,0,0) * num;
                num++;
            }
        }

        public void DestroyWeapon(Weapon weapon)
        {
            Pool.Release(weapon);
            weapon.WeaponGameObject.transform.parent = inactiveWeapon;
            weapon.WeaponGameObject.SetActive(false);
        }
        /// <summary>
        /// Return the weapon if it is within the specified range.
        /// </summary>
        /// <param name="pos"> Position </param>
        /// <param name="range"> Range </param>
        /// <returns></returns>
        public Weapon PickUpWeaponInRange(Vector3 pos, float range)
        {
            //return (from weapon in _droppedWeapons where Vector3.Distance(pos, weapon.Key.transform.position) < range select weapon.Value).FirstOrDefault();
            foreach (var weapon in _droppedWeapons)
                if (Vector3.Distance(pos,weapon.Key.transform.position)<range)
                    return weapon.Value;
            return default;
        }
    }
}
