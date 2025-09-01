using UnityEngine;

public class Pillar_Standing : MonoBehaviour, IPillar
{
    private IHealth _healthSystem;
    public void StartSpawn(PillarContext context, IHealth healthSystem = null)
    {
        _healthSystem = healthSystem;
        transform.position = context.Origin.position;

        if (_healthSystem != null)
        {
            _healthSystem.OnDead += HandleDeath;
        }

    }
    private void HandleDeath()
    {
        gameObject.SetActive(false);
    }
}
