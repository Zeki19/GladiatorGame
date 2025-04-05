using UnityEngine;

public interface IHealthSystem 
{
    float getCurrentHealthPercentage();
    void heal(float HV);
    void fullHeal(float HV);
    void damage(float HV);
    void Kill();
    bool isAlive();
    void onDeadEvent();
    void onHealEvent();
    void onDamageEvent();

}
