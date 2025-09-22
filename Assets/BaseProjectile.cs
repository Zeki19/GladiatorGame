using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    protected readonly List<GameObject> Hit = new List<GameObject>();
    [SerializeField] protected LayerMask collisionLayer;
    protected float Damage;
    protected float Speed;
    protected float MaxRange;
    protected Vector3 StartingPosition;
    public Action OnHit;
    public Action<BaseProjectile> OnReturnedToPool;

   
    
    public virtual void SetUp(float damage, LayerMask layer, float speed,float maxRange)
    {
        Speed = speed;
        Damage = damage;
        collisionLayer = layer;
        Hit.Clear();
        MaxRange = maxRange;
        StartingPosition=transform.position;
    }
    
    public void ResetState()
    {
        Hit.Clear();
    }

    private void OnEnable()
    {
        Hit.Clear();
    }

    

    protected bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return (layerMask.value & (1 << obj.layer)) != 0;
    }
}