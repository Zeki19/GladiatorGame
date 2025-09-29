using System.Collections.Generic;
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

    public int CurrentPhase()
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

    public float[] PhasesHealth()
    {
        int n = _phasesThresholds?.Length ?? 0;
        var result = new float[n + 1];

        int max = (int)_entityHealth.maxHealth;
        
        if (n == 0)
        {
            result[0] = max;
            return result;
        }
        
        int prevCut = 100; // start from 100%
        int accumulated = 0;
        
        for (int i = 0; i < n; i++)
        {
            int cut = Mathf.Clamp(_phasesThresholds[i], 0, 100);
            int widthPct = Mathf.Max(0, prevCut - cut); // % span of this phase

            // Use Floor then fix last segment with remainder to avoid rounding drift
            int segment = Mathf.FloorToInt(max * (widthPct / 100f));
            result[i] = segment;
            accumulated += segment;

            prevCut = cut;
        }

        // Last phase: [last threshold .. 0]
        // Make it the remainder so sum(result) == max exactly
        int remainder = Mathf.Max(0, max - accumulated);
        result[n] = remainder;

        return result;
    }
}
