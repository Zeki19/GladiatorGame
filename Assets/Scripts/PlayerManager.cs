using System;
using Entities;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

public class PlayerManager : EntityManager
{
    private Weapon _weapon;
    [SerializeField] private float speed;
    [SerializeField] private float maxHp;

    public Weapon weapon
    {
        get => _weapon;
        set => _weapon = value;
    }

    private void Awake()
    {
        HealthSystem = new HealthSystem.HealthSystem(maxHp);
    }
}