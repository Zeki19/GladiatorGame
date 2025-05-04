using System;
using Factory.Essentials;
using Unity.VisualScripting;
using UnityEngine;
using Utilitys.Factory.WeaponFactory;
using Weapons;
using Object = UnityEngine.Object;

public class Weapon:IConfigurable<SoWeapon>
{
    

    public GameObject WeaponGameObject;
    private Collider2D weaponCollider2D;
    private string _weaponName;
    public float _baseDamage;
    public float _attackSpeed;
    public float _range;
    private int _durability;
    private int _durabilityStandardLoss;
    private int _durabilityChargeLoss;
    //private enum _attackType{};
    private float _knockbackForce;
    private float _slowPercent;
    private float _changeThreshold;
    private float _chargePerAttack;
    private float _currentDurability;
    private bool _isOnCooldown;
    private bool _canAttack;
    //private Player _owner;
    private float _chargeMeter;
    public Attack _baseSoAttack;
    public Attack _chargeSoAttack;
    public Attack CurrentAttack;
    
    public void Configure(SoWeapon config)
    {
        WeaponGameObject=Object.Instantiate(config.weaponPrefab);
        weaponCollider2D = WeaponGameObject.GetComponent<Collider2D>();
        _weaponName = config.weaponName;
        _baseDamage = config.baseDamage;
        _attackSpeed = config.attackSpeed;
        _range = config.range;
        _durability = config.durability;
        _durabilityStandardLoss = config.durabilityStandardLoss;
        _durabilityChargeLoss = config.durabilityChargeLoss;
        _knockbackForce = config.knockbackForce;
        _slowPercent = config.slowPercent;
        _changeThreshold = config.changeThreshold;
        _chargePerAttack = config.chargePerAttack;
        _baseSoAttack = config.basic.Clone();
    }
    public void SetCollision(bool state)
    {
        weaponCollider2D.enabled = state;
    }
}
