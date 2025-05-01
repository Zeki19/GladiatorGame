using Interfaces;
using System;
using UnityEngine;

public class PhaseSystem : HealthSystem.HealthSystem
{
    // Serialized fields for inspector assignment
    [SerializeField] public MonoBehaviour speedManagerRaw;
    [SerializeField] public MonoBehaviour damageManagerRaw;

    // Public interface accessors
    public IMove speedManager => speedManagerRaw as IMove;
    public IAttack damageManager => damageManagerRaw as IAttack;


    public bool showTankSettings = true;
     public int tankPercentage;
     public float tankAttack;
     public float tankDefence;
     public float tankSpeed;


    public bool showBalancedSettings = true;
     public int balancedPercentage;
     public float balancedAttack;
     public float balancedDefence;
     public float balancedSpeed;


    public bool showGlassCannonSettings = true;
     public int glassCannonPercentage;
     public float glassCannonAttack;
     public float glassCannonDefence;
     public float glassCannonSpeed;
    public PhaseSystem(float MaxHealth) : base(MaxHealth)
    {

    }
    //void Start()
    //{
    //    OnHeal += _ => CheckState(); // "_" Is for ignoring the float parameter
    //    OnDamage += _ => CheckState();
    //}
    /*void CheckState()
    {
        float healthPercentage = GetCurrentHealthPercentage();
        if (balancedPercentage < healthPercentage)
        {
            damageManager.ModifyDamage(tankAttack);
            SetDefence(tankDefence);
            speedManager.ModifySpeed(tankSpeed);
        }
        else if (glassCannonPercentage < healthPercentage)
        {
            damageManager.ModifyDamage(balancedAttack);
            SetDefence(balancedDefence);
            speedManager.ModifySpeed(balancedSpeed);
        }
        else 
        {
            damageManager.ModifyDamage(glassCannonAttack);
            SetDefence(glassCannonDefence);
            speedManager.ModifySpeed(glassCannonSpeed);
        }*/
    }
//}
