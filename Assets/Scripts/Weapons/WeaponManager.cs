using System.Collections.Generic;
using Factory.Essentials;
using UnityEngine;
using Utilities.Factory.WeaponFactory;

namespace Weapons
{
    public class WeaponManager : MonoBehaviour
    {
        #region ForTesting

        [SerializeField]private Vector3 startingPos;//For Testing
        private Factory<Weapon, SoWeapon> _factory;

        #endregion

        [SerializeField] private Transform inactiveWeapon;
        private WeaponPool _pool;
        public List<SoWeapon>  forTest;
        private Dictionary<GameObject, Weapon> _droppedWeapons;
        private void Awake()
        {
            _factory = new Factory<Weapon, SoWeapon>();
            _droppedWeapons = new Dictionary<GameObject, Weapon>();
            _pool = new WeaponPool(_factory.Create);
            ServiceLocator.Instance.RegisterService(this);
        }
        private void Start()
        {
            GetWeapon();
        }
        private Weapon CreateWeapon(SoWeapon weaponConfig)
        {
            var newWeapon = _pool.Get(weaponConfig);
            _droppedWeapons.Add(newWeapon.WeaponGameObject,newWeapon);
            newWeapon.WeaponGameObject.transform.parent = transform;
            return newWeapon;
        }

        public Weapon RequestWeapon(SoWeapon weaponConfig)
        {
            return CreateWeapon(weaponConfig);
        }
        private void GetWeapon()
        {
            foreach (var weapon in forTest)
                CreateWeapon(weapon);
        }
        public void DestroyWeapon(Weapon weapon)
        {
            _pool.Release(weapon);
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
            foreach (var weapon in _droppedWeapons)
                if (Vector3.Distance(pos,weapon.Key.transform.position)<range)
                    return weapon.Value;
            return default;
        }
    }
}
