using UnityEngine;

/*
    Cualquier cosa si no se entiende, esta mal, tiene un error, avisenme.
    - Maximo

   HV = HealthValue

    */

public class healthSystem 
{
    private float maxHealth;
    private float currentHealth;
    private float savedCurrentHealth;
    private void Awake()
    {
        currentHealth = maxHealth;
        savedCurrentHealth = currentHealth;
    }
    public float getMaxHealth()
    {
        return maxHealth;
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public float getCurrentHealthPercentage()
    {
        if (currentHealth <= 0)
        {
            return 0;
        }

        return (currentHealth / maxHealth) * 100f;
    }
    public void heal(float HV)
    {
        currentHealth += HV;
    }
    public void fullHeal()
    {
        currentHealth = savedCurrentHealth;
    }

    public void damage(float HV)
    {
        currentHealth -= HV;

        if (currentHealth <= 0)
        {
            kill();
        }
    }
    public void kill()
    {
        //isAlive = false;
        //Dead entity logic.
    }

    public bool isAlive()
    {
       //While entity is alive....
       return currentHealth > 0;
    }

    public void onDeadEvent()
    {
        //update when gameplay is defined
    }

    public void onHealEvent()
    {
        //update when gameplay is defined
    }

    public void onDamageEvent()
    {
        //update when gameplay is defined
    }

}
