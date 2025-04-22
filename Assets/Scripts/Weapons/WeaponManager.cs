using System;
using System.Collections.Generic;
using Factory.Essentials;
using UnityEditor;
using UnityEngine;
using Utilitys.Factory.WeaponFactory;

public class WeaponManager : MonoBehaviour
{
    private Factory<Weapon, SoWeapon> _factory;

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
    
}
