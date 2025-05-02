using System.Collections.Generic;
using Factory.Essentials;
using UnityEngine;
using Utilitys.Factory.WeaponFactory;

namespace Weapons
{
    public class WeaponManager : MonoBehaviour
    {
        private Factory<Weapon, SoWeapon> _factory;
        public SoWeapon forTest;

        private List<Weapon> _droppedWeapons;
        private void Awake()
        {
            _factory = new Factory<Weapon, SoWeapon>();
            _droppedWeapons = new List<Weapon>();
            ServiceLocator.Instance.RegisterService(this);
        }

        public void CreateWeapon(SoWeapon weaponConfig)
        {
            var newWeapon = _factory.Create(weaponConfig);
            _droppedWeapons.Add(newWeapon);
            newWeapon.WeaponGameObject.transform.parent = transform;
        }

        public Weapon GetWeapon()
        {
            //if (_droppedWeapons!= null)
            //{
            //    return _droppedWeapons[0];
            //}

            CreateWeapon(forTest);
            return _droppedWeapons[0];
        }
    
    }
}
