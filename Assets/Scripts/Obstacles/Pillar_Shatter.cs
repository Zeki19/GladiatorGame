using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pillar_Shatter : MonoBehaviour, IPillar
{
    [SerializeField] private string paintTag = "Scraps";

    [SerializeField] private int damage = 10;
    [SerializeField] private LayerMask damageableLayers;

    private ArenaPainter _painter;

    [SerializeField] private int damageToPlayer = 10;

    private IHealth _healthSystem;

    public void StartSpawn(PillarContext context, IHealth healthSystem = null)
    {
        _healthSystem = healthSystem;
        transform.position = context.Origin.position;

        if (_healthSystem != null)
        {
            _healthSystem.OnDead += HandleDeath;
        }

        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        if (((1 << col.gameObject.layer) & damageableLayers) != 0)
        {
            var entity = col.GetComponent<EntityManager>();
            if (entity != null)
            {
                entity.HealthComponent.TakeDamage(damage);
            }
        }
    }

    private void HandleDeath()
    {

        Destroy(gameObject);
    }

    private static Vector3 GetRandomCardinal()
    {
        int r = Random.Range(0, 4);
        switch (r)
        {
            case 0: return Vector3.right;
            case 1: return Vector3.left;
            case 2: return Vector3.up;
            default: return Vector3.down;
        }
    }
}
