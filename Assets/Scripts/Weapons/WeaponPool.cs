using System;
using System.Collections.Generic;
using System.Linq;
using Factory.Essentials;
using Utilities.Factory.WeaponFactory;
using Object = UnityEngine.Object;

namespace Weapons
{
    public class WeaponPool 
    {
        private Factory<Weapon, SoWeapon> _factory;

        private List< Weapon> _pool=new List<Weapon>();
        private Func<SoWeapon, Weapon> _ceatefunc;
        public WeaponPool(Func<SoWeapon, Weapon> creat)
        {
            _ceatefunc = creat;
        }
        private Weapon Create(SoWeapon config)
        {
            return _ceatefunc(config);
        }
        public Weapon Get(SoWeapon config)
        {
            foreach (var value in from value in _pool where value.WeaponName == config.weaponName let weapon = value select value)
            {
                _pool.Remove(value);
                value.Configure(config);
                return value;
            }
            return Create(config);
        }
        public void Release(Weapon weapon)
        {
            _pool.Add(weapon);
        }
        public void Destroy(ref Weapon weapon)
        {
            if (weapon != null) Object.Destroy(weapon.WeaponGameObject);
            weapon = null;
        }
    }
}
