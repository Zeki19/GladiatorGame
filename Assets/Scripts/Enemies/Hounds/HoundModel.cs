using System;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

public class HoundModel : MonoBehaviour, IMove, IAttack
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float timePrediction = 0; //For the PURSUIT steering
    [SerializeField] private float attackRange;
    public Rigidbody2D _rb;
    public Vector2 Position => transform.position;
    public float MoveSpeed => moveSpeed;
    public float TimePredict => timePrediction;
    public float AttackRange => attackRange;
    public void ModifySpeed(float speed)
    {
        throw new NotImplementedException();
    }

    public void Move(Vector2 dir)
    {
        _rb.linearVelocity = dir * (moveSpeed);
    }

    public void Dash(float dashForce)
    {
        throw new NotImplementedException();
    }

    
    public void Attack()
    {
        throw new NotImplementedException();
    }

    public Action OnAttack { get; set; }
}
