using UnityEngine;

public class PhaseSystem
{
    private int[] _phasesThresholds;
    private IHealth _entityHealth;
    public PhaseSystem(int[] phases, IHealth entityHealth)
    {
        _phasesThresholds = phases;
        _entityHealth = entityHealth;
    }

    public int currentPhase()
    {

        for (int i = 0; i < _phasesThresholds.Length; i++) 
        {
            if (_entityHealth.GetCurrentHealthPercentage() > _phasesThresholds[i])
            {
                return i + 1;
            }
        }
        
        return _phasesThresholds.Length + 1;
    }



}
