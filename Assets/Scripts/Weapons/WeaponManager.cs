using System;
using System.Collections.Generic;
using Factory.Essentials;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Utilitys.Factory.WeaponFactory;

namespace Weapons
{
    public class WeaponManager : MonoBehaviour
    {
        private Factory<Weapon, SoWeapon> _factory;
        public List<SoWeapon>  forTest;
        [SerializeField]private Vector3 startingPos;
        private List<Weapon> _droppedWeapons;
        private void Awake()
        {
            _factory = new Factory<Weapon, SoWeapon>();
            _droppedWeapons = new List<Weapon>();
            ServiceLocator.Instance.RegisterService(this);
        }

        private void Start()
        {
            GetWeapon();
        }


        public void CreateWeapon(SoWeapon weaponConfig)
        {
            var newWeapon = _factory.Create(weaponConfig);
            _droppedWeapons.Add(newWeapon);
            newWeapon.WeaponGameObject.transform.parent = transform;
        }
        [ContextMenu("create")]
        public Weapon GetWeapon()
        {
            foreach (var weapon in forTest)
            {
                CreateWeapon( weapon);
            }

            var num=1;
            foreach (var weapon in _droppedWeapons)
            {
                weapon.WeaponGameObject.transform.position = startingPos + new Vector3(2,0,0) * num;
                num++;
            }
            return _droppedWeapons[0];
        }
    
    }
}
