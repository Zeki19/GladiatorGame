using System;
using Interfaces;
using UnityEngine;

public class HoundModel : MonoBehaviour, IMove, IAttack
{
    [SerializeField] private float moveSpeed = 5f;
    
    
    public void ModifySpeed(float speed)
    {
        throw new NotImplementedException();
    }

    public void Move(float moveSpeed)
    {
        
    }

    public void Dash(float dashForce)
    {
        throw new NotImplementedException();
    }

    public Vector2 Position { get; }
    public void Attack()
    {
        throw new NotImplementedException();
    }

    public Action OnAttack { get; set; }
}
