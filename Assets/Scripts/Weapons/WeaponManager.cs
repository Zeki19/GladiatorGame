using System.Collections.Generic;
using Factory.Essentials;
using UnityEngine;
using Utilitys.Factory.WeaponFactory;

namespace Weapons
{
    public class WeaponManager : MonoBehaviour
    {
        private Factory<Weapon, SoWeapon> _factory;
        public List<SoWeapon>  forTest;
        [SerializeField]private Vector3 startingPos;
        private Dictionary<GameObject, Weapon> _droppedWeapons;
        private void Awake()
        {
            _factory = new Factory<Weapon, SoWeapon>();
            _droppedWeapons = new Dictionary<GameObject, Weapon>();
            ServiceLocator.Instance.RegisterService(this);
        }

        private void Start()
        {
            GetWeapon();
        }

        private void CreateWeapon(SoWeapon weaponConfig)
        {
            var newWeapon = _factory.Create(weaponConfig);
            _droppedWeapons.Add(newWeapon.WeaponGameObject,newWeapon);
            newWeapon.WeaponGameObject.transform.parent = transform;
        }

        private void GetWeapon()
        {
            foreach (var weapon in forTest)
            {
                CreateWeapon( weapon);
            }
            var num=1;
            foreach (var weapon in _droppedWeapons)
            {
                weapon.Key.transform.position = startingPos + new Vector3(2,0,0) * num;
                num++;
            }
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
