using UnityEngine;

public class PhaseSystem : HealthSystem
{
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
    #region EVENTS
    //OnDead
    //OnHeal
    //OnDamage
    #endregion

    void Start()
    {
        
    }
    void CheckState()
    {
        float healthPercentage = GetCurrentHealthPercentage();
        if (tankPercentage < healthPercentage)
        {
            //Tank
        }
        else if (balancedPercentage < healthPercentage)
        {
            //Balanced
        }
        else 
        {
            //GlassCannon
        }
    }
}
